using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.OpenGL;
using ArcticFoxEngine.Rendering.Textures;
using Silk.NET.OpenGL;

namespace ArcticFoxEngine.Rendering.Sprites;

internal class TextureBatch : IDisposable
{
    private readonly GL _gl;
    private readonly uint _maxSprites;
    private readonly Texture2D _texture;

    private uint _pointer;
    private readonly uint[] _indices;
    private readonly VertexPositionColorTexture[] _vertices;

    private readonly VertexArrayObject<VertexPositionColorTexture, uint> _vao;
    private readonly BufferObject<uint> _ebo;
    private readonly BufferObject<VertexPositionColorTexture> _vbo;

    public TextureBatch(GraphicsDevice graphics, Texture2D texture, uint maxSprites = 2000)
    {
        _gl = graphics.GL;
        _texture = texture;
        _maxSprites = maxSprites;

        _pointer = 0;
        _indices = new uint[6 * _maxSprites];
        _vertices = new VertexPositionColorTexture[4 * _maxSprites];

        _vao = new VertexArrayObject<VertexPositionColorTexture, uint>(_gl);
        _vao.Bind();

        _vbo = new BufferObject<VertexPositionColorTexture>(_gl, (uint)_vertices.Length, BufferTargetARB.ArrayBuffer);
        _ebo = new BufferObject<uint>(_gl, (uint)_indices.Length, BufferTargetARB.ElementArrayBuffer);

        _vao.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0);
        _vao.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 3 * sizeof(float));
        _vao.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 7 * sizeof(float));

        _vao.Unbind();
    }

    public void BeginDraw()
    {
        _pointer = 0;
    }

    public void DrawRectangle(Rectangle source, Rectangle destination, Color color)
    {
        if (_pointer >= _maxSprites)
        {
            throw new Exception($"Attempted to draw too many rectangles in texturebatch of size {_maxSprites}.");
        }

        float z = 0.0f;

        int x1 = destination.x;
        int y1 = destination.y;
        int x2 = destination.x + destination.width;
        int y2 = destination.y + destination.height;

        float tx1 = (float)source.x / _texture.Width;
        float tx2 = tx1 + (float)source.width / _texture.Width;
        float ty2 = (float)(_texture.Height - source.y) / _texture.Height;
        float ty1 = ty2 - (float)source.height / _texture.Height;

        var vColor = Color.ToVector4(color);

        _vertices[(4 * _pointer) + 0] = new VertexPositionColorTexture(new Vector3(x1, y1, z), vColor, new Vector2(tx1, ty1)); // Bottom-left vertex
        _vertices[(4 * _pointer) + 1] = new VertexPositionColorTexture(new Vector3(x2, y1, z), vColor, new Vector2(tx2, ty1)); // Bottom-right vertex
        _vertices[(4 * _pointer) + 2] = new VertexPositionColorTexture(new Vector3(x1, y2, z), vColor, new Vector2(tx1, ty2)); // Top-left vertex
        _vertices[(4 * _pointer) + 3] = new VertexPositionColorTexture(new Vector3(x2, y2, z), vColor, new Vector2(tx2, ty2)); // Top-right vertex

        _indices[(6 * _pointer) + 0] = _pointer * 4 + 0;
        _indices[(6 * _pointer) + 1] = _pointer * 4 + 1;
        _indices[(6 * _pointer) + 2] = _pointer * 4 + 2;

        _indices[(6 * _pointer) + 3] = _pointer * 4 + 2;
        _indices[(6 * _pointer) + 4] = _pointer * 4 + 1;
        _indices[(6 * _pointer) + 5] = _pointer * 4 + 3;

        _pointer++;
    }

    /// <summary>
    /// A shader must be used before running this function.
    /// </summary>
    public unsafe void EndDraw()
    {
        _vbo.BufferData(new Span<VertexPositionColorTexture>(_vertices, 0, (int)(_pointer * 4)));
        _ebo.BufferData(new Span<uint>(_indices, 0, (int)(_pointer * 6)));

        _texture.Bind();

        _vao.Bind();
        _gl.DrawElements(PrimitiveType.Triangles, _pointer * 6, DrawElementsType.UnsignedInt, null);
        _vao.Unbind();
    }

    public void Dispose()
    {
        _vao.Dispose();
        _ebo.Dispose();
        _vbo.Dispose();
    }
}
