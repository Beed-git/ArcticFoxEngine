using ArcticFoxEngine.Math;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArcticFoxEngine.Services.Window;

public interface IWindowEventHandler
{
    public void OnLoad();
    public void OnUnload();
    public void OnUpdate(double time);
    public void OnRender(double time);
    public void OnResize(Point size);
    [Obsolete("This will probably change in the future as to not require OpenTK for input.")]
    public void OnKeyDown(Keys key);
    [Obsolete("This will probably change in the future as to not require OpenTK for input.")]
    public void OnKeyUp(Keys key);

}
