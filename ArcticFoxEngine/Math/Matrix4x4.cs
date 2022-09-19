namespace ArcticFoxEngine.Math;

using YamlDotNet.Serialization;
using SM = Silk.NET.Maths;

/// <summary>
/// Represents a 4x4 matrix of floats. Matrices are row-major.
/// </summary>
public struct Matrix4x4
{
    public Vector4 row0;
    public Vector4 row1;
    public Vector4 row2;
    public Vector4 row3;

    public Matrix4x4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
    {
        this.row0 = row0;
        this.row1 = row1;
        this.row2 = row2;
        this.row3 = row3;
    }

    [YamlIgnore]
    public float M00 => row0.x;
    [YamlIgnore]
    public float M01 => row0.y;
    [YamlIgnore]
    public float M02 => row0.z;
    [YamlIgnore]
    public float M03 => row0.w;
    [YamlIgnore]
    public float M10 => row1.x;
    [YamlIgnore]
    public float M11 => row1.y;
    [YamlIgnore]
    public float M12 => row1.z;
    [YamlIgnore]
    public float M13 => row1.w;
    [YamlIgnore]
    public float M20 => row2.x;
    [YamlIgnore]
    public float M21 => row2.y;
    [YamlIgnore]
    public float M22 => row2.z;
    [YamlIgnore]
    public float M23 => row2.w;
    [YamlIgnore]
    public float M30 => row3.x;
    [YamlIgnore]
    public float M31 => row3.y;
    [YamlIgnore]
    public float M32 => row3.z;
    [YamlIgnore]
    public float M33 => row3.w;

    public static Matrix4x4 Identity => new(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

    public static Matrix4x4 LookAt(float eyeX, float eyeY, float eyeZ, float targetX, float targetY, float targetZ, float upX, float upY, float upZ)
    {
        var eye = new Vector3(eyeX, eyeY, eyeZ);
        var target = new Vector3(targetX, targetY, targetZ);
        var up = new Vector3(upX, upY, upZ);
        return LookAt(eye, target, up);
    }

    public static Matrix4x4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
        var z = Vector3.Normalize(eye - target);
        var x = Vector3.Normalize(Vector3.Cross(up, z));
        var y = Vector3.Cross(z, x);

        return new Matrix4x4(
            new Vector4(x.x, y.x, z.x, 0),
            new Vector4(x.y, y.y, z.y, 0),
            new Vector4(x.z, y.z, z.z, 0),
            new Vector4(-Vector3.Dot(eye, x), -Vector3.Dot(eye, y), -Vector3.Dot(eye, z), 1));
    }

    public static Matrix4x4 CreateOrthographic(Vector2 size, float depthNear, float depthFar)
    {
        return CreateOrthographic(size.x, size.y, depthNear, depthFar);
    }

    public static Matrix4x4 CreateOrthographic(float width, float height, float depthNear, float depthFar)
    {
        return CreateOrthographicOffCenter(-width / 2, width / 2, -height / 2, height / 2, depthNear, depthFar);
    }

    public static Matrix4x4 CreateOrthographicOffCenter(Vector2 topLeft, Vector2 bottomRight, float depthNear, float depthFar)
    {
        return CreateOrthographicOffCenter(topLeft.x, bottomRight.x, bottomRight.y, topLeft.y, depthNear, depthFar);
    }

    public static Matrix4x4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar)
    {
        // For now we'll borrow the function from silk.
        SM.Matrix4X4<float> ortho = SM.Matrix4X4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
        return FromSilkMatrix(ortho);
    }

    private static Matrix4x4 FromSilkMatrix(SM.Matrix4X4<float> mat)
    {
        return new Matrix4x4(
            FromSilkVector4(mat.Row1),
            FromSilkVector4(mat.Row2),
            FromSilkVector4(mat.Row3),
            FromSilkVector4(mat.Row4));
    }

    private static Vector4 FromSilkVector4(SM.Vector4D<float> vec)
    {
        return new Vector4(vec.X, vec.Y, vec.Z, vec.W);
    }

    public override string ToString()
    {
        return $"{row0}\n{row1}\n{row2}\n{row3}";
    }
}
