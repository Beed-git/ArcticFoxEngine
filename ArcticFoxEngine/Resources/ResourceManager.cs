using ArcticFoxEngine.Logging;

namespace ArcticFoxEngine.Resources;

internal class ResourceManager : IDisposable
{
    private readonly FileManager _fileManager;
    private readonly ILogger? _logger;

    private readonly Dictionary<Type, IResourceStore> _stores;

    public ResourceManager(FileManager fileManager, ILogger? logger)
    {
        _fileManager = fileManager;
        _logger = logger;

        _stores = new Dictionary<Type, IResourceStore>();
    }

    public ResourceManager AddLoader<T>(IResourceLoader<T> resourceLoader)
    {
        var type = typeof(T);
        if (_stores.ContainsKey(type))
        {
            _logger?.Error($"Resource storage for type '{type.Name}' already exists. A resource has likely already been created for this type.");
        }
        else
        {
            var resourceType = typeof(ResourceStore<>).MakeGenericType(typeof(T));
            var store = Activator.CreateInstance(resourceType, _fileManager, resourceLoader);
            if (store is not null)
            {
                _stores.Add(type, (IResourceStore)store);
            }
            else
            {
                _logger?.Warn($"Failed to create resource storage for type '{type.Name}'");
            }
        }
        return this;
    }

    public Resource<T> GetResource<T>(string path) where T : class
    {
        if (!Uri.IsWellFormedUriString(path, UriKind.Relative))
        {
            throw new Exception($"Invalid path! {path}");
        }

        var storage = GetOrCreateStore<T>();
        return storage.GetResource(path);
    }

    private ResourceStore<T> GetOrCreateStore<T>() where T : class
    {
        var type = typeof(T);
        ResourceStore<T> storage;
        if (_stores.TryGetValue(type, out var store))
        {
            storage = (ResourceStore<T>)store;
        }
        else
        {
            storage = new ResourceStore<T>(_fileManager, null);
            _stores.Add(type, storage);
        }
        return storage;
    }

    public void Dispose()
    {
        foreach (var store in _stores.Values)
        {
            store.Dispose();
        }
    }
}
