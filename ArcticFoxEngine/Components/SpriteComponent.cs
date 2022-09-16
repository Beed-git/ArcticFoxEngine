using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Components;

public class SpriteComponent : Component
{
    public Vector2i Size { get; set; }
    public Rectangle TextureRegion { get; set; }
    public Color Color { get; set; }
    public string Texture { get; set; } 

    public SpriteComponent(Entity parent) : base(parent)
    {
        Color = Color.White;
    }
}
