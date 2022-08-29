using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections;

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

    public ServiceProvider Build()
    {
        return _provider;
    }
}