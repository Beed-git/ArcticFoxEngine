using System.Collections.ObjectModel;
using ArcticFoxEngine.Logging;

namespace ArcticFoxEngine.Rendering.Resources;

public class ResourceManagerBuilder
{
    private readonly ILogger _logger;
    private readonly Dictionary<Type, IResourceLoader> _loaders;

    public ResourceManagerBuilder(ILogger logger)
    {
        _logger = logger;
        _loaders = new Dictionary<Type, IResourceLoader>();
    }

    public ILogger Logger => _logger;
    public ReadOnlyDictionary<Type, IResourceLoader> Loaders => new(_loaders);

    public ResourceManagerBuilder WithLoader<T>(IResourceLoader<T> loader)
    {
        if (_loaders.ContainsKey(typeof(T)))
        {
            _logger.Log($"Loader for type {typeof(T).Name} already exists.");
        }
        else
        {
            _loaders.Add(typeof(T), loader);
        }
        return this;
    }

    public ResourceManager Build()
    {
        return new ResourceManager(this);
    }
}
