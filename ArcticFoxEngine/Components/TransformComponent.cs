using ArcticFoxEngine.EC;

namespace ArcticFoxEngine.Components;

public class TransformComponent : Component
{
    public int X;
    public int Y;

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}
