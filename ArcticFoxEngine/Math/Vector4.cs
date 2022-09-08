namespace ArcticFoxEngine.Math;

public struct Vector4
{
    public readonly float x;
    public readonly float y;
    public readonly float z;
    public readonly float w;

    public Vector4(Vector2 vector) : this(vector.x, vector.y, 0.0f, 0.0f)
    {
    }

    public Vector4(Vector2 vector, float z, float w) : this(vector.x, vector.y, z, w)
    {
    }

    public Vector4(Vector2 a, Vector2 b) : this(a.x, a.y, b.x, b.y)
    {
    }

    public Vector4(Vector3 vector) : this(vector.x, vector.y, vector.z, 0.0f)
    {
    }

    public Vector4(Vector3 vector, float w) : this (vector.x, vector.y, vector.z, w)
    {
    }

    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Vector4 Zero => new(0, 0, 0, 0);
    public static Vector4 One => new(1, 1, 1, 1);
    public static Vector4 UnitX => new(1, 0, 0, 0);
    public static Vector4 UnitY => new(0, 1, 0, 0);
    public static Vector4 UnitZ => new(0, 0, 1, 0);
    public static Vector4 UnitW => new(0, 0, 0, 1);
    public static Vector4 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    public static Vector4 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

    public Vector3 XYZ => new(x, y, z);
    public Vector2 XY => new(x, y);
    public Vector2 ZW => new(z, w);

    public override bool Equals(object? obj)
    {
        return obj is Vector4 i &&
               x == i.x &&
               y == i.y &&
               z == i.z &&
               w == i.w;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }

    public override string ToString()
    {
        return $"({x},{y},{z},{w})";
    }

    public float Magnitude()
    {
        return Magnitude(this);
    }

    public static float Magnitude(Vector4 vector)
    {
        return MathF.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z + vector.w * vector.w);
    }

    public Vector4 Normalize()
    {
        return Normalize(this);
    }

    public static Vector4 Normalize(Vector4 vector)
    {
        return Divide(vector, Magnitude(vector));
    }

    public float Dot(Vector4 vector)
    {
        return Dot(this, vector);
    }

    public static float Dot(Vector4 a, Vector4 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public Vector4 Scale(Vector4 vector)
    {
        return Scale(this, vector);
    }

    public static Vector4 Scale(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    }

    public static Vector4 Add(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    public static Vector4 Subtract(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    }

    public static Vector4 Multiply(Vector4 vector, float scale)
    {
        return new Vector4(vector.x * scale, vector.y * scale, vector.z * scale, vector.w * scale);
    }

    public static Vector4 Divide(Vector4 vector, float scale)
    {
        return new Vector4(vector.x / scale, vector.y / scale, vector.z / scale, vector.w / scale);
    }

    public static Vector4 Remainder(Vector4 vector, float scale)
    {
        return new Vector4(vector.x % scale, vector.y % scale, vector.z % scale, vector.w % scale);
    }

    //
    // Operators
    //

    public static bool operator ==(Vector4 a, Vector4 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector4 a, Vector4 b)
    {
        return !(a == b);
    }

    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
        return Add(a, b);
    }

    public static Vector4 operator -(Vector4 a, Vector4 b)
    {
        return Subtract(a, b);
    }

    public static Vector4 operator *(Vector4 vector, float scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector4 operator /(Vector4 vector, float scale)
    {
        return Divide(vector, scale);
    }

    public static Vector4 operator %(Vector4 vector, float scale)
    {
        return Remainder(vector, scale);
    }

    public static implicit operator System.Numerics.Vector4(Vector4 vector) => new(vector.x, vector.y, vector.z, vector.w);
    public static implicit operator Vector4(System.Numerics.Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
}
