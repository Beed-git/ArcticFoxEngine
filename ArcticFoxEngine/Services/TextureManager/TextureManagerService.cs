using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.OpenGL;
using StbImageSharp;

namespace ArcticFoxEngine.Services.TextureManager;

public class TextureManagerService : ITextureManager, IDisposable
{
    private Dictionary<string, ITexture2D> _textures;

    public TextureManagerService()
    {
        _textures = new Dictionary<string, ITexture2D>();
    }

    public ITexture2D LoadTexture(string path)
    {
        if (_textures.TryGetValue(path, out var outTexture))
        {
            return outTexture;
        }

        StbImage.stbi_set_flip_vertically_on_load(1);

        using var stream = File.OpenRead(path);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        var texture = new Texture2D(image.Width, image.Height);
        texture.SetData(new Rectangle(0, 0, image.Width, image.Height), image.Data);

        _textures.Add(path, texture);
        return texture;
    }

    public void Dispose()
    {
        foreach (var texture in _textures)
        {
            texture.Value.Dispose();
        }
    }
}
