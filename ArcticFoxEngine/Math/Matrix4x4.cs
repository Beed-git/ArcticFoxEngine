namespace ArcticFoxEngine.Math;

/// <summary>
/// Represents a 4x4 matrix of floats. Matrices are row-major.
/// </summary>
public struct Matrix4x4
{
    public readonly Vector4 row0;
    public readonly Vector4 row1;
    public readonly Vector4 row2;
    public readonly Vector4 row3;

    public Matrix4x4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
    {
        this.row0 = row0;
        this.row1 = row1;
        this.row2 = row2;
        this.row3 = row3;
    }

    public float M00 => row0.x;
    public float M01 => row0.y;
    public float M02 => row0.z;
    public float M03 => row0.w;
    public float M10 => row1.x;
    public float M11 => row1.y;
    public float M12 => row1.z;
    public float M13 => row1.w;
    public float M20 => row2.x;
    public float M21 => row2.y;
    public float M22 => row2.z;
    public float M23 => row2.w;
    public float M30 => row3.x;
    public float M31 => row3.y;
    public float M32 => row3.z;
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
        var inverseLR = 1.0f / (right - left);
        var inverseTB = 1.0f / (top - bottom);
        var inverseNF = 1.0f / (depthFar - depthNear);

        var row0 = new Vector4(2 * inverseLR, 0, 0, 0);
        var row1 = new Vector4(0, 2 * inverseTB, 0, 0);
        var row2 = new Vector4(0, 0, -2 * inverseNF, 0);

        var row3x = -(right + left) * inverseLR;
        var row3y = -(bottom + top) * inverseTB;
        var row3z = -(left + right) * inverseNF;
        var row3 = new Vector4(row3x, row3y, row3z, 1);

        return new Matrix4x4(row0, row1, row2, row3);
    }

    public static Matrix4x4 CreatePerspective()
    {
        throw new NotImplementedException("CreatePerspective not yet implemented.");
    }
}
