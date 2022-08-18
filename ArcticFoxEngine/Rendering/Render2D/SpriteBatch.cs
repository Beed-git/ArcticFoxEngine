using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public class SpriteBatch : ISpriteBatch, IDisposable
{
    private int _pointer;

    private readonly VertexPositionColor[] _vertices;
    private readonly uint[] _indices;

    private readonly VertexArrayObject _vao;
    private readonly BufferObject<VertexPositionColor> _vbo;
    private readonly BufferObject<uint> _ebo;

    private readonly Shader _shader;

    public SpriteBatch()
    {
        _vertices = new VertexPositionColor[MaxSprites * 4];
        _indices = new uint[MaxSprites * 6];

        _vao = new VertexArrayObject(7 * sizeof(float));
        _vao.Bind();

        _vbo = new BufferObject<VertexPositionColor>(_vertices.Length, BufferTarget.ArrayBuffer);
        _ebo = new BufferObject<uint>(_indices.Length, BufferTarget.ElementArrayBuffer);

        _vao.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0);
        _vao.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 3 * sizeof(float));

        _shader = new Shader(vert, frag);
        _shader.Use();
    }

    public int MaxSprites { get; } = 2000;

    public void BeginDraw(Matrix4? mvp = null)
    {
        var cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        var cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
        var view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 20.0f),
            cameraFront, cameraUp);

        var aspectRatio = 1920.0f / 1080.0f;
        var h = 32.0f * 20.0f;
        var w = h * aspectRatio;

        var projection = Matrix4.CreateOrthographic(w, h, 0.0f, 100.0f);

        mvp = view * projection;
        _pointer = 0;
        _shader.SetUniform("mvp", mvp ?? Matrix4.Identity);
    }

    public void DrawRectangle(Rectangle destination, Color color)
    {
        if (_pointer >= MaxSprites)
        {
            throw new Exception($"Attempted to draw too many rectangles in a full spritebatch of size {MaxSprites}.");
        }

        int x1 = destination.x;
        int y1 = destination.y;
        int x2 = destination.x + destination.width;
        int y2 = destination.y + destination.height;

        var vColor = color.ToVector4();

        _vertices[(4 * _pointer) + 0] = new VertexPositionColor(new Vector3(x1, y1, 0.0f), vColor); // Bottom-left vertex
        _vertices[(4 * _pointer) + 1] = new VertexPositionColor(new Vector3(x2, y1, 0.0f), vColor); // Bottom-right vertex
        _vertices[(4 * _pointer) + 2] = new VertexPositionColor(new Vector3(x1, y2, 0.0f), vColor); // Top-left vertex
        _vertices[(4 * _pointer) + 3] = new VertexPositionColor(new Vector3(x2, y2, 0.0f), vColor); // Top-right vertex

        _indices[(6 * _pointer) + 0] = (uint)_pointer * 4 + 0;
        _indices[(6 * _pointer) + 1] = (uint)_pointer * 4 + 1;
        _indices[(6 * _pointer) + 2] = (uint)_pointer * 4 + 2;
                 
        _indices[(6 * _pointer) + 3] = (uint)_pointer * 4 + 2;
        _indices[(6 * _pointer) + 4] = (uint)_pointer * 4 + 1;
        _indices[(6 * _pointer) + 5] = (uint)_pointer * 4 + 3;

        _pointer++;
    }

    public void EndDraw()
    {
        _vbo.BufferData(_vertices);
        _ebo.BufferData(_indices);

        _shader.Use();
        _vao.Bind();
        GL.DrawElements(PrimitiveType.Triangles, _pointer * 6, DrawElementsType.UnsignedInt, 0); 
    }

    public void Dispose()
    {
        _vbo.Dispose();
        _ebo.Dispose();
        _vao.Dispose();
        _shader.Dispose();
    }

    private struct VertexPositionColor
    {
        public Vector3 position;
        public Vector4 color;

        public VertexPositionColor(Vector3 position, Vector4 color)
        {
            this.position = position;
            this.color = color;
        }

        public override string ToString()
        {
            return $"({position},{color})";
        }
    }

    private const string vert = @"
#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec4 aColor;

out vec4 fColor;

uniform mat4 mvp;

void main(void)
{
    fColor = aColor;
    gl_Position = vec4(aPosition, 1.0) * mvp;
}
";

    private const string frag = @"
#version 330

in vec4 fColor;

out vec4 outputColor;

void main()
{
    outputColor = fColor;
}
";
}
