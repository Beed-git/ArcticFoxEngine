using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Camera;

public interface ICamera
{
    public Matrix4x4 ViewMatrix { get; }
    public Matrix4x4 ProjectionMatrix { get; }

    public void UpdateAspectRatio(Vector2i size);
}
