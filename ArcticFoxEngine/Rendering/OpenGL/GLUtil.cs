using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

internal static class GLUtil
{
    public static void CheckError(this GL gl)
    {
        var error = (ErrorCode)gl.GetError();
        if (error is not ErrorCode.NoError)
        {
            throw new Exception($"OpenGL error: {error}");
        }
    }
}
