using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering.Sprites;

internal struct VertexPositionColorTexture
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
