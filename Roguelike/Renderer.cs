using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Render2D;
using ArcticFoxEngine.Services;
using ArcticFoxEngine.Services.GraphicsManager;
using ArcticFoxEngine.Services.TextureManager;

namespace Roguelike;

public class Renderer : IInitService, IRenderService
{
    private const int _spriteCount = 7000;

    private SpriteBatch _spriteBatch;
    private SpriteSheet _spriteSheet;
    private Random _random;

    private readonly IGraphicsManager _graphicsManager;
    private readonly ITextureManager _textureManager;
    private ITexture2D _texture;
    private ITexture2D _texture2;

    private Rectangle[] _dests;
    private Rectangle[] _sources;
    private Color[] _colors;

    private Camera2D _camera;
    private Camera2D _viewportCamera;

    public Renderer(ITextureManager textureManager, IGraphicsManager graphicsManager)
    {
        _textureManager = textureManager;
        _graphicsManager = graphicsManager;
    }

    public void Init()
    {
        _camera = new Camera2D();
        _viewportCamera = new Camera2D();

        _graphicsManager.OnResize += (s, e) => _camera.UpdateAspectRatio(e.x, e.y);
        _graphicsManager.OnResize += (s, e) =>
        {
            _viewportCamera.UpdateAspectRatio(e.x, e.y);
        };

        _texture = _textureManager.LoadTexture("Assets/SpriteSheet.png");
        _texture2 = _textureManager.LoadTexture("Assets/128x.png");

        _spriteSheet = new SpriteSheet(16, 16, _texture);

        _spriteBatch = new SpriteBatch(_graphicsManager);
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

    double t = 0;

    public void Render(double dt)
    {
        t += dt;
        double cx = Math.Sin(t) * 100;
        double cy = Math.Cos(t) * 100;
        _camera.Position = new Vector2((float)cx, (float)cy);

        _spriteBatch.BeginDraw(_camera);
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

        dst = new Rectangle(0, 0, 512, 512);
        src = _texture2.Bounds;
        _spriteBatch.BeginDraw(_viewportCamera);
        _spriteBatch.DrawRectangle(_texture2, dst, src, Color.White);
        _spriteBatch.EndDraw();
    }
}
