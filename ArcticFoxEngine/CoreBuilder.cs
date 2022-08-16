using ArcticFoxEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArcticFoxEngine;

public class CoreBuilder
{
    private ServiceCollection _services;
    private HashSet<Type> _updatableServces;

    public CoreBuilder()
    {
        _services = new ServiceCollection();
        _updatableServces = new HashSet<Type>();
    }

    public Core Build()
    {
        return new Core(_services, _updatableServces.ToList());
    }

    public CoreBuilder RegisterTransient<TService>() where TService : class
    {
        _services.AddTransient<TService>();
        RegisterUpdatable<TService>();
        return this;
    }

    public CoreBuilder RegisterTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _services.AddTransient<TService, TImplementation>();
        RegisterUpdatable<TImplementation>();
        return this;
    }

    public CoreBuilder RegisterSingleton<TService>() where TService : class
    {
        _services.AddTransient<TService>();
        RegisterUpdatable<TService>();
        return this;
    }

    public CoreBuilder RegisterSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _services.AddTransient<TService, TImplementation>();
        RegisterUpdatable<TImplementation>();
        return this;
    }

    private void RegisterUpdatable<T>() 
    {
        var t = typeof(T);
        if (t.IsAssignableTo(typeof(IUpdateService)))
        {
            _updatableServces.Add(t);
        }
    }
}
