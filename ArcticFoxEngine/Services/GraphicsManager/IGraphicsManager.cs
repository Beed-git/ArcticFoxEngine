using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Services.GraphicsManager;

public interface IGraphicsManager
{
    public int Width { get; }
    public int Height { get; }

    public event EventHandler<Point> OnResize;
    public void Resize(int width, int height);
}
