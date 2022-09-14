using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Textures;

namespace ArcticFoxEngine.Rendering.Sprites;

public struct Sprite
{
    public Texture2D Texture;
    public Rectangle Source;
    public Color Color;

    public Sprite(Texture2D texture, Rectangle source, Color color) 
    {
        Texture = texture;
        Source = source;
        Color = color;
    }
}
