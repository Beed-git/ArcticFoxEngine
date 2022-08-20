using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Camera;

public interface ICamera
{
    public Matrix4 ViewMatrix { get; }
    public Matrix4 ProjectionMatrix { get; }
    public void UpdateAspectRatio(int width, int height);
}
