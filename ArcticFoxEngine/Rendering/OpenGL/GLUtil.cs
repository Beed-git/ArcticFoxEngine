using OpenTK.Graphics.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

public static class GLUtil
{
    public static void CheckError()
    {
        var error = GL.GetError();
        if (error is not ErrorCode.NoError)
        {
            throw new Exception($"OpenGL error: {error}");
        }
    }
}
