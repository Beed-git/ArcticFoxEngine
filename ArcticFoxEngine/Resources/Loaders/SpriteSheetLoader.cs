using ArcticFoxEngine.Rendering.Sprites;
using ArcticFoxEngine.Resources;
using YamlDotNet.Serialization;

namespace ArcticFoxEngine.Resources.Loaders;

public class SpriteSheetLoader : IResourceLoader<SpriteSheet>
{
    private readonly IDeserializer _deserializer;

    public SpriteSheetLoader(IDeserializer deserializer)
    {
        _deserializer = deserializer;
    }

    SpriteSheet? IResourceLoader<SpriteSheet>.LoadResource(FileManager fileManager, string path)
    {
        path = Path.Combine(fileManager.AssetFolder, path);
        if (!File.Exists(path))
        {
            return null;
        }

        var text = File.ReadAllText(path);
        return _deserializer.Deserialize<SpriteSheet>(text);
    }
}
