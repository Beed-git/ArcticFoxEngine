using ArcticFoxEngine.Math;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public interface ITextureBatch : IDisposable
{
    public bool Drawing { get; }
    public int MaxSprites { get; }
    public void BeginDraw(Matrix4? projection = null);
    public void DrawRectangle(Rectangle destination, Rectangle source, Color color);
    public void EndDraw();
}
