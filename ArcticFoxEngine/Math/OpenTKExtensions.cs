using OpenTK.Mathematics;

namespace ArcticFoxEngine.Math;

public static class OpenTKExtensions
{
    public static Vector4 ToVector4(this Color color)
    {
        return new Vector4(color.r / 255.0f, color.g / 255.0f, color.b / 255.0f, color.a / 255.0f);
    }

    public static Color4 ToColor4(this Color color)
    {
        return new Color4(color.r, color.g, color.b, color.a);
    }
}
