using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ArcticFoxEngine.Services.Window;

internal class Window : IWindow
{
    private GameWindow? _window;
    private IEnumerable<IWindowEventHandler> _handlers;
    private HashSet<IRenderService> _renderServices;
    private HashSet<IUpdateService> _updateServices;

    public Window(IEnumerable<IWindowEventHandler> handlers)
    {
        _handlers = handlers;
        _renderServices = new HashSet<IRenderService>();
        _updateServices = new HashSet<IUpdateService>();
    }

    public int Width => _window is null ? 0 : _window.Size.X;
    public int Height => _window is null ? 0 : _window.Size.Y;

    public void AddRenderServices(IEnumerable<IRenderService> renderServices)
    {
        foreach (var service in renderServices)
        {
            _renderServices.Add(service);
        }
    }

    public void AddUpdateServices(IEnumerable<IUpdateService> updateServices)
    {
        foreach (var service in updateServices)
        {
            _updateServices.Add(service);
        }
    }

    public void Close()
    {
        _window?.Close();
    }

    public void Run()
    {
        var settings = new WindowSettings
        {
            Height = 1080,
            Width = 1920,
            OpenGLVersion = new Version(3, 3)
        };

        var gameWindowSettings = GameWindowSettings.Default;
        var nativeWindowSettings = new NativeWindowSettings
        {
            APIVersion = settings.OpenGLVersion,
            Size = new OpenTK.Mathematics.Vector2i(settings.Width, settings.Height),
        };

        _window = new GameWindow(gameWindowSettings, nativeWindowSettings);
        foreach (var service in _renderServices)
        {
            _window.Load += service.Load;
        }

        foreach (var service in _updateServices)
        {
            _window.UpdateFrame += (e) => service.Update(e.Time);
        }

        foreach (var handler in _handlers)
        {
            _window.Load += handler.OnLoad;
            _window.Unload += handler.OnUnload;
            _window.UpdateFrame += (s) => handler.OnUpdate(s.Time);
            _window.Resize += (s) => handler.OnResize(new Math.Point(s.Width, s.Height));
        }

        _window.RenderFrame += OnRenderFrame;

        _window?.Run();
    }

    private void OnRenderFrame(FrameEventArgs e)
    {
        foreach (var handler in _handlers)
        {
            handler.OnRender(e.Time);
        }
        foreach (var service in _renderServices)
        {
            service.Render(e.Time);
        }
        _window?.SwapBuffers();
    }
}
