namespace ArcticFoxEngine.Services;

/// <summary>
/// I service which runs on the render thread.
/// Allows for the usage of GL calls.
/// </summary>
public interface IRenderService
{
    /// <summary>
    /// Called during the window render method.
    /// </summary>
    /// <param name="dt">The time taken since the last render call.</param>
    public void Render(double dt);
}
