using ArcticFoxEngine.EC;
using ArcticFoxEngine.EC.Components;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Camera;

internal class Camera2D : ICamera
{
    private IEntity _parent;
    private TransformComponent _transform;

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
    public Matrix4x4 ViewMatrix
    {
        get
        {
            var scale = Matrix4x4.CreateScale(100, 100, 1);
            
            var eye = _parent is null 
                ? new Vector3(Position, 20.0f) 
                : new Vector3(_transform.Position.x, _transform.Position.y, 20.0f);

            var target = _parent is null
                ? new Vector3(Position) + _front
                : _transform.Position + _front;

            return Matrix4x4.LookAt(eye, target, _up) * scale;
        }
    }
    public Matrix4x4 ProjectionMatrix => Matrix4x4.CreateOrthographic(_width, _height, _nearPlane, _farPlane);

    public IEntity? Parent 
    {
        get => _parent;
        set
        {
            if (value is not null && value.TryGetComponent(out _transform))
            {
                _parent = value;
            }
            else
            {
                throw new Exception("Attempted to give camera a parent which doesn't have a transform component.");
            }
        }
    }

    public void UpdateAspectRatio(Vector2i size)
    {
        _width = size.x;
        _height = size.y;
    }
}
