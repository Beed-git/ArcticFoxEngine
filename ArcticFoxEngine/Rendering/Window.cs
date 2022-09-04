using ArcticFoxEngine.Math;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;

namespace ArcticFoxEngine.Rendering;

public class Window : IDisposable
{
    private readonly WindowSettings _settings;
    private readonly IWindow _window;

    public Window(WindowSettings settings)
    {
        _settings = settings;
        var options = WindowOptions.Default;
        options.Title = settings.Title;
        options.Size = new Silk.NET.Maths.Vector2D<int>(settings.Size.x, settings.Size.y);

        _window = SilkWindow.Create(options);
    }

    public void Run()
    {
        _window.Run();
    }
    
    public void Dispose()
    {
    }
}
