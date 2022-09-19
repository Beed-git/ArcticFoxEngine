using System.Collections.ObjectModel;
using ArcticFoxEngine.Logging;

namespace ArcticFoxEngine.Resources;

public class ResourceManagerBuilder
{
    private readonly Dictionary<Type, IResourceLoader> _loaders;

    public ResourceManagerBuilder(ProjectManager projectManager)
    {
        ProjectManager = projectManager;
        _loaders = new Dictionary<Type, IResourceLoader>();
    }

    public ProjectManager ProjectManager { get; private set; }
    public ILogger? Logger { get; private set; }
    public ReadOnlyDictionary<Type, IResourceLoader> Loaders => new(_loaders);

    public ResourceManagerBuilder WithLogger(ILogger? logger)
    {
        Logger = logger;
        return this;
    }

    public ResourceManagerBuilder WithLoader<T>(IResourceLoader<T> loader)
    {
        if (_loaders.ContainsKey(typeof(T)))
        {
            Logger?.Log($"Loader for type {typeof(T).Name} already exists.");
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
