using ArcticFoxEngine.Math;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public interface ISpriteBatch : IDisposable
{
    public void BeginDraw(Matrix4? projection = null);
    public void DrawRectangle(Rectangle destination, Color color);
    public void EndDraw();
}
