using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.OpenGL;
using ArcticFoxEngine.Rendering.Render2D;
using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.TextureManager;
using OpenTK.Graphics.ES20;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Roguelike;

public class Renderer : IRenderThreadService
{
    private SpriteBatch _spriteBatch;
    private Random _random;

    private readonly ITextureManager _textureManager;
    private ITexture2D _texture;

    private Rectangle[] _dests;
    private Rectangle[] _sources;
    private Color[] _colors;

    public Renderer(ITextureManager textureManager)
    {
        _textureManager = textureManager;
    }

    public void Load()
    {
        _texture = _textureManager.LoadTexture("Assets/SpriteSheet.png");

        _spriteBatch = new SpriteBatch(_texture);
        _random = new Random();

        _dests = new Rectangle[_spriteBatch.MaxSprites];
        _sources = new Rectangle[_spriteBatch.MaxSprites];
        _colors = new Color[_spriteBatch.MaxSprites];

        for (int i = 0; i < _spriteBatch.MaxSprites; i++)
        {
            var x = _random.Next(0, 1138) - 569;
            var y = _random.Next(0, 640) - 320;

            var dest = new Rectangle(x, y, 32, 32);
            _dests[i] = dest;

            x = (int)(_random.NextSingle() * _texture.Width);
            y = (int)(_random.NextSingle() * _texture.Height);
            
            int w = Math.Min((int)(_random.NextSingle() * _texture.Width), (_texture.Width -  x));
            int h = Math.Min((int)(_random.NextSingle() * _texture.Height), (_texture.Height - y));
            
            var source = new Rectangle(x, y, w, h);
            _sources[i] = source;

            var color = new Color(_random.NextSingle(), _random.NextSingle(), _random.NextSingle());
            _colors[i] = color;
        }
    }

    public void Render(double dt)
    {
        _spriteBatch.BeginDraw();
        //for (int i = 0; i < _spriteBatch.MaxSprites; i++)
        //{
        //    _spriteBatch.DrawRectangle(_dests[i], _sources[i], _colors[i]);
        //}

        var spriteSheet = new SpriteSheet(16, 16, _texture);

        Rectangle src, dest;

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                dest = spriteSheet.GetDestination(x, y);
                src = spriteSheet.GetSource(0);
                _spriteBatch.DrawRectangle(dest, src, Color.White);
            }
        }

        dest = spriteSheet.GetDestination(0, 0);
        src = spriteSheet.GetSource(2);
        _spriteBatch.DrawRectangle(dest, src, Color.White);

        _spriteBatch.EndDraw();
    }
}
