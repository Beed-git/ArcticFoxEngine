using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Camera;

namespace ArcticFoxEngine.EC.Models;

internal class SceneModel
{
    public string Name { get; set; }
    public CameraModel Camera { get; set; }
    public Color BackgroundColor { get; set; }
    public Dictionary<string, EntityModel> Entities { get; set; }
}
