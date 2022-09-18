namespace ArcticFoxEngine.Components;

public class EntityModel
{
    public IEnumerable<ComponentModel> Components { get; set; }
    public IEnumerable<string> Scripts { get; set; }
}