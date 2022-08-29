using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Camera;

public class Camera2D : ICamera
{
    private const float _nearPlane = 0.01f;
    private const float _farPlane = 100.0f;

    private Vector3 _front = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 _up = new Vector3(0.0f, 1.0f, 0.0f);

    private readonly int _size;
    private int _width;
    private int _height;

    public Camera2D()
    {
        _size = 512;
        UpdateAspectRatio(_size, _size);
    }

    public Vector2 Position { get; set; }
    public Matrix4x4 ViewMatrix => Matrix4x4.LookAt(new Vector3(Position, 20.0f), new Vector3(Position) + _front, _up);
    public Matrix4x4 ProjectionMatrix => Matrix4x4.CreateOrthographic(_width, _height, _nearPlane, _farPlane);

    public void UpdateAspectRatio(int width, int height)
    {
        float aspectRatio = width / (float)height;
        _width = (int)(_size * aspectRatio);
        _height = _size;
    }
}
