using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.Textures;

public class RenderTarget : IDisposable
{
    private readonly GL _gl;

    private readonly uint _width;
    private readonly uint _height;

    private uint _fbo;
    private uint _texture;
    private uint _depthBuffer;

    private int[] _previousViewportSize;

    public RenderTarget(GraphicsDevice graphicsDevice, uint width, uint height)
    {
        _gl = graphicsDevice.GL;

        _width = width;
        _height = height;

        _previousViewportSize = new int[4];

        Resize(width, height);
    }

    public uint TextureHandle => _texture;

    public void Resize(uint width, uint height)
    {
        Dispose();

        // Create fbo.
        _fbo = _gl.GenFramebuffer();
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

        // Create texture.
        _texture = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, _width, _height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, ReadOnlySpan<byte>.Empty);
        _gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Nearest);
        _gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Nearest);

        // Create depth buffer.
        _depthBuffer = _gl.GenRenderbuffer();
        _gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _depthBuffer);
        _gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, GLEnum.DepthComponent, _width, _height);
        _gl.FramebufferRenderbuffer(GLEnum.Framebuffer, GLEnum.DepthAttachment, GLEnum.Renderbuffer, _depthBuffer);

        // Configure fbo.
        _gl.FramebufferTexture(GLEnum.Framebuffer, GLEnum.ColorAttachment0, _texture, 0);
        _gl.DrawBuffer(GLEnum.ColorAttachment0);

        if (_gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != GLEnum.FramebufferComplete)
        {
            var status = _gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            throw new Exception($"Something went wrong when creating the framebuffer!\n{status}");
        }
    }

    public void Bind()
    {
        _gl.GetInteger(GetPName.Viewport, _previousViewportSize);

        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        _gl.Viewport(0, 0, _width, _height);
    }

    public void Unbind()
    {
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        _gl.Viewport(_previousViewportSize[0], _previousViewportSize[1], (uint)_previousViewportSize[2], (uint)_previousViewportSize[3]);
    }

    public void Dispose()
    {
        _gl.DeleteRenderbuffer(_depthBuffer);
        _gl.DeleteTexture(_texture);
        _gl.DeleteFramebuffer(_fbo);
    }
}
