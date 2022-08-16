using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ArcticFoxEngine;

public class Core : IDisposable
{
    private Thread _thread;

    private ServiceProvider _provider;
    private IEnumerable<IUpdateService> _updateServices;

    private ILogger? _logger;

    internal Core(IServiceCollection serviceDescriptors, IEnumerable<Type> updateServices)
    {
        var options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        };

        _provider = serviceDescriptors.BuildServiceProvider(options);

        var services = new List<IUpdateService>();
        foreach (var service in updateServices)
        {
            if (service.IsAssignableTo(typeof(IUpdateService)))
            {
                var updates = _provider.GetServices(service);
                foreach (var u in updates)
                {
                    var s = (IUpdateService?)u;
                    if (u is not null && !services.Contains(s))
                    {
                        services.Add(s);
                    }
                }
            }
        }
        _updateServices = services;

        _logger = _provider.GetService<ILogger>();
    }

    public bool Running { get; set; }

    public void Run()
    {
        _thread = new Thread(RunThread);
        _thread.Start();
    }

    private void RunThread()
    {
        var watch = new Stopwatch();
        Running = true;

        Init();

        _logger?.Debug("Starting update cycle.");
        int lastTime = 0;
        watch.Start();
        while (Running)
        {
            int currentTime = watch.Elapsed.Milliseconds;
            Update(currentTime - lastTime);
            lastTime = currentTime;
        }
    }

    private void Init()
    {
        _logger?.Debug("Beginning init");

        foreach (var service in _updateServices)
        {
            service.Init();
        }

        _logger?.Debug("Init finished.");
    }

    private void Update(int dt)
    {
        foreach (var service in _updateServices)
        {
            service.Update(dt);
        }
    }

    public void Dispose()
    {
        _logger?.Debug("Beginning shutdown");
        _provider.Dispose();
        _thread.Join();
        _logger?.Debug("Shutdown finished");
    }
}
