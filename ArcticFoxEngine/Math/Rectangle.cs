namespace ArcticFoxEngine.Math;

public struct Rectangle
{
    public int x;
    public int y;
    public int width;
    public int height;

    public Rectangle(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public Point XY => new(x, y);
    public Point WH => new(width, height);
    public static Rectangle Zero => new(0, 0, 0, 0);

    public override bool Equals(object? obj)
    {
        return obj is Rectangle rectangle &&
               x == rectangle.x &&
               y == rectangle.y &&
               width == rectangle.width &&
               height == rectangle.height;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, width, height);
    }

    public override string ToString()
    {
        return $"(x:{x},y:{y},w:{width},h:{height})";
    }

    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }
}
