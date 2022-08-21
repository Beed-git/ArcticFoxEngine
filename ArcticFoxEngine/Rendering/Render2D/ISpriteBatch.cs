using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Camera;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public interface ISpriteBatch
{
    public bool Drawing { get; }
    public void BeginDraw(ICamera camera);
    public void BeginDraw(Matrix4 mvp);
    public void DrawRectangle(ITexture2D texture, Rectangle destination, Color color);
    public void DrawRectangle(ITexture2D texture, Rectangle destination, Rectangle source, Color color);
    public void EndDraw();
}
