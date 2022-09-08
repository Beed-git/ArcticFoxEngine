using ArcticFoxEngine.EC;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Sprites;
using ArcticFoxEngine.Rendering.Textures;
using Silk.NET.OpenGL;

namespace ArcticFoxEngine;

public class Core
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SceneManager _sceneManager;

    private SpriteBatch _spriteBatch;
    private Texture2D _texture;

    private Sprite _testSprite;
    private Sprite _testSprite2;

    private ICamera _camera;

    public Core(GraphicsDevice graphics)
    {
        _graphicsDevice = graphics;
        _sceneManager = new SceneManager();
    }

    public void OnLoad()
    {
        _camera = new Camera2D();

        _sceneManager.ChangeScene(new Scene());

        int size = 16;
        float stride = 256.0f / size;

        _texture = new Texture2D(_graphicsDevice, (uint)size, (uint)size);
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
        _texture.SetData(_texture.Bounds, data);

        _spriteBatch = new SpriteBatch(_graphicsDevice);

        _testSprite = new Sprite(_texture);
        _testSprite2 = new Sprite(_texture, _texture.Bounds, Color.Red);
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
        _spriteBatch.DrawSprite(_testSprite, new Rectangle(0, 0, 100, 100));
        _spriteBatch.DrawSprite(_testSprite2, new Rectangle(10, 50, 100, 100));
        _spriteBatch.EndDraw();
    }

    public void OnClose()
    {
        _spriteBatch.Dispose();
        _texture.Dispose();
    }

    public void OnResize(Vector2i size)
    {
        _camera.UpdateAspectRatio(size);
    }
}
