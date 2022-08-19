using ArcticFoxEngine.Rendering;

namespace ArcticFoxEngine.Services.TextureManager;

public interface ITextureManager
{
    public ITexture2D LoadTexture(string path);
}
