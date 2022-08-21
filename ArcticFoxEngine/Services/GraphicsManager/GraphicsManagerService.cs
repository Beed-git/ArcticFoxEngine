using ArcticFoxEngine.Math;
using OpenTK.Windowing.Desktop;

namespace ArcticFoxEngine.Services.GraphicsManager;

public class GraphicsManagerService : IGraphicsManager
{
    private readonly GameWindow _window;

    public GraphicsManagerService(GameWindow window)
    {
        _window = window;
        Width = _window.Size.X;
        Height = _window.Size.Y;

        _window.Resize += (s) => OnResize?.Invoke(this, new Point(s.Width, s.Height));
    }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public event EventHandler<Point>? OnResize;

    public void Resize(int width, int height)
    {
        _window.Size = new(width, height);
        OnResize?.Invoke(this, new Point(width, height));
    }
}
