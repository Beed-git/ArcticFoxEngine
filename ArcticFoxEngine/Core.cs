using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ArcticFoxEngine;

public class Core : IDisposable
{
    private ServiceProvider _provider;

    public Core(IServiceCollection services)
    {
        var serviceManager = new ServiceProviderBuilder(services);

        // Get all services.
        var initServices = serviceManager.GetServices<IInitService>();
        var updateServices = serviceManager.GetServices<IUpdateService>();
        var renderServices = serviceManager.GetServices<IRenderService>();

        // Initialise services.
        foreach (var service in initServices)
        {
            service.Init();
        }

        _provider = serviceManager.Build();

        var window = _provider.GetService<IWindow>();
        window?.AddUpdateServices(updateServices);
        window?.AddRenderServices(renderServices);
    }

    public void Run()
    {
        var window = _provider.GetService<IWindow>();
        window?.Run();
    }

    public void Dispose()
    {
        _provider.Dispose();
    }
}
