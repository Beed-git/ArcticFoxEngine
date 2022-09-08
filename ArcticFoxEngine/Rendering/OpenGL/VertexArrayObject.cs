using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

public class VertexArrayObject<TVertex, TIndex> : IDisposable
    where TVertex : unmanaged
    where TIndex : unmanaged
{
    private readonly GL _gl;
    private readonly uint _handle;

    private readonly uint _stride;

    public unsafe VertexArrayObject(GL gl)
    {
        _gl = gl;
        _stride = (uint)sizeof(TVertex);

        _handle = _gl.GenVertexArray();
        _gl.CheckError();

        Bind();
    }

    public void Bind()
    {
        _gl.BindVertexArray(_handle);
        _gl.CheckError();
    }

    public void Unbind()
    {
        _gl.BindVertexArray(0);
        _gl.CheckError();
    }

    public void Dispose()
    {
        _gl.DeleteVertexArray(_handle);
        _gl.CheckError();
    }

    public unsafe void VertexAttribPointer(uint position, int size, VertexAttribPointerType type, bool normalized, int offset)
    {
        _gl.VertexAttribPointer(position, size, type, normalized, _stride, (void*)offset);
        _gl.EnableVertexAttribArray(position);
        _gl.CheckError();
    }

    public unsafe void VertexAttribIPointer(uint position, int size, VertexAttribIType type, int offset)
    {
        _gl.VertexAttribIPointer(position, size, type, _stride, (void*)new IntPtr(offset));
        _gl.EnableVertexAttribArray(position);
        _gl.CheckError();
    }

    public unsafe void VertexAttribLPointer(uint position, int size, VertexAttribLType type, int offset)
    {
        _gl.VertexAttribLPointer(position, size, type, _stride, (void*)new IntPtr(offset));
        _gl.EnableVertexAttribArray(position);
        _gl.CheckError();
    }

    public override string ToString()
    {
        return _handle.ToString();
    }
}
