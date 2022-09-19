namespace ArcticFoxEngine.EC.Models;

public class EntityModel
{
    public IEnumerable<IComponentModel> Components { get; set; }
    public IEnumerable<string> Scripts { get; set; }
}