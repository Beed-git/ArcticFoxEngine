using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Render2D;
using ArcticFoxEngine.Services;

namespace Roguelike;

public class Renderer : IRenderThreadService
{
    private SpriteBatch _spriteBatch;

    public void Load()
    {
        _spriteBatch = new SpriteBatch();
    }

    public void Render(double dt)
    {
        _spriteBatch.BeginDraw();
        _spriteBatch.DrawRectangle(new Rectangle(16, 16, 32, 32), new Color(127, 0, 255));
        _spriteBatch.DrawRectangle(new Rectangle(0, 450, 32, 32), new Color(127, 255, 127));

        for (int i = 0; i < _spriteBatch.MaxSprites; i++)
        {
            
        }

        _spriteBatch.EndDraw();
    }
}
