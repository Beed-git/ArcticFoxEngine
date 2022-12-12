using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Textures;
using StbImageSharp;

namespace ArcticFoxEngine.Resources.Loaders;

internal class TextureLoader : IResourceLoader<Texture2D>
{
    private readonly GraphicsDevice _graphicsDevice;

    public TextureLoader(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
    }

    // TODO: This currently assumes that the resource is already in the 
    //       engine specific format - need to create a pipeline to convert
    //       from standard formats (jpg, png, tga) to the engine resource
    //       format.
    public Texture2D? LoadResource(FileManager fileManager, string path)
    {
        path = Path.Combine(fileManager.AssetFolder, path);
        if (!File.Exists(path))
        {
            return null;
        }

        StbImage.stbi_set_flip_vertically_on_load(1);

        using var stream = File.OpenRead(path);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        var texture = new Texture2D(_graphicsDevice, (uint)image.Width, (uint)image.Height);
        texture.SetData(texture.Bounds, image.Data);
        return texture;
    }
}
