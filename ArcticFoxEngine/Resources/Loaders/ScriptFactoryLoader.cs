using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Resources;
using ArcticFoxEngine.Scripts;

namespace ArcticFoxEngine.Resources.Loaders;

internal class ScriptFactoryLoader : IResourceLoader<ScriptFactory>
{
    private readonly ILogger? _logger;
    private readonly Dictionary<string, Type> _types;

    public ScriptFactoryLoader(ILogger? logger, IEnumerable<Type> scriptTypes)
    {
        _logger = logger;
        _types = new Dictionary<string, Type>();

        foreach (var type in scriptTypes)
        {
            if (type.FullName is not null && !type.IsAbstract && !type.IsGenericType && type.IsAssignableTo(typeof(BaseScript)))
            {
                _types.Add(type.Name, type);
            }
            else
            {
                _logger?.Error($"Invalid script type provided to ScriptFactoryLoader: {type.Name}");
            }
        }
    }

    public ScriptFactory? LoadResource(FileManager fileManager, string path)
    {
        if (_types.TryGetValue(path, out var type))
        {
            return new ScriptFactory(type);
        }
        _logger?.Warn($"Failed to find script '{path}'");
        return null;
    }
}
