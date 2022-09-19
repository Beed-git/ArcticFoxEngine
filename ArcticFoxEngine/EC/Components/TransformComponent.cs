using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.EC.Components;

public class TransformComponent : IComponentModel
{
    public Vector3 Position { get; set; }

    public TransformComponent()
    {
        Position = Vector3.Zero;
    }

    public override string ToString()
    {
        return $"({Position.x},{Position.y},{Position.z})";
    }
}
