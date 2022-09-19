using ArcticFoxEngine;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Textures;
using ArcticFoxEngine.Rendering;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.Window;
using ArcticFoxEngine.Logging;
using ArcticFoxEditor.EditorImGui;

namespace ArcticFoxEditor;

public class EditorWindow : IDisposable
{
    private readonly IWindow _window;
    private Core? _core;

    private GL _gl;
    private GraphicsDevice? _graphicsDevice;

    private EditorImGuiPanels _editorImGui;
    private ImGuiController _imGuiController;
    private IInputContext _inputContext;

    private RenderTarget _target;

    private System.Numerics.Vector2 _lastImguiImageSize;

    public EditorWindow(WindowSettings settings)
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
        _lastImguiImageSize = new Vector2(800, 600);

        _gl = _window.CreateOpenGL();
        _graphicsDevice = new GraphicsDevice(_gl);

        _target = new RenderTarget(_graphicsDevice, (uint)_lastImguiImageSize.X, (uint)_lastImguiImageSize.Y);

        _inputContext = _window.CreateInput();

        _imGuiController = new ImGuiController(_gl, _window, _inputContext, ConfigureImgui);

        _core = new Core(_graphicsDevice, new ConsoleLogger());
        _core.OnLoad();
        _core.OnResize(new Vector2i((int)_lastImguiImageSize.X, (int)_lastImguiImageSize.Y));

        _editorImGui = new EditorImGuiPanels(_core);
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

        _editorImGui.DrawPropertiesPanel();
        _editorImGui.DrawEntityPanel();

        ImGui.Begin("Game");
        ImGui.BeginChild("GLContext");

        // Draw the game to the render target.
        _target.Bind();
        _core?.OnRender(dt);
        _target.Unbind();

        // Draw render target on imgui image.
        var size = ImGui.GetWindowSize();
        if (size != _lastImguiImageSize)
        {
            var s = new Vector2i((int)size.X, (int)size.Y);
            _target.Resize((uint)s.x, (uint)s.y);
            _core?.OnResize(s);
        }

        _lastImguiImageSize = size;
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
