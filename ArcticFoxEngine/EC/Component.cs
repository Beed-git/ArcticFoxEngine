namespace ArcticFoxEngine.EC;

public abstract class Component
{
    private readonly Entity _parent;

    public Component(Entity parent)
    {
        _parent = parent;
    }
}
