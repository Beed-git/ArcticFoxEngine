using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Camera;

public class Camera2D : ICamera
{
    private const float _nearPlane = 0.01f;
    private const float _farPlane = 100.0f;

    private Vector3 _front = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 _up = new Vector3(0.0f, 1.0f, 0.0f);

    private int _width;
    private int _height;

    public Camera2D()
    {
        _width = 512;
        _height = (int)(_width * 1080.0f / 1920.0f);
    }

    public Vector2 Position { get; set; }
    public Matrix4 ViewMatrix => Matrix4.LookAt(new Vector3(Position.X, Position.Y, 20.0f), new Vector3(Position) + _front, _up);
    public Matrix4 ProjectionMatrix => Matrix4.CreateOrthographic(_width, _height, _nearPlane, _farPlane);

    public void UpdateAspectRatio(int width, int height)
    {
        throw new NotImplementedException();
    }
}
