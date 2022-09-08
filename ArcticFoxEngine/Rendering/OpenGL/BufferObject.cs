using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace ArcticFoxEngine.Rendering.OpenGL;

public class BufferObject<T> : IDisposable where T : unmanaged
{
    private readonly GL _gl;

    private readonly uint _handle;
    private readonly BufferTargetARB _bufferTarget;
    private readonly BufferUsageARB _bufferUsage;

    private BufferObject(GL gl, BufferTargetARB bufferTarget, bool isDynamic)
    {
        _gl = gl;
        _bufferTarget = bufferTarget;
        _bufferUsage = isDynamic ? BufferUsageARB.StreamDraw : BufferUsageARB.StaticDraw;

        _handle = _gl.GenBuffer();
        _gl.CheckError();

        Bind();
    }

    public unsafe BufferObject(GL gl, Span<T> data, BufferTargetARB bufferTarget, bool isDynamic = false) : this(gl, bufferTarget, isDynamic)
    {
        fixed (void* d = data)
        {
            _gl.BufferData(_bufferTarget, (nuint)(data.Length * sizeof(T)), d, _bufferUsage);
        }
    }

    public unsafe BufferObject(GL gl, uint size, BufferTargetARB bufferTarget, bool isDynamic = false) : this(gl, bufferTarget, isDynamic)
    {
        _gl.BufferData(_bufferTarget, (nuint)(size * sizeof(T)), null, _bufferUsage);
    }

    public void Bind()
    {
        _gl.BindBuffer(_bufferTarget, _handle);
        _gl.CheckError();
    }

    public unsafe void BufferData(Span<T> data)
    {
        Bind();
        fixed (void* d = data)
        {
            _gl.BufferSubData(_bufferTarget, 0, (nuint)(data.Length * Marshal.SizeOf<T>()), d);
        }
        _gl.CheckError();
    }

    public void Dispose()
    {
        _gl.DeleteBuffer(_handle);
        _gl.CheckError();
    }
}
