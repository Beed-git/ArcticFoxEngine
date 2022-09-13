using ArcticFoxEngine.Logging;

namespace ArcticFoxEngine.Rendering.Resources;

public interface IResourceStore
{
}

public class ResourceStore<T> : IResourceStore where T : class
{
    private readonly string _rootPath;
    private readonly ILogger _logger;

    private readonly IResourceLoader<T>? _loader;
    private readonly Dictionary<string, Resource<T>> _resources;

    public ResourceStore(string rootPath, ILogger logger, IResourceLoader<T>? loader)
    {
        _rootPath = rootPath;
        _logger = logger;
        _loader = loader;
        _resources = new Dictionary<string, Resource<T>>();
    }

    public Resource<T> GetResource(string path)
    {
        if (_resources.TryGetValue(path, out var resource))
        {
            return resource;
        }
        resource = new Resource<T>(path);
        if (_loader is not null)
        {
            resource.Data = _loader.LoadResource(Path.Combine(_rootPath, path));
        }
        _resources.Add(path, resource);
        return resource;
    }

    public Resource<T> CreateResource(string path, T data)
    {
        if (_resources.TryGetValue(path, out var resource))
        {
            _logger.Log($"Resource already exists at path {path}.");
            return resource;
        }
        resource = new Resource<T>(path, data);
        _resources.Add(path, resource);
        return resource;
    }

    public void DeleteResource(string path)
    {
        if (_resources.ContainsKey(path))
        {
            _resources.Remove(path);
        }
        else
        {
            _logger.Log($"Attempting to delete not-existant resource at path {path}.");
        }
    }
}

