using ArcticFoxEngine.Logging;

namespace ArcticFoxEngine.Resources;

public class ResourceManager : IDisposable
{
    private readonly ProjectManager _projectManager;
    private readonly ILogger? _logger;

    private readonly Dictionary<Type, IResourceStore> _stores;

    public ResourceManager(ResourceManagerBuilder builder)
    {
        _projectManager = builder.ProjectManager;
        _logger = builder.Logger;

        _logger?.Log($"Asset directory is {builder.ProjectManager.AssetFolder}");

        _stores = new Dictionary<Type, IResourceStore>();
        foreach (var (type, loader) in builder.Loaders)
        {
            var resourceType = typeof(ResourceStore<>).MakeGenericType(type);
            var store = Activator.CreateInstance(resourceType, _projectManager, _logger, loader);
            if (store is not null)
            {
                _stores.Add(type, (IResourceStore)store);
            }
            else
            {
                _logger?.Log($"Failed to create resource storage for type {type.Name}.");
            }
        }
    }

    public Resource<T> CreateResource<T>(string path, T data) where T : class
    {
        if (!Uri.IsWellFormedUriString(path, UriKind.Relative))
        {
            throw new Exception($"Invalid path! {path}");
        }

        var storage = GetOrCreateStore<T>();
        return storage.CreateResource(path, data);
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

    public void DeleteResource<T>(string path) where T : class
    {
        if (!Uri.IsWellFormedUriString(path, UriKind.Relative))
        {
            throw new Exception($"Invalid path! {path}");
        }

        var storage = GetOrCreateStore<T>();
        storage.DeleteResource(path);
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
            storage = new ResourceStore<T>(_projectManager, _logger, null);
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
