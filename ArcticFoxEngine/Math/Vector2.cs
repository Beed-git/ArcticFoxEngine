namespace ArcticFoxEngine.Math;

public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2 Zero => new(0, 0);
    public static Vector2 One => new(1, 1);
    public static Vector2 UnitX => new(1, 0);
    public static Vector2 UnitY => new(0, 1);
    public static Vector2 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity);
    public static Vector2 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity);
    public static Vector2 Right => new(1, 0);
    public static Vector2 Up => new(0, 1);
    public static Vector2 Left => new(-1, 0);
    public static Vector2 Down => new(0, -1);

    public override bool Equals(object? obj)
    {
        return obj is Vector2 i &&
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

    public static float Magnitude(Vector2 vector)
    {
        return MathF.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }

    public Vector2 Normalize()
    {
        return Normalize(this);
    }

    public static Vector2 Normalize(Vector2 vector)
    {
        return Divide(vector, Magnitude(vector));
    }

    public float Dot(Vector2 vector)
    {
        return Dot(this, vector);
    }

    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public Vector2 Scale(Vector2 vector)
    {
        return Scale(this, vector);
    }

    public static Vector2 Scale(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }

    public static Vector2 Add(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 Subtract(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x - b.x, a.y - b.y);
    }

    public static Vector2 Multiply(Vector2 vector, float scale)
    {
        return new Vector2(vector.x * scale, vector.y * scale);
    }

    public static Vector2 Divide(Vector2 vector, float scale)
    {
        return new Vector2(vector.x / scale, vector.y / scale);
    }

    public static Vector2 Remainder(Vector2 vector, float scale)
    {
        return new Vector2(vector.x % scale, vector.y % scale);
    }

    //
    // Operators
    //

    public static bool operator ==(Vector2 a, Vector2 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector2 a, Vector2 b)
    {
        return !(a == b);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return Add(a, b);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return Subtract(a, b);
    }

    public static Vector2 operator *(Vector2 vector, float scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector2 operator /(Vector2 vector, float scale)
    {
        return Divide(vector, scale);
    }

    public static Vector2 operator %(Vector2 vector, float scale)
    {
        return Remainder(vector, scale);
    }

    public static implicit operator System.Numerics.Vector2(Vector2 vector) => new(vector.x, vector.y);
    public static implicit operator Vector2(System.Numerics.Vector2 vector) => new(vector.X, vector.Y);
}
