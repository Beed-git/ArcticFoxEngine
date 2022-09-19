using YamlDotNet.Serialization;

namespace ArcticFoxEngine.Math;

public struct Vector3
{
    public float x;
    public float y;
    public float z;

    public Vector3(Vector2 vector) : this(vector.x, vector.y, 0.0f)
    {
    }

    public Vector3(Vector2 vector, float z) : this(vector.x, vector.y, z)
    { 
    }

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3 Zero => new(0, 0, 0);
    public static Vector3 One => new(1, 1, 1);
    public static Vector3 UnitX => new(1, 0, 0);
    public static Vector3 UnitY => new(0, 1, 0);
    public static Vector3 UnitZ => new(0, 0, 1);
    public static Vector3 PositiveInfinity => new(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    public static Vector3 NegativeInfinity => new(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
    public static Vector3 Right => new(1, 0, 0);
    public static Vector3 Up => new(0, 1, 0);
    public static Vector3 Forward => new(0, 0, 1);
    public static Vector3 Left => new(-1, 0, 0);
    public static Vector3 Down => new(0, -1, 0);
    public static Vector3 Back => new(0, 0, -1);

    [YamlIgnore]
    public Vector2 XY => new(x, y);

    public override bool Equals(object? obj)
    {
        return obj is Vector3 i &&
               x == i.x &&
               y == i.y &&
               z == i.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    public override string ToString()
    {
        return $"({x},{y},{z})";
    }

    public float Magnitude()
    {
        return Magnitude(this);
    }

    public static float Magnitude(Vector3 vector)
    {
        return MathF.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    public Vector3 Normalize()
    {
        return Normalize(this);
    }

    public static Vector3 Normalize(Vector3 vector)
    {
        return Multiply(vector, 1.0f / Magnitude(vector));
    }

    public float Dot(Vector3 vector)
    {
        return Dot(this, vector);
    }

    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public Vector3 Scale(Vector3 vector)
    {
        return Scale(this, vector);
    }

    public static Vector3 Scale(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public Vector3 Cross(Vector3 vector)
    {
        return Cross(this, vector);
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        var x = a.y * b.z - a.z * b.y;
        var y = a.z * b.x - a.x * b.z;
        var z = a.x * b.y - a.y * b.x;
        return new Vector3(x, y, z);
    }

    public static Vector3 Add(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Vector3 Subtract(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vector3 Multiply(Vector3 vector, float scale)
    {
        return new Vector3(vector.x * scale, vector.y * scale, vector.z * scale);
    }

    public static Vector3 Divide(Vector3 vector, float scale)
    {
        return new Vector3(vector.x / scale, vector.y / scale, vector.z / scale);
    }

    public static Vector3 Remainder(Vector3 vector, float scale)
    {
        return new Vector3(vector.x % scale, vector.y % scale, vector.z % scale);
    }

    //
    // Operators
    //

    public static bool operator ==(Vector3 a, Vector3 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector3 a, Vector3 b)
    {
        return !(a == b);
    }

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return Add(a, b);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return Subtract(a, b);
    }

    public static Vector3 operator *(Vector3 vector, float scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector3 operator /(Vector3 vector, float scale)
    {
        return Divide(vector, scale);
    }

    public static Vector3 operator %(Vector3 vector, float scale)
    {
        return Remainder(vector, scale);
    }


    public static implicit operator System.Numerics.Vector3(Vector3 vector) => new(vector.x, vector.y, vector.z);
    public static implicit operator Vector3(System.Numerics.Vector3 vector) => new(vector.X, vector.Y, vector.Z);
}
