using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Camera;

public class Camera2D : ICamera
{
    private const float _nearPlane = 0.01f;
    private const float _farPlane = 100.0f;

    private readonly Vector3 _front = Vector3.Back;
    private readonly Vector3 _up = Vector3.Up;

    private int _width;
    private int _height;

    public Camera2D()
    {
        _width = 512;
        _height = (int)(_width * 1080.0f / 1920.0f);
    }

    public Vector2 Position { get; set; }
    public Matrix4x4 ViewMatrix => Matrix4x4.LookAt(new Vector3(Position.x, Position.y, 20.0f), new Vector3(Position) + _front, _up);
    public Matrix4x4 ProjectionMatrix => Matrix4x4.CreateOrthographic(_width, _height, _nearPlane, _farPlane);

    public void UpdateAspectRatio(Vector2i size)
    {
        _width = size.x;
        _height = size.y;
    }
}
