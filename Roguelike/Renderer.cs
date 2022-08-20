using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Render2D;
using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.TextureManager;
using OpenTK.Mathematics;

namespace Roguelike;

public class Renderer : IRenderService
{
    private const int _spriteCount = 7000;

    private SpriteBatch _spriteBatch;
    private SpriteSheet _spriteSheet;
    private Random _random;

    private readonly ITextureManager _textureManager;
    private ITexture2D _texture;
    private ITexture2D _texture2;

    private Rectangle[] _dests;
    private Rectangle[] _sources;
    private Color[] _colors;

    private Camera2D _camera;

    public Renderer(ITextureManager textureManager)
    {
        _textureManager = textureManager;
    }

    public void Load()
    {
        _camera = new Camera2D();

        _texture = _textureManager.LoadTexture("Assets/SpriteSheet.png");
        _texture2 = _textureManager.LoadTexture("Assets/128x.png");

        _spriteSheet = new SpriteSheet(16, 16, _texture);

        _spriteBatch = new SpriteBatch();
        _random = new Random();

        _dests = new Rectangle[_spriteCount];
        _sources = new Rectangle[_spriteCount];
        _colors = new Color[_spriteCount];

        for (int i = 0; i < _spriteCount; i++)
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
        var pos = _camera.Position;
        _camera.Position = new Vector2(pos.X - (float)(16 * dt), pos.Y);

        _spriteBatch.BeginDraw(_camera.ViewMatrix * _camera.ProjectionMatrix);
        for (int i = 0; i < _spriteCount; i++)
        {
            _spriteBatch.DrawRectangle(_texture2, _dests[i], _sources[i], _colors[i]);
        }

        Rectangle src, dst;

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                dst = _spriteSheet.GetDestination(x, y);
                src = _spriteSheet.GetSource(0);
                _spriteBatch.DrawRectangle(_texture, dst, src, Color.White);
            }
        }

        dst = _spriteSheet.GetDestination(0, 0);
        src = _spriteSheet.GetSource(2);
        _spriteBatch.DrawRectangle(_texture, dst, src, Color.White);

        _spriteBatch.EndDraw();
    }
}
