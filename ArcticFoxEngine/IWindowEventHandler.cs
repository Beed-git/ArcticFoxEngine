using ArcticFoxEngine.Math;
using OpenTK.Windowing.Common;

namespace ArcticFoxEngine;

public interface IWindowEventHandler
{
    public void OnKeyDown(KeyboardKeyEventArgs key);
    public void OnKeyUp(KeyboardKeyEventArgs key);
    public void OnLoad();
    public void OnRender(double dt);
    public void OnResize(Point size);
    public void OnUnload();
    public void OnUpdate(double dt);
}