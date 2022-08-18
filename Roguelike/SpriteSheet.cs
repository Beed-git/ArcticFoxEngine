using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;

namespace Roguelike;

public class SpriteSheet
{
    private readonly int _spriteX;

    public SpriteSheet(int spriteWidth, int spriteHeight, ITexture2D texture)
    {
        SpriteWidth = spriteWidth;
        SpriteHeight = spriteHeight;
        Texture = texture;

        _spriteX = Texture.Width / SpriteWidth;
    }

    public int SpriteWidth { get; private init; }
    public int SpriteHeight { get; private init; }
    public ITexture2D Texture { get; private init; }

    public Rectangle GetSource(int id)
    {
        int x = id % _spriteX;
        int y = id / _spriteX;
        return new Rectangle(x * SpriteWidth, y * SpriteHeight, SpriteWidth, SpriteHeight);
    }
}
