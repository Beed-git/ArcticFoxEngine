namespace ArcticFoxEngine.Math;

public struct Vector2i
{
    public readonly int x;
    public readonly int y;

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2i Zero => new(0, 0);
    public static Vector2i One => new(1, 1);
    public static Vector2i UnitX => new(1, 0);
    public static Vector2i UnitY => new(0, 1);
    public static Vector2i Right => new(1, 0);
    public static Vector2i Up => new(0, 1);
    public static Vector2i Left => new(-1, 0);
    public static Vector2i Down => new(0, -1);

    public override bool Equals(object? obj)
    {
        return obj is Vector2i i &&
               x == i.x &&
               y == i.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }

    public int Dot(Vector2i vector)
    {
        return Dot(this, vector);
    }

    public static int Dot(Vector2i a, Vector2i b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public Vector2i Scale(Vector2i vector)
    {
        return Scale(this, vector);
    }

    public static Vector2i Scale(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.x * b.x, a.y * b.y);
    }

    public static Vector2i Add(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.x + b.x, a.y + b.y);
    }

    public static Vector2i Subtract(Vector2i a, Vector2i b)
    {
        return new Vector2i(a.x - b.x, a.y - b.y);
    }

    public static Vector2i Multiply(Vector2i vector, int scale)
    {
        return new Vector2i(vector.x * scale, vector.y * scale);
    }

    public static Vector2i Divide(Vector2i vector, int scale)
    {
        return new Vector2i(vector.x / scale, vector.y / scale);
    }

    public static Vector2i Remainder(Vector2i vector, int scale)
    {
        return new Vector2i(vector.x % scale, vector.y % scale);
    }

    //
    // Operators
    //

    public static bool operator ==(Vector2i a, Vector2i b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector2i a, Vector2i b)
    {
        return !(a == b);
    }

    public static Vector2i operator +(Vector2i a, Vector2i b)
    {
        return Add(a, b);
    }

    public static Vector2i operator -(Vector2i a, Vector2i b)
    {
        return Subtract(a, b);
    }

    public static Vector2i operator *(Vector2i vector, int scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector2i operator /(Vector2i vector, int scale)
    {
        return Divide(vector, scale);
    }

    public static Vector2i operator %(Vector2i vector, int scale)
    {
        return Remainder(vector, scale);
    }
}
