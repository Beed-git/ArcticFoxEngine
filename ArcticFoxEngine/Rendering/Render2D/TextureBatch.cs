﻿using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.OpenGL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public class TextureBatch : ITextureBatch, IDisposable
{
    private int _pointer;

    private readonly VertexPositionColorTexture[] _vertices;
    private readonly uint[] _indices;

    private readonly VertexArrayObject _vao;
    private readonly BufferObject<VertexPositionColorTexture> _vbo;
    private readonly BufferObject<uint> _ebo;

    private readonly Shader _shader;
    private readonly ITexture2D _texture;

    public TextureBatch(ITexture2D texture, int maxSprites = 2000)
    {
        MaxSprites = maxSprites;
        _texture = texture;
        _vertices = new VertexPositionColorTexture[MaxSprites * 4];
        _indices = new uint[MaxSprites * 6];

        _vao = new VertexArrayObject(9 * sizeof(float));
        _vao.Bind();

        _vbo = new BufferObject<VertexPositionColorTexture>(_vertices.Length, BufferTarget.ArrayBuffer);
        _ebo = new BufferObject<uint>(_indices.Length, BufferTarget.ElementArrayBuffer);

        _vao.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0);
        _vao.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 3 * sizeof(float));
        _vao.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 7 * sizeof(float));

        _shader = new Shader(vert, frag);
        _shader.Use();
    }

    public bool Drawing { get; private set; }
    public int MaxSprites { get; private init; }

    public void BeginDraw(Matrix4? mvp = null)
    {
        if (Drawing)
        {
            throw new InvalidOperationException("TextureBatch is already drawing, can't call TextureBatch.BeginDraw() while drawing.");
        }
        Drawing = true;
        _pointer = 0;
        _shader.SetUniform("mvp", mvp ?? Matrix4.Identity);
    }

    public void DrawRectangle(Rectangle destination, Rectangle source, Color color)
    {
        if (!Drawing)
        {
            throw new InvalidOperationException("TextureBatch isn't drawing, can't call TextureBatch.DrawRectangle() while not drawing.");
        }
        if (_pointer >= MaxSprites)
        {
            throw new Exception($"Attempted to draw too many rectangles in a full spritebatch of size {MaxSprites}.");
        }

        var vColor = color.ToVector4();

        int x1 = destination.x;
        int y1 = destination.y;
        int x2 = destination.x + destination.width;
        int y2 = destination.y + destination.height;

        float tx1 = (float)source.x / _texture.Width;
        float tx2 = tx1 + (float)source.width / _texture.Width;
        float ty2 = (float)(_texture.Height - source.y) / _texture.Height;
        float ty1 = ty2 - (float)source.height / _texture.Height;

        _vertices[(4 * _pointer) + 0] = new VertexPositionColorTexture(new Vector3(x1, y1, 0.0f), vColor, new Vector2(tx1, ty1)); // Bottom-left vertex
        _vertices[(4 * _pointer) + 1] = new VertexPositionColorTexture(new Vector3(x2, y1, 0.0f), vColor, new Vector2(tx2, ty1)); // Bottom-right vertex
        _vertices[(4 * _pointer) + 2] = new VertexPositionColorTexture(new Vector3(x1, y2, 0.0f), vColor, new Vector2(tx1, ty2)); // Top-left vertex
        _vertices[(4 * _pointer) + 3] = new VertexPositionColorTexture(new Vector3(x2, y2, 0.0f), vColor, new Vector2(tx2, ty2)); // Top-right vertex

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
        if (!Drawing)
        {
            throw new InvalidOperationException("TextureBatch isn't drawing, can't call TextureBatch.EndDraw() while not drawing.");
        }
        Drawing = false;
        _vbo.BufferData(_vertices);
        _ebo.BufferData(_indices);

        _vao.Bind();
        _texture.Bind();
        _shader.Use();
        GL.DrawElements(PrimitiveType.Triangles, _pointer * 6, DrawElementsType.UnsignedInt, 0); 
    }

    public void Dispose()
    {
        _vbo.Dispose();
        _ebo.Dispose();
        _vao.Dispose();
        _shader.Dispose();
    }

    private struct VertexPositionColorTexture
    {
        public Vector3 position;
        public Vector4 color;
        public Vector2 texture;

        public VertexPositionColorTexture(Vector3 position, Vector4 color, Vector2 texture)
        {
            this.position = position;
            this.color = color;
            this.texture = texture;
        }

        public override string ToString()
        {
            return $"({position},{color},{texture})";
        }
    }

    private const string vert = @"
#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec4 aColor;
layout(location = 2) in vec2 aTexCoord;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 mvp;

void main(void)
{
    fColor = aColor;
    fTexCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0) * mvp;
}
";

    private const string frag = @"
#version 330

in vec4 fColor;
in vec2 fTexCoord;

out vec4 outputColor;

uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, fTexCoord) * fColor;
}
";
}
