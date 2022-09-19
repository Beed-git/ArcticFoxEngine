using YamlDotNet.Serialization;

namespace ArcticFoxEngine.Math;

public struct Vector4i
{
    public int x;
    public int y;
    public int z;
    public int w;

    public Vector4i(Vector2i vector) : this(vector.x, vector.y, 0, 0)
    {
    }

    public Vector4i(Vector2i vector, int z, int w) : this(vector.x, vector.y, z, w)
    {
    }

    public Vector4i(Vector2i a, Vector2i b) : this(a.x, a.y, b.x, b.y)
    {
    }

    public Vector4i(Vector3i vector) : this(vector.x, vector.y, vector.z, 0)
    {
    }

    public Vector4i(Vector3i vector, int w) : this (vector.x, vector.y, vector.z, w)
    {
    }

    public Vector4i(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Vector4i Zero => new(0, 0, 0, 0);
    public static Vector4i One => new(1, 1, 1, 1);
    public static Vector4i UnitX => new(1, 0, 0, 0);
    public static Vector4i UnitY => new(0, 1, 0, 0);
    public static Vector4i UnitZ => new(0, 0, 1, 0);
    public static Vector4i UnitW => new(0, 0, 0, 1);

    [YamlIgnore]
    public Vector3i XYZ => new(x, y, z);
    [YamlIgnore]
    public Vector2i XY => new(x, y);
    [YamlIgnore]
    public Vector2i ZW => new(z, w);

    public override bool Equals(object? obj)
    {
        return obj is Vector4i i &&
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

    public int Dot(Vector4i vector)
    {
        return Dot(this, vector);
    }

    public static int Dot(Vector4i a, Vector4i b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public Vector4i Scale(Vector4i vector)
    {
        return Scale(this, vector);
    }

    public static Vector4i Scale(Vector4i a, Vector4i b)
    {
        return new Vector4i(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    }

    public static Vector4i Add(Vector4i a, Vector4i b)
    {
        return new Vector4i(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    public static Vector4i Subtract(Vector4i a, Vector4i b)
    {
        return new Vector4i(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    }

    public static Vector4i Multiply(Vector4i vector, int scale)
    {
        return new Vector4i(vector.x * scale, vector.y * scale, vector.z * scale, vector.w * scale);
    }

    public static Vector4i Divide(Vector4i vector, int scale)
    {
        return new Vector4i(vector.x / scale, vector.y / scale, vector.z / scale, vector.w / scale);
    }

    public static Vector4i Remainder(Vector4i vector, int scale)
    {
        return new Vector4i(vector.x % scale, vector.y % scale, vector.z % scale, vector.w % scale);
    }

    //
    // Operators
    //

    public static bool operator ==(Vector4i a, Vector4i b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector4i a, Vector4i b)
    {
        return !(a == b);
    }

    public static Vector4i operator +(Vector4i a, Vector4i b)
    {
        return Add(a, b);
    }

    public static Vector4i operator -(Vector4i a, Vector4i b)
    {
        return Subtract(a, b);
    }

    public static Vector4i operator *(Vector4i vector, int scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector4i operator /(Vector4i vector, int scale)
    {
        return Divide(vector, scale);
    }

    public static Vector4i operator %(Vector4i vector, int scale)
    {
        return Remainder(vector, scale);
    }
}
