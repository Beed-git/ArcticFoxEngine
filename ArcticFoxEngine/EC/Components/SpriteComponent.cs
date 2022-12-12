using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.EC.Components;

public class SpriteComponent : IComponentModel
{
    public Point Size { get; set; }
    public bool Centered { get; set; }
    public Rectangle TextureRegion { get; set; }
    public Color Color { get; set; }
    public string Texture { get; set; } 

    public SpriteComponent()
    {
        Centered = true;
        Size = Point.Zero;
        TextureRegion = Rectangle.Zero;
        Color = Color.White;
        Texture = "default";
    }
}
