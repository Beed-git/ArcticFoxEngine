using System.Xml;

namespace ArcticFoxEngine.Math;

public struct Color
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public Color(uint color)
    {
        r = (byte)((color >> 24) & 0xFF);
        g = (byte)((color >> 16) & 0xFF);
        b = (byte)((color >> 8) & 0xFF);
        a = (byte)(color & 0xFF);
    }

    public Color(byte r, byte g, byte b) : this(r, g, b, 255)
    {
    }

    public Color(byte r, byte g, byte b, byte a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color(float r, float g, float b) : this(r, g, b, 1.0f)
    {
    }

    public Color(float r, float g, float b, float a) : this
        (
            (byte)(r * 255),
            (byte)(g * 255),
            (byte)(b * 255),
            (byte)(a * 255)
        )
    {
    }

    /// <summary>
    /// Converts a color to an int in the argb format.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static int ToInt(Color color)
    {
        int i = 0;
        i |= color.r << 24;
        i |= color.g << 16;
        i |= color.b << 8;
        i |= color.a;
        return i;
    }

    /// <summary>
    /// Converts an integer to a color.
    /// Integers are expected to be argb format.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color FromInt(uint color)
    {
        return new Color(color);
    }

    public override bool Equals(object? obj)
    {
        return obj is Color color &&
               r == color.r &&
               g == color.g &&
               b == color.b &&
               a == color.a;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(r, g, b, a);
    }
    public override string ToString()
    {
        return $"(r:{r},g:{g},b:{b},a:{a})";
    }

    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
    {
        return !(left == right);
    }

    public static implicit operator Color(System.Drawing.Color c) => new(c.R, c.G, c.B, c.A);
    public static implicit operator System.Drawing.Color(Color c) => System.Drawing.Color.FromArgb(c.a, c.r, c.g, c.b);

    public static Color White => new Color(255, 255, 255);
}
