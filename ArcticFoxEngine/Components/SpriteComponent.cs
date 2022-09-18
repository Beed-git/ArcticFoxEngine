using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Components;

public class SpriteComponent : ComponentModel
{
    public Point Size { get; set; }
    public Rectangle TextureRegion { get; set; }
    public Color Color { get; set; }
    public string Texture { get; set; } 

    public SpriteComponent()
    {
        Color = Color.White;
    }
}
