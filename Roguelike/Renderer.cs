using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Render2D;
using ArcticFoxEngine.Services;

namespace Roguelike;

public class Renderer : IRenderThreadService
{
    private SpriteBatch _spriteBatch;
    private Random _random;

    public void Load()
    {
        _spriteBatch = new SpriteBatch();
        _random = new Random();

        _rects = new Rectangle[_spriteBatch.MaxSprites];
        _colors = new Color[_spriteBatch.MaxSprites];

        for (int i = 0; i < _spriteBatch.MaxSprites; i++)
        {
            var x = _random.Next(0, 1138) - 569;
            var y = _random.Next(0, 640) - 320;

            var rect = new Rectangle(x, y, 32, 32);
            _rects[i] = rect;

            var color = new Color(_random.NextSingle(), _random.NextSingle(), _random.NextSingle());
            _colors[i] = color;
        }
    }

    Rectangle[] _rects;
    Color[] _colors;

    public void Render(double dt)
    {
        _spriteBatch.BeginDraw();
        //_spriteBatch.DrawRectangle(new Rectangle(16, 16, 32, 32), new Color(127, 0, 255));
        //_spriteBatch.DrawRectangle(new Rectangle(-569, -320, 32, 32), new Color(127, 255, 127));
        for (int i = 0; i < _spriteBatch.MaxSprites; i++)
        {
            _spriteBatch.DrawRectangle(_rects[i], _colors[i]);
        }
        

        _spriteBatch.EndDraw();
    }
}
