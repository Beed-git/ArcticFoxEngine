using ArcticFoxEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArcticFoxEngine;

public class Core : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly ILogger<Core>? _logger;

    private readonly IEnumerable<IInitService> _initServices;
    private readonly IEnumerable<IUpdateService> _updateServices;
    private readonly IEnumerable<IRenderService> _renderServices;

    public Core(IServiceCollection services)
    {
        var serviceManager = new ServiceProviderBuilder(services);

        // Get all services.
        _initServices = serviceManager.GetServices<IInitService>();
        _updateServices = serviceManager.GetServices<IUpdateService>();
        _renderServices = serviceManager.GetServices<IRenderService>();

        _provider = serviceManager.Build();
        _logger = _provider.GetService<ILogger<Core>>();
    }

    public void Run()
    {
        var window = _provider.GetService<GameWindow>();
        if (window is not null)
        {
            var eventHandler = new CoreWindowEventHandler(window, _initServices, _updateServices, _renderServices);

            window.Load += eventHandler.OnLoad;
            window.Unload += eventHandler.OnUnload;
            window.UpdateFrame += (e) => eventHandler.OnUpdate(e.Time);
            window.RenderFrame += (e) => eventHandler.OnRender(e.Time);
            window.Resize += (e) => eventHandler.OnResize(new(e.Width, e.Height));
            window.KeyDown += eventHandler.OnKeyDown;
            window.KeyUp += eventHandler.OnKeyUp;

            window.Run();
        }
        else
        {
            _logger?.LogWarning("No window attached, engine cannot currently be run from core without a window.");
        }
    }

    public void Dispose()
    {
        _provider.Dispose();
    }
}
