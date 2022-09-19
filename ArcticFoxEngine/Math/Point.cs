namespace ArcticFoxEngine.Math;

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Point Zero => new(0, 0);
    public static Point One => new(1, 1);

    public override bool Equals(object? obj)
    {
        return obj is Point point &&
               x == point.x &&
               y == point.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override string ToString()
    {
        return $"(x:{x},y:{y})";
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }
}
