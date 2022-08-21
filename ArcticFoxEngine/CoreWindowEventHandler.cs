using ArcticFoxEngine.Math;
using ArcticFoxEngine.Services;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ArcticFoxEngine;

internal class CoreWindowEventHandler : IWindowEventHandler
{
    private readonly GameWindow _window;
    private readonly IEnumerable<IInitService> _initServices;
    private readonly IEnumerable<IUpdateService> _updateServices;
    private readonly IEnumerable<IRenderService> _renderServices;

    public CoreWindowEventHandler(
        GameWindow window,
        IEnumerable<IInitService> initServices,
        IEnumerable<IUpdateService> updateServices,
        IEnumerable<IRenderService> renderServices
    ) {
        _window = window;
        _initServices = initServices;
        _updateServices = updateServices;
        _renderServices = renderServices;
    }

    public void OnLoad()
    {
        foreach (var service in _initServices)
        {
            service.Init();
        }
    }

    public void OnUpdate(double dt)
    {
        foreach (var service in _updateServices)
        {
            service.Update(dt);
        }
    }

    public void OnRender(double dt)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.ClearColor(0.2f, 0.5f, 1.0f, 1.0f);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        foreach (var service in _renderServices)
        {
            service.Render(dt);
        }

        _window.SwapBuffers();
    }

    public void OnUnload()
    {
    }

    public void OnResize(Point size)
    {
        GL.Viewport(0, 0, size.x, size.y);
    }

    public void OnKeyDown(KeyboardKeyEventArgs key)
    {
    }

    public void OnKeyUp(KeyboardKeyEventArgs key)
    {
    }
}
