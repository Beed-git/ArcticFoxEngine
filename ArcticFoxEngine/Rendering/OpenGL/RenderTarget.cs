using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.OpenGL;

public class RenderTarget : IDisposable
{
    private readonly GL _gl;

    private readonly uint _width;
    private readonly uint _height;

    private uint _fbo;
    private uint _texture;
    private uint _depthBuffer;

    private int[] _previousViewport;

    public RenderTarget(GL context, uint width, uint height)
    {
        _gl = context;

        _width = width;
        _height = height;

        _previousViewport = new int[4];

        CreateBuffers();
    }

    public uint FBO => _fbo;

    private void CreateBuffers()
    {
        // http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-14-render-to-texture/#creating-the-render-target

        // Create fbo.
        _fbo = _gl.GenFramebuffer();
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

        // Create texture.
        _texture = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, _texture);
        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, _width, _height, 0, GLEnum.Rgb, GLEnum.UnsignedByte, ReadOnlySpan<byte>.Empty);
        _gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Nearest);
        _gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Nearest);

        // Create depth buffer.
        _depthBuffer = _gl.GenRenderbuffer();
        _gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _depthBuffer);
        _gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, GLEnum.DepthComponent, _width, _height);
        _gl.FramebufferRenderbuffer(GLEnum.Framebuffer, GLEnum.DepthAttachment, GLEnum.Renderbuffer, _depthBuffer);
        _gl.FramebufferTexture(GLEnum.Framebuffer, GLEnum.ColorAttachment0, _texture, 0);
        _gl.DrawBuffer(GLEnum.ColorAttachment0);
    }

    public void Bind()
    {
        _gl.GetInteger(GetPName.Viewport, _previousViewport);

        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        _gl.Viewport(0, 0, _width, _height);
    }

    public void Unbind()
    {
        _gl.BindFramebuffer(GLEnum.Framebuffer, 0);
        _gl.Viewport(_previousViewport[0], _previousViewport[1], (uint)_previousViewport[2], (uint)_previousViewport[3]);
    }

    public void Dispose()
    {
        _gl.DeleteRenderbuffer(_depthBuffer);
        _gl.DeleteTexture(_texture);
        _gl.DeleteFramebuffer(_fbo);
    }
}
