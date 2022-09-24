using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.EC.Models;

internal class SceneModel
{
    public string Name { get; set; }
    public string Camera { get; set; }
    public Color BackgroundColor { get; set; }
    public Dictionary<string, EntityModel> Entities { get; set; }
}
