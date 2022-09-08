using ArcticFoxEngine.Math;
using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.Textures;

public class Texture2D : ITexture
{
    private readonly GL _gl;

    private readonly uint _handle;
    private const TextureTarget _textureTarget = TextureTarget.Texture2D;

    public Texture2D(GraphicsDevice graphicsDevice, uint width, uint height)
    {
        _gl = graphicsDevice.GL;

        Width = width;
        Height = height;

        _handle = _gl.GenTexture();

        Bind();

        _gl.TexImage2D(_textureTarget, 0, InternalFormat.Rgba8, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ReadOnlySpan<byte>.Empty);
        _gl.TexParameter(_textureTarget, GLEnum.TextureMinFilter, (int)GLEnum.Nearest);
        _gl.TexParameter(_textureTarget, GLEnum.TextureMagFilter, (int)GLEnum.Nearest);
        _gl.TexParameter(_textureTarget, GLEnum.TextureWrapS, (int)GLEnum.Repeat);
        _gl.TexParameter(_textureTarget, GLEnum.TextureWrapT, (int)GLEnum.Repeat);
        _gl.GenerateMipmap(_textureTarget);
    }

    public uint Width { get; private init; }
    public uint Height { get; private init; }
    public Rectangle Bounds => new(0, 0, (int)Width, (int)Height);

    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(unit);
        _gl.BindTexture(_textureTarget, _handle);
    }

    public unsafe void SetData(Rectangle bounds, Span<byte> data)
    {
        Bind();
        fixed (void* d = data)
        {
            _gl.TexSubImage2D(_textureTarget, 0, bounds.x, bounds.y, (uint)bounds.width, (uint)bounds.height, PixelFormat.Rgba, PixelType.UnsignedByte, d);
        }
    }

    public void Dispose()
    {
        _gl.DeleteTexture(_handle);
    }
}
