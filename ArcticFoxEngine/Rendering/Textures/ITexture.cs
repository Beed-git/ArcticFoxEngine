using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Textures;

public interface ITexture : IDisposable
{
    public uint Width { get; }
    public uint Height { get; }
    public Rectangle Bounds { get; }
}
