using ArcticFoxEngine.EC;

namespace ArcticFoxEngine.Scripts;

public abstract class BaseScript
{
    public BaseScript(IEntity parent)
    {
        Entity = parent;
    }

    protected IEntity Entity { get; private init; }

    public virtual void OnCreate()
    {
    }

    public virtual void OnUpdate(double dt)
    {
    }
}
