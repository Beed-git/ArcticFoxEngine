using ArcticFoxEngine.Math;
using OpenTK.Graphics.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

public class Texture2D : ITexture2D
{
    private static readonly TextureTarget _target = TextureTarget.Texture2D;
    
    private readonly int _handle;

    private readonly PixelInternalFormat _pixelInternalFormat;
    private readonly PixelFormat _pixelFormat;
    private readonly PixelType _pixelType;

    public Texture2D(int width, int height)
    {
        Width = width;
        Height = height;

        _pixelInternalFormat = PixelInternalFormat.Rgba8;
        _pixelFormat = PixelFormat.Rgba;
        _pixelType = PixelType.UnsignedByte;

        _handle = GL.GenTexture();
        GLUtil.CheckError();

        Use();
        GL.TexImage2D(_target, 0, _pixelInternalFormat, Width, Height, 0, _pixelFormat, _pixelType, IntPtr.Zero);
        GLUtil.CheckError();

        CreateParameters();
    }

    private static void CreateParameters()
    {
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        GLUtil.CheckError();
    }

    public int Width { get; private init; }
    public int Height { get; private init; }
    public Rectangle Bounds => new(0, 0, Width, Height);

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GLUtil.CheckError();

        GL.BindTexture(_target, _handle);
        GLUtil.CheckError();
    }

    public void SetData(Rectangle bounds, byte[] data)
    {
        GL.TexSubImage2D(
            _target,
            0,
            bounds.x,
            bounds.y,
            bounds.x + bounds.width,
            bounds.y + bounds.height,
            _pixelFormat,
            _pixelType,
            data);
        GLUtil.CheckError();
    }

    public void Dispose()
    {
        GL.DeleteTexture(_handle);
        GLUtil.CheckError();
    }
}
