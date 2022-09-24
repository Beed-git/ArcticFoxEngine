using ArcticFoxEngine.Logging;

namespace ArcticFoxEngine.Resources;

internal interface IResourceStore : IDisposable
{
}

internal class ResourceStore<T> : IResourceStore, IDisposable where T : class
{
    private readonly FileManager _projectManager;
    private readonly ILogger? _logger;

    private readonly IResourceLoader<T>? _loader;
    private readonly Dictionary<string, Resource<T>> _resources;

    public ResourceStore(FileManager projectManager, ILogger? logger, IResourceLoader<T>? loader)
    {
        _projectManager = projectManager;
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
            resource.Data = _loader.LoadResource(Path.Combine(_projectManager.AssetFolder, path));
        }
        _resources.Add(path, resource);
        return resource;
    }

    public void Dispose()
    {
        if (typeof(T).IsAssignableTo(typeof(IDisposable)))
        {
            foreach (var resource in _resources.Values)
            {
                if (resource.Data is not null)
                {
                    ((IDisposable)resource.Data).Dispose();
                }
            }
        }
    }
}

