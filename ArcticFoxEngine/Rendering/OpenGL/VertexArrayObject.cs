using OpenTK.Graphics.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

public class VertexArrayObject : IDisposable
{
    private readonly int _handle;
    private readonly int _stride;

    public VertexArrayObject(int stride)
    {
        _stride = stride;
        _handle = GL.GenVertexArray();
        GLUtil.CheckError();
    }

    public void Use()
    {
        GL.BindVertexArray(_handle);
        GLUtil.CheckError();
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(_handle);
        GLUtil.CheckError();
    }

    public void VertexAttribPointer(int position, int size, VertexAttribPointerType type, bool normalized, int offset)
    {
        GL.EnableVertexAttribArray(position);
        GL.VertexAttribPointer(position, size, type, normalized, _stride, offset);
        GLUtil.CheckError();
    }

    public void VertexAttribIPointer(int position, int size, VertexAttribIntegerType type, int offset)
    {
        GL.EnableVertexAttribArray(position);
        GL.VertexAttribIPointer(position, size, type, _stride, new IntPtr(offset));
        GLUtil.CheckError();
    }

    public void VertexAttribLPointer(int position, int size, VertexAttribDoubleType type, int offset)
    {
        GL.EnableVertexAttribArray(position);
        GL.VertexAttribLPointer(position, size, type, _stride, new IntPtr(offset));
        GLUtil.CheckError();
    }

    public override string ToString()
    {
        return _handle.ToString();
    }
}
