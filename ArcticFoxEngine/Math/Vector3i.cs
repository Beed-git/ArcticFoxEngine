namespace ArcticFoxEngine.Math;

public struct Vector3i
{
    public readonly int x;
    public readonly int y;
    public readonly int z;

    public Vector3i(Vector2i vector) : this(vector.x, vector.y, 0)
    {
    }

    public Vector3i(Vector2i vector, int z) : this(vector.x, vector.y, z)
    { 
    }

    public Vector3i(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3i Zero => new(0, 0, 0);
    public static Vector3i One => new(1, 1, 1);
    public static Vector3i UnitX => new(1, 0, 0);
    public static Vector3i UnitY => new(0, 1, 0);
    public static Vector3i UnitZ => new(0, 0, 1);
    public static Vector3i Right => new(1, 0, 0);
    public static Vector3i Up => new(0, 1, 0);
    public static Vector3i Forward => new(0, 0, 1);
    public static Vector3i Left => new(-1, 0, 0);
    public static Vector3i Down => new(0, -1, 0);
    public static Vector3i Back => new(0, 0, -1);

    public Vector2i XY => new(x, y);

    public override bool Equals(object? obj)
    {
        return obj is Vector3i i &&
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

    public int Dot(Vector3i vector)
    {
        return Dot(this, vector);
    }

    public static int Dot(Vector3i a, Vector3i b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public Vector3i Scale(Vector3i vector)
    {
        return Scale(this, vector);
    }

    public static Vector3i Scale(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public Vector3i Cross(Vector3i vector)
    {
        return Cross(this, vector);
    }

    public static Vector3i Cross(Vector3i a, Vector3i b)
    {
        var x = a.y * b.z - a.z * b.y;
        var y = a.z * b.x - a.x * b.z;
        var z = a.x * b.y - a.y * b.x;
        return new Vector3i(x, y, z);
    }

    public static Vector3i Add(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Vector3i Subtract(Vector3i a, Vector3i b)
    {
        return new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vector3i Multiply(Vector3i vector, int scale)
    {
        return new Vector3i(vector.x * scale, vector.y * scale, vector.z * scale);
    }

    public static Vector3i Divide(Vector3i vector, int scale)
    {
        return new Vector3i(vector.x / scale, vector.y / scale, vector.z / scale);
    }

    public static Vector3i Remainder(Vector3i vector, int scale)
    {
        return new Vector3i(vector.x % scale, vector.y % scale, vector.z % scale);
    }

    //
    // Operators
    //

    public static bool operator ==(Vector3i a, Vector3i b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector3i a, Vector3i b)
    {
        return !(a == b);
    }

    public static Vector3i operator +(Vector3i a, Vector3i b)
    {
        return Add(a, b);
    }

    public static Vector3i operator -(Vector3i a, Vector3i b)
    {
        return Subtract(a, b);
    }

    public static Vector3i operator *(Vector3i vector, int scale)
    {
        return Multiply(vector, scale);
    }

    public static Vector3i operator /(Vector3i vector, int scale)
    {
        return Divide(vector, scale);
    }

    public static Vector3i operator %(Vector3i vector, int scale)
    {
        return Remainder(vector, scale);
    }
}
