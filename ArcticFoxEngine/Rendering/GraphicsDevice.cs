using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering;

public class GraphicsDevice
{
    private readonly GL _gl;

    public GraphicsDevice(GL gl)
    {
        _gl = gl;
    }

    public GL GL => _gl;
}
