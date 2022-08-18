using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.Game;
using ArcticFoxEngine.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ArcticFoxEngine;

public class Core : IDisposable
{
    private ServiceProvider _provider;

    public Core(IServiceCollection serviceDescriptors)
    {
        var options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };

        // Get all services which will need to be updated.
        var types = new HashSet<Type>();
        foreach (var service in serviceDescriptors)
        {
            types.Add(service.ServiceType);
        }

        _provider = serviceDescriptors.BuildServiceProvider(options);

        var logger = _provider.GetService<ILogger<Core>>();

        // Get all services.
        var initServices = new List<IInitService>();
        var updateServices = new List<IUpdateThreadService>();
        var renderServices = new List<IRenderThreadService>();

        foreach (var type in types)
        {
            if (type.ContainsGenericParameters)
            {
                logger?.LogWarning($"Cannot add IUpdateable or use injection on generic service of type {type}.");
                continue;
            }
            var services = _provider.GetServices(type);
            foreach (var service in services)
            {
                if (service is null)
                {
                    continue;
                }

                AddService(initServices, service);
                AddService(updateServices, service);
                AddService(renderServices, service);
                InjectServices(_provider, service, logger);
            }
        }

        // Initialise services.
        foreach (var service in initServices)
        {
            service.Init();
        }

        // Prepare update services.
        var gameManager = _provider.GetService<IGameManager>();
        gameManager?.AddUpdateServices(updateServices);

        // Prepare render services.
        var window = _provider.GetService<IWindow>();
        window?.AddRenderServices(renderServices);
    }

    private static void InjectServices(ServiceProvider provider, object service, ILogger? logger)
    {
        var type = service.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (Attribute.IsDefined(field, typeof(InjectAttribute)))
            {
                var attributes = (InjectAttribute[])field.GetCustomAttributes(typeof(InjectAttribute), true);
                if (attributes.Length > 1)
                {
                    logger?.LogWarning($"Multiple inject attributes applied to field {field.Name} in type {type}.\nOnly the first one will be used.");
                }
                var attribute = attributes.First();
                var t = field.FieldType;
                if (attribute.Requirement == Requirement.Optional)
                {
                    var s = provider.GetService(t);
                    field.SetValue(service, s);
                }
                else if (attribute.Requirement == Requirement.Required)
                {
                    var rs = provider.GetRequiredService(t);
                    field.SetValue(service, rs);
                }
                else
                {
                    throw new Exception($"Invalid requirement applied to property {field.Name} in type {type}.");
                }
            }
        }

        foreach (var property in properties)
        {
            if (Attribute.IsDefined(property, typeof(InjectAttribute)))
            {
                var attributes = (InjectAttribute[])property.GetCustomAttributes(typeof(InjectAttribute), true);
                if (attributes.Length > 1)
                {
                    logger?.LogWarning($"Multiple inject attributes applied to property {property.Name} in type {type}.\nOnly the first one will be used.");
                }
                var attribute = attributes.First();
                var t = property.PropertyType;
                if (attribute.Requirement == Requirement.Optional)
                {
                    var s = provider.GetService(t);
                    property.SetValue(service, s);
                }
                else if (attribute.Requirement == Requirement.Required)
                {
                    var rs = provider.GetRequiredService(t);
                    property.SetValue(service, rs);
                }
                else
                {
                    throw new Exception($"Invalid requirement applied to property {property.Name} in type {type}.");
                }
            }
        }
    }

    private static void AddService<T>(IList<T> list, object service)
    {
        var type = service.GetType();
        if (type.IsAssignableTo(typeof(T)))
        {
            list.Add((T)service);
        }
    }

    public void Run()
    {
        var gameManager = _provider.GetService<IGameManager>();
        var window = _provider.GetService<IWindow>();

        gameManager?.Run();
        window?.Run();
    }

    public void Dispose()
    {
        _provider.Dispose();
    }
}
