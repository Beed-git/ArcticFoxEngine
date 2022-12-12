using ArcticFoxEngine.EC;
using ArcticFoxEngine.EC.Converters;
using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Rendering;
using YamlDotNet.Serialization;

namespace ArcticFoxEngine.Resources.Loaders;

internal class SceneLoader : IResourceLoader<Scene>
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly ResourceManager _resourceManager;
    private readonly IDeserializer _deserializer;

    public SceneLoader(ResourceManager resourceManager, IDeserializer deserializer, GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _resourceManager = resourceManager;
        _deserializer = deserializer;
    }

    public Scene? LoadResource(FileManager fileManager, string path)
    {
        path = Path.Combine(fileManager.AssetFolder, path);
        if (!File.Exists(path))
        {
            return null;
        }
        
        var sceneText = File.ReadAllText(path);
        var sceneModel = _deserializer.Deserialize<SceneModel>(sceneText);

        var scene = SceneModelConverter.ToScene(_graphicsDevice, _resourceManager, sceneModel);
        return scene;
    }
}
