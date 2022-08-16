namespace ArcticFoxEngine.Math;

public struct Vector2f
{
    public float x;
    public float y;

    public Vector2f(float i) : this(i, i)
    {
    }

    public Vector2f(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2f i &&
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

    public static float Magnitude(Vector2f vector)
    {
        return MathF.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }

    // v2i, v2i

    public static Vector2f Add(Vector2f left, Vector2f right)
    {
        return new Vector2f(left.x + right.x, left.y + right.y);
    }

    public static Vector2f Subtract(Vector2f left, Vector2f right)
    {
        return new Vector2f(left.x - right.x, left.y + right.y);
    }

    // v2i, int

    public static Vector2f Add(Vector2f vector, float scale)
    {
        return new Vector2f(vector.x + scale, vector.y + scale);
    }

    public static Vector2f Subtract(Vector2f vector, float scale)
    {
        return new Vector2f(vector.x - scale, vector.y - scale);
    }

    public static Vector2f Multiply(Vector2f vector, float scale)
    {
        return new Vector2f(vector.x * scale, vector.y * scale);
    }

    public static Vector2f Divide(Vector2f vector, float scale)
    {
        return new Vector2f(vector.x / scale, vector.y / scale);
    }

    public static Vector2f Remainder(Vector2f vector, float scale)
    {
        return new Vector2f(vector.x % scale, vector.y % scale);
    }

    //
    // Operators
    //

    // v2i, v2i

    public static bool operator ==(Vector2f left, Vector2f right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2f left, Vector2f right)
    {
        return !(left == right);
    }

    public static Vector2f operator +(Vector2f left, Vector2f right)
    {
        return Add(left, right);
    }

    public static Vector2f operator -(Vector2f left, Vector2f right)
    {
        return Subtract(left, right);
    }

    // v2i, int

    public static Vector2f operator +(Vector2f vector, float scale)
    {
        return Add(vector, scale);
    }

    public static Vector2f operator -(Vector2f vector, float scale)
    {
        return Subtract(vector, scale);
    }

    public static Vector2f operator *(Vector2f vector, float scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector2f operator /(Vector2f vector, float scale)
    {
        return Divide(vector, scale);
    }

    public static Vector2f operator %(Vector2f vector, float scale)
    {
        return Remainder(vector, scale);
    }
}
