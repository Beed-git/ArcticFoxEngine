using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.OpenGL;

namespace ArcticFoxEngine.Rendering.Sprites;

public class SpriteBatch : IDisposable
{
    private ICamera _camera;
    private readonly GraphicsDevice _graphicsDevice;

    private TextureBatch _textureBatch;
    private readonly Shader _shader;

    public SpriteBatch(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _shader = new Shader(graphicsDevice.GL, _vert, _frag);
    }

    public void BeginDraw(ICamera camera)
    {
        _camera = camera;
        _textureBatch?.BeginDraw();
    }

    public void DrawSprite(Sprite sprite, Rectangle position)
    {
        if (_textureBatch is null)
        {
            _textureBatch = new TextureBatch(_graphicsDevice, sprite.Texture);
            _textureBatch.BeginDraw();
        }
        _textureBatch.DrawRectangle(sprite.Source, position, sprite.Color);
    }

    public void EndDraw()
    {
        _shader.Use();
        _shader.SetUniform("uView", _camera.ViewMatrix);
        _shader.SetUniform("uProjection", _camera.ProjectionMatrix);
        _shader.SetUniform("uTexture", 0);
            
        _textureBatch.EndDraw();
    }

    public void Dispose()
    {
        _textureBatch.Dispose();
        _shader.Dispose();
    }

    private const string _vert = @"
#version 330 core

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec4 vColor;
layout (location = 2) in vec2 vTexCoord;

uniform mat4 uView;
uniform mat4 uProjection;

out vec4 fColor;
out vec2 fTexCoord;

void main()
{
    gl_Position = vec4(vPosition, 1.0) * uView * uProjection;
    fTexCoord = vTexCoord;
    fColor = vColor;
}
";

    private const string _frag = @"
#version 330 core

in vec4 fColor;
in vec2 fTexCoord;

uniform sampler2D uTexture;

out vec4 FragColor;

void main()
{
    FragColor = texture(uTexture, fTexCoord) * fColor;
}
";
}

