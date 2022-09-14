using ArcticFoxEngine.Components;
using ArcticFoxEngine.EC;
using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Resources;
using ArcticFoxEngine.Rendering.Sprites;
using ArcticFoxEngine.Rendering.Textures;
using Silk.NET.OpenGL;

namespace ArcticFoxEngine;

public class Core
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SceneManager _sceneManager;

    private readonly ILogger _logger;

    private SpriteBatch _spriteBatch;

    private Sprite _testSprite;
    private Sprite _testSprite2;

    private ResourceManager _resourceManager;

    private ICamera _camera;

    public Core(GraphicsDevice graphics)
    {
        _graphicsDevice = graphics;
        _sceneManager = new SceneManager();

        _logger = new ConsoleLogger();
    }

    public void OnLoad()
    {
        _camera = new Camera2D();

        var scene = new Scene();
        var entity = scene.CreateEntity();
        entity.AddComponent(new TransformComponent() { X = 40, Y = 20 });
        _sceneManager.ChangeScene(scene);

        _resourceManager = new ResourceManagerBuilder(_logger)
            .WithLoader(new TextureLoader(_graphicsDevice))
            .Build();

        
        int size = 16;
        float stride = 256.0f / size;

        var texture = new Texture2D(_graphicsDevice, (uint)size, (uint)size);
        byte[] data = new byte[size * size * 4];
        for (int r = 0; r < size; r++) 
        {
            for (int b = 0; b < size; b++) 
            {
                int key = (r * size + b) * 4;
                data[key + 0] = (byte)(r * stride);
                data[key + 1] = 0;
                data[key + 2] = (byte)(b * stride);
                data[key + 3] = byte.MaxValue;
            }
        }
        texture.SetData(texture.Bounds, data);

        var t1 = _resourceManager.CreateResource("test//@", texture);
        var t2 = _resourceManager.GetResource<Texture2D>("128x.png");

        _spriteBatch = new SpriteBatch(_graphicsDevice);

        _testSprite = new Sprite(t1.Data);
        _testSprite2 = new Sprite(t2.Data);
    }

    public void OnUpdate(double dt)
    {
        _sceneManager.Update(dt);
    }

    public unsafe void OnRender(double dt)
    {
        _graphicsDevice.GL.ClearColor(System.Drawing.Color.SteelBlue);
        _graphicsDevice.GL.Clear(ClearBufferMask.ColorBufferBit);

        _spriteBatch.BeginDraw(_camera);
        _spriteBatch.DrawSprite(_testSprite, new Rectangle(0, 0, 200, 200));
        _spriteBatch.DrawSprite(_testSprite2, new Rectangle(10, 50, 200, 200));
        _spriteBatch.EndDraw();

        _spriteBatch.EndDraw();
    }

    public void OnClose()
    {
        _spriteBatch.Dispose();
        _resourceManager.Dispose();
    }

    public void OnResize(Vector2i size)
    {
        _camera.UpdateAspectRatio(size);
    }
}
