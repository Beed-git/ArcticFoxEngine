using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Components;

public class SpriteComponent : Component
{
    public Vector2i Size;
    public Rectangle TextureRegion;
    public Color Color;
    public string Texture;

    public SpriteComponent(Entity parent) : base(parent)
    {
        Color = Color.White;
    }
}
