namespace ArcticFoxEngine.Math;

public struct Vector2i
{
    public int x;
    public int y;

    public Vector2i(int i) : this(i, i)
    {
    }

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

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

    public float Magnitude()
    {
        return Magnitude(this);
    }

    public static float Magnitude(Vector2i vector)
    {
        return MathF.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }

    // v2i, v2i

    public static Vector2i Add(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.x + right.x, left.y + right.y);
    }
    
    public static Vector2i Subtract(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.x - right.x, left.y + right.y);
    }

    // v2i, int

    public static Vector2i Add(Vector2i vector, int scale)
    {
        return new Vector2i(vector.x + scale, vector.y + scale);
    }
    
    public static Vector2i Subtract(Vector2i vector, int scale)
    {
        return new Vector2i(vector.x - scale, vector.y - scale);
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

    // v2i, v2i

    public static bool operator ==(Vector2i left, Vector2i right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2i left, Vector2i right)
    {
        return !(left == right);
    }

    public static Vector2i operator +(Vector2i left, Vector2i right)
    {
        return Add(left, right);
    }

    public static Vector2i operator -(Vector2i left, Vector2i right)
    {
        return Subtract(left, right);
    }

    // v2i, int

    public static Vector2i operator +(Vector2i vector, int scale)
    {
        return Add(vector, scale);
    }

    public static Vector2i operator -(Vector2i vector, int scale)
    {
        return Subtract(vector, scale);
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
