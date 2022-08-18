using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace ArcticFoxEngine.Rendering.OpenGL;

internal class BufferObject<T> : IDisposable where T : unmanaged
{
    private readonly int _handle;
    private readonly int _size;
    private readonly int _elementByteSize;
    private readonly BufferTarget _bufferTarget;

    public BufferObject(int size, BufferTarget bufferTarget, bool isDynamic = false)
    {
        _bufferTarget = bufferTarget;
        _size = size;

        _handle = GL.GenBuffer();
        GLUtil.CheckError();

        Bind();

        _elementByteSize = Marshal.SizeOf<T>();
        GL.BufferData(_bufferTarget, _size * _elementByteSize, IntPtr.Zero, isDynamic ? BufferUsageHint.StreamDraw : BufferUsageHint.StaticDraw);
        GLUtil.CheckError();
    }

    public void Bind()
    {
        GL.BindBuffer(_bufferTarget, _handle);
        GLUtil.CheckError();
    }

    public void BufferData(T[] data)
    {
        Bind();
        GL.BufferSubData(_bufferTarget, IntPtr.Zero, _size * _elementByteSize, data);
        GLUtil.CheckError();
    }

    public void BufferData(T[] data, int length)
    {
        Bind();
        GL.BufferSubData(_bufferTarget, IntPtr.Zero, length * _elementByteSize, data);
    }

    public void Dispose()
    {
        GL.DeleteBuffer(_handle);
        GLUtil.CheckError();
    }
}
