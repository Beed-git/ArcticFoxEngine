using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Camera;

internal interface ICamera
{ 
    public IEntity? Parent { get; set; }

    public Matrix4x4 ViewMatrix { get; }
    public Matrix4x4 ProjectionMatrix { get; }

    public void UpdateAspectRatio(Vector2i size);
}
