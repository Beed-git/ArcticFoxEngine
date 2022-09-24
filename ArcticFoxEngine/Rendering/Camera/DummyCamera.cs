using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Camera;

internal class DummyCamera : ICamera
{
    public Matrix4x4 ViewMatrix => Matrix4x4.Identity;

    public Matrix4x4 ProjectionMatrix => Matrix4x4.Identity;

    public void UpdateAspectRatio(Vector2i size)
    {
    }
}
