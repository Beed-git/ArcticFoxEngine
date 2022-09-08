using ArcticFoxEngine.Math;
using M = Silk.NET.Maths;

namespace Tests.Math;

[TestClass]
public class Matrix4x4Tests
{
    [TestMethod]
    public void CompareLookat()
    {
        Vector3 _front = Vector3.Back;
        Vector3 _up = Vector3.Up;

        var position = new Vector2(15, 4);

        var afResult = Matrix4x4.LookAt(new Vector3(position.x, position.y, 20.0f), new Vector3(position) + _front, _up);

        M.Vector3D<float> _sFront = new(0.0f, 0.0f, -1.0f);
        M.Vector3D<float> _sUp = new(0.0f, 1.0f, 0.0f);

        M.Vector2D<float> sPosition = new(15, 4);

        var sResult = M.Matrix4X4.CreateLookAt(new M.Vector3D<float>(sPosition, 20.0f), new M.Vector3D<float>(sPosition, 0.0f) + _sFront, _sUp);

        Assert.IsFalse(MatricesAreNotEqual(afResult, sResult));
    }

    [TestMethod]
    public void CompareCreateOrthographic()
    {
        float width = 1920.0f;
        float height = 1080.0f;
        float near = 0.01f;
        float far = 100.0f;

        var acOrtho = Matrix4x4.CreateOrthographic(width, height, near, far);
        var sOrtho = M.Matrix4X4.CreateOrthographic(width, height, near, far);

        Console.WriteLine(acOrtho.row0);
        Console.WriteLine(acOrtho.row1);
        Console.WriteLine(acOrtho.row2);
        Console.WriteLine(acOrtho.row3);
        Console.WriteLine();
        Console.WriteLine(sOrtho.Row1);
        Console.WriteLine(sOrtho.Row2);
        Console.WriteLine(sOrtho.Row3);
        Console.WriteLine(sOrtho.Row4);

        Assert.IsFalse(MatricesAreNotEqual(acOrtho, sOrtho));
    }

    [TestMethod]
    public void CompareCreateOrthographicOffCenter()
    {
        float left = 0.0f;
        float top = 0.0f;
        float right = 1920.0f;
        float bottom = 1080.0f;
        float near = 0.01f;
        float far = 100.0f;

        var acOffCenter = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);
        var sOffCenter = M.Matrix4X4.CreateOrthographicOffCenter(left, right, bottom, top, near, far);

        Console.WriteLine(acOffCenter.row0);
        Console.WriteLine(acOffCenter.row1);
        Console.WriteLine(acOffCenter.row2);
        Console.WriteLine(acOffCenter.row3);
        Console.WriteLine();
        Console.WriteLine(sOffCenter.Row1);
        Console.WriteLine(sOffCenter.Row2);
        Console.WriteLine(sOffCenter.Row3);
        Console.WriteLine(sOffCenter.Row4);

        Assert.IsFalse(MatricesAreNotEqual(acOffCenter, sOffCenter));
    }

    private static bool MatricesAreNotEqual(Matrix4x4 m, M.Matrix4X4<float> s)
    {
        bool different = false;
        different |= RowsAreNotEqual(m.row0, s.Row1);
        different |= RowsAreNotEqual(m.row1, s.Row2);
        different |= RowsAreNotEqual(m.row2, s.Row3);
        different |= RowsAreNotEqual(m.row3, s.Row4);
        return different;
    }

    private static bool RowsAreNotEqual(Vector4 m, M.Vector4D<float> s)
    {
        bool different = false;
        different |= m.x != s.X;
        different |= m.y != s.Y;
        different |= m.z != s.Z;
        different |= m.w != s.W;
        return different;
    }
}
