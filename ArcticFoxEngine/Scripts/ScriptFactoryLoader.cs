using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Resources;
using System.Reflection;

namespace ArcticFoxEngine.Scripts;

internal class ScriptFactoryLoader  : IResourceLoader<ScriptFactory>
{
    private readonly ILogger? _logger;
    private Dictionary<string, Type> _types;

    public ScriptFactoryLoader (ILogger? logger, IEnumerable<Assembly> scriptAssemblies)
    {
        _logger = logger;
        _types = new Dictionary<string, Type>();

        foreach (var assembly in scriptAssemblies)
        {
            var types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(BaseScript)));
            foreach (var type in types)
            {
                if (type.FullName is not null)
                {
                    _types.Add(type.FullName, type);
                }
                else
                {
                    if (type.IsGenericType)
                    {
                        _logger?.Error("BaseScript cannot be a generic class.");
                    }
                    else
                    {
                        _logger?.Warn($"Failed to get full name for type '{type.Name}'");
                    }
                }
            }
        }
    }

    public ScriptFactory? LoadResource(string path)
    {
        if (_types.TryGetValue(path, out var type))
        {
            return new ScriptFactory(type);
        }
        _logger?.Warn($"Failed to find script '{path}'");
        return null;
    }
}
