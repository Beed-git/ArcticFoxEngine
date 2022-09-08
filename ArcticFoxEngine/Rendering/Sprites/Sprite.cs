using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Textures;

namespace ArcticFoxEngine.Rendering.Sprites;

public class Sprite
{
    public Sprite(Texture2D texture) : this(texture, texture.Bounds, Color.White)
    {
    }

    public Sprite(Texture2D texture, Rectangle source) : this(texture, source, Color.White)
    {
    }

    public Sprite(Texture2D texture, Rectangle source, Color color) 
    {
        Texture = texture;
        Source = source;
        Color = color;
    }

    public Texture2D Texture { get; }
    public Rectangle Source { get; }
    public Color Color { get; }
}
