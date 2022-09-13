using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Rendering.Resources;
using System.Reflection;

namespace ArcticFoxEngine.Rendering.Resources;

public class ResourceManager
{
    private const string _assetFolder = "assets";

    private readonly string _rootPath;
    private readonly ILogger _logger;

    private readonly Dictionary<Type, IResourceStore> _stores;

    public ResourceManager(ResourceManagerBuilder builder)
    {
        _logger = builder.Logger;

        var location = Assembly.GetExecutingAssembly().Location;
        var parent = Directory.GetParent(location);
        if (parent is null)
        {
            throw new Exception("Failed to find engine folder.");
        }
        _rootPath = Path.Combine(parent.FullName, _assetFolder);
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }

#if DEBUG
        _logger.Log($"Asset directory is {_rootPath}");
#endif

        _stores = new Dictionary<Type, IResourceStore>();
        foreach (var (type, loader) in builder.Loaders)
        {
            var resourceType = typeof(ResourceStore<>).MakeGenericType(type);
            var store = Activator.CreateInstance(resourceType, _rootPath, _logger, loader);
            if (store is not null)
            {
                _stores.Add(type, (IResourceStore)store);
            }
            else
            {
                _logger.Log($"Failed to create resource storage for type {type.Name}.");
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
            storage = new ResourceStore<T>(_rootPath, _logger, null);
            _stores.Add(type, storage);
        }
        return storage;
    }
}
