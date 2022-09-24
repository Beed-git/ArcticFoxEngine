namespace ArcticFoxEngine.EC.Models;

internal class EntityModel
{
    public IEnumerable<IComponentModel> Components { get; set; }
    public IEnumerable<string> Scripts { get; set; }
}