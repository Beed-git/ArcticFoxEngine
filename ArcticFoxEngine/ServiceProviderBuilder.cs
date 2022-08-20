using ArcticFoxEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Reflection;

namespace ArcticFoxEngine;

internal class ServiceProviderBuilder
{
    private readonly ILogger<ServiceProviderBuilder>? _logger;

    private readonly List<Type> _types;
    private readonly List<(IList, Type)> _getServicesLists;
    private readonly ServiceProvider _provider;

    public ServiceProviderBuilder(IServiceCollection services)
    {
        _getServicesLists = new List<(IList, Type)>();

        var options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };

        // Get all services types.
        var types = new HashSet<Type>();
        foreach (var service in services)
        {
            types.Add(service.ServiceType);
        }
        _types = types.ToList();

        _provider = services.BuildServiceProvider(options);
        _logger = _provider.GetService<ILogger<ServiceProviderBuilder>>();

        foreach (var type in _types)
        {
            if (type.IsGenericType)
            {
                _logger?.LogWarning($"Can't check if type {type.Name} has any inject arguments as type is generic.");
                continue;
            }

            var providerServices = _provider.GetServices(type);
            foreach (var service in providerServices)
            {
                if (service is not null)
                {
                    InjectServices(service);
                }
            }
        }
    }

    public IEnumerable<T> GetServices<T>()
    {
        var list = new List<T>();
        foreach (var type in _types)
        {
            if (type.IsGenericType)
            {
                continue;
            }
            var services = _provider.GetServices(type);
            foreach (var service in services)
            {
                if (service is not null && service.GetType().IsAssignableTo(typeof(T)))
                {
                    list.Add((T)service);
                }
            }
        }
        return list;
    }

    private void InjectServices(object service)
    {
        var type = service.GetType();
        var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m is PropertyInfo or FieldInfo)
            .ToList();

        foreach (var member in members)
        {
            if (!member.IsDefined(typeof(InjectAttribute)))
            {
                continue;
            }
            var attributes = member.GetCustomAttributes<InjectAttribute>();
            if (attributes.Count() > 1)
            {
                _logger?.LogWarning($"Multiple inject attributes applied to member {member.Name} in type {type}.\nOnly the first one will be used.");
            }
            var attribute = attributes.First();

            if (member.MemberType is MemberTypes.Field)
            {
                var field = (FieldInfo)member;
                var t = field.FieldType;
                if (attribute.Requirement == Requirement.Optional)
                {
                    var s = _provider.GetService(t);
                    field.SetValue(service, s);
                }
                else if (attribute.Requirement == Requirement.Required)
                {
                    var rs = _provider.GetRequiredService(t);
                    field.SetValue(service, rs);
                }
            }
            else if (member.MemberType is MemberTypes.Property)
            {
                var property = (PropertyInfo)member;
                var t = property.PropertyType;
                if (attribute.Requirement == Requirement.Optional)
                {
                    var s = _provider.GetService(t);
                    property.SetValue(service, s);
                }
                else if (attribute.Requirement == Requirement.Required)
                {
                    var rs = _provider.GetRequiredService(t);
                    property.SetValue(service, rs);
                }
            }
            else throw new InvalidCastException($"Inject attribute applied to invalid member of type {member.MemberType}");
        }
    }

        public ServiceProvider Build()
    {
        return _provider;
    }
}