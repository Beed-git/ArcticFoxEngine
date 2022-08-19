using ArcticFoxEngine.Math;
using ArcticFoxEngine.Services.Window;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Roguelike;

public class WindowEventHandler : IWindowEventHandler
{
    private readonly ILogger<WindowEventHandler> _logger;

    public WindowEventHandler(
        ILogger<WindowEventHandler> logger
    ) {
        _logger = logger;
    }

    public void OnLoad()
    {
    }

    public void OnUnload()
    { 
    }

    public void OnRender(double time)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.ClearColor(0.2f, 0.5f, 1.0f, 1.0f);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public void OnUpdate(double time)
    {
    }

    public void OnResize(Point size)
    {
    }

    public void OnKeyDown(Keys key)
    {
    }

    public void OnKeyUp(Keys key)
    {
    }
}
