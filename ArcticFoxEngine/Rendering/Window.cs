using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;
using ImGuiNET;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Textures;
using ArcticFoxEngine.Rendering.Sprites;

namespace ArcticFoxEngine.Rendering;

public class Window : IDisposable
{
    private readonly IWindow _window;
    private Core? _core;

    private GL _gl;
    private GraphicsDevice? _graphicsDevice;

    private ImGuiController _imGuiController;
    private IInputContext _inputContext;

    private RenderTarget _target;

    public Window(WindowSettings settings)
    {
        var options = WindowOptions.Default;
        options.Title = settings.Title;
        options.Size = new Vector2D<int>(settings.Size.x, settings.Size.y);

        _window = SilkWindow.Create(options);
    }

    public void Run()
    {
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.FramebufferResize += OnResize;
        _window.Closing += OnClose;

        _window.FileDrop += (s) =>
        {
            foreach (var st in s)
            {
                Console.WriteLine("Dragged and dropped a file: " + st);
            }
        };

        _window.Run();
    }

    private static void ConfigureImgui()
    {
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
    }

    private void OnLoad()
    {
        var gameSize = new Vector2i(800, 600);

        _gl = _window.CreateOpenGL();
        _graphicsDevice = new GraphicsDevice(_gl);

        _target = new RenderTarget(_graphicsDevice, (uint)gameSize.x, (uint)gameSize.y);

        _inputContext = _window.CreateInput();

        _imGuiController = new ImGuiController(_gl, _window, _inputContext, ConfigureImgui);

        _core = new Core(_graphicsDevice);
        _core.OnLoad();
        _core.OnResize(gameSize);
    }

    private void OnUpdate(double dt)
    {
        _core?.OnUpdate(dt);

        _imGuiController.Update((float)dt);
    }

    private void OnRender(double dt)
    {
        _gl.ClearColor(System.Drawing.Color.CornflowerBlue);
        _gl.Clear(ClearBufferMask.ColorBufferBit);

        ImGui.DockSpaceOverViewport();

        ImGui.Begin("Properties");

        ImGui.Text("[Entity]");
        ImGui.Text("Entity name");
        ImGui.NewLine();

        ImGui.Text("[Components]");
        // ImGuiExtensions.DragInt2("Transform", ref i);
        ImGui.End();

        ImGui.Begin("Game");
        ImGui.BeginChild("GLContext");

        ImGui.ShowDemoWindow();

        // Draw the game to the render target.
        _target.Bind();
        _core?.OnRender(dt);
        _target.Unbind();

        // Draw render target on imgui image.
        var size = ImGui.GetWindowSize();
        ImGui.Image((IntPtr)_target.TextureHandle, size, new(0, 1), new(1, 0));

        ImGui.EndChild();
        ImGui.End();

        _imGuiController.Render();
    }

    private void OnResize(Vector2D<int> size)
    {
        _gl.Viewport(size);
        //_core?.OnResize(new Vector2i(size.X, size.Y));
    }

    private void OnClose()
    {
        _core?.OnClose();

        _imGuiController?.Dispose();
        _inputContext?.Dispose();

        _target?.Dispose();
        _gl?.Dispose();
    }

    public void Dispose()
    {
        _window.Dispose();
    }
}
