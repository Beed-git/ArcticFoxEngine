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

    public Vector2i XY => new Vector2i(x, y);
    public Vector2i WH => new Vector2i(width, height);

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
}
