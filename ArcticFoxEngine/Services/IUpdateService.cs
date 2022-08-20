namespace ArcticFoxEngine.Services;

public interface IUpdateService
{
    /// <summary>
    /// Updates a fixed timestep, by default 60 times per second.
    /// </summary>
    /// <param name="dt">The time between this frame and the previous.</param>
    public void Update(double dt);
}
