using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;

namespace ArcticFoxEngine;

public class GameWindow : IDisposable
{
    private readonly IWindow _window;

    private Core? _core;
    private IEnumerable<Type>? _scriptTypes;

    private GL _gl;
    private GraphicsDevice? _graphicsDevice;

    private IInputContext _inputContext;

    public GameWindow(WindowSettings settings)
    {
        var options = WindowOptions.Default;
        options.Title = settings.Title;
        options.Size = new Vector2D<int>(settings.Size.x, settings.Size.y);

        _window = SilkWindow.Create(options);
        _scriptTypes = null;
    }

    public string Title
    {
        get => _window.Title;
        set => _window.Title = value;
    }

    public GameWindow WithScripts(IEnumerable<Type> types)
    {
        _scriptTypes = types;
        return this;
    }

    public void Run()
    {
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.FramebufferResize += OnResize;
        _window.Closing += OnClose;

        _window.Run();
    }

    private void OnLoad()
    {
        _gl = _window.CreateOpenGL();
        _graphicsDevice = new GraphicsDevice(_gl);

        _inputContext = _window.CreateInput();

        _core = new Core(this, _graphicsDevice);
        if (_scriptTypes is not null)
        { 
            _core.WithScripts(_scriptTypes);
        }
        _core.OnLoad();
        _core.OnResize(new Vector2i(_window.Size.X, _window.Size.Y));
    }

    private void OnUpdate(double dt)
    {
        _core?.OnUpdate(dt);
    }

    private void OnRender(double dt)
    {
        _core?.OnRender(dt);
    }

    private void OnResize(Vector2D<int> size)
    {
        _gl.Viewport(size);
        _core?.OnResize(new Vector2i(size.X, size.Y));
    }

    private void OnClose()
    {
        _core?.OnClose();

        _inputContext?.Dispose();

        _gl?.Dispose();
    }

    public void Dispose()
    {
        _window.Dispose();
    }
}
