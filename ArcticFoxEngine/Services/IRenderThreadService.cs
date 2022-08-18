namespace ArcticFoxEngine.Services;

/// <summary>
/// I service which runs on the render thread.
/// Allows for the usage of GL calls.
/// </summary>
public interface IRenderThreadService
{
    /// <summary>
    /// Called during the window onload method.
    /// Runs after all injections are finished.
    /// </summary>
    public void Load()
    {
    }

    /// <summary>
    /// Update which is called at a consistant rate.
    /// </summary>
    /// <param name="dt">The time taken since the last fixed update.</param>
    public void FixedUpdate(double dt)
    {
    }

    /// <summary>
    /// Called during the window render method.
    /// </summary>
    /// <param name="dt">The time taken since the last render call.</param>
    public void Render(double dt);
}
