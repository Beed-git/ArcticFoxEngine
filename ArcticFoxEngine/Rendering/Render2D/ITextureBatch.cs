using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.OpenGL;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public interface ITextureBatch : IDisposable
{
    public bool Drawing { get; }
    public int MaxSprites { get; }
    public void BeginDraw(ITexture2D texture);
    public void DrawRectangle(Rectangle destination, Rectangle source, Color color);
    public void EndDraw(Shader shader, Matrix4 mvp);
}
