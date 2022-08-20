using ArcticFoxEngine.Math;
using OpenTK.Graphics.OpenGL;

namespace ArcticFoxEngine.Rendering;

public interface ITexture2D : IDisposable
{
    public int Width { get; }
    public int Height { get; }
    public Rectangle Bounds { get; }
    public void Bind(TextureUnit unit = TextureUnit.Texture0);
    public void SetData(Rectangle bounds, byte[] data);
}
