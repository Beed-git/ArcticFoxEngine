using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Components;

public class TransformComponent : ComponentModel
{
    public Vector3 Position { get; set; }

    public override string ToString()
    {
        return $"({Position.x},{Position.y},{Position.z})";
    }
}
