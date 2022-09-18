using ArcticFoxEngine.Resources;
using StbImageSharp;

namespace ArcticFoxEngine.Rendering.Textures;

public class TextureLoader : IResourceLoader<Texture2D>
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
    public Texture2D? LoadResource(string path)
    {
        StbImage.stbi_set_flip_vertically_on_load(1);

        if (!File.Exists(path))
        {
            return null;
        }
        using var stream = File.OpenRead(path);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        var texture = new Texture2D(_graphicsDevice, (uint)image.Width, (uint)image.Height);
        texture.SetData(texture.Bounds, image.Data);
        return texture;
    }
}
