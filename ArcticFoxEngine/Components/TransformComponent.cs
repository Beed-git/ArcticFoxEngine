using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Components;

public class TransformComponent : Component
{
    public Vector3 Position;

    public TransformComponent(Entity parent) : base(parent)
    {
    }

    public override string ToString()
    {
        return $"({Position.x},{Position.y},{Position.z})";
    }
}
