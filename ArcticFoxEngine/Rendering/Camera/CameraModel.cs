namespace ArcticFoxEngine.Rendering.Camera;

internal class CameraModel
{
    public string Type { get; set; }
    public string Parent { get; set; }

    public CameraModel()
    {
        Type = "Camera2D";
        Parent = string.Empty;
    }
}
