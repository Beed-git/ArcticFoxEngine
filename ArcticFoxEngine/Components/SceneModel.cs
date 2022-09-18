namespace ArcticFoxEngine.Components;

public class SceneModel
{
    public string Name { get; set; }
    public string Camera { get; set; }
    public Dictionary<string, EntityModel> Entities { get; set; }
}
