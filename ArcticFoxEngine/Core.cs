using ArcticFoxEngine.Components;
using ArcticFoxEngine.EC;
using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Resources;
using ArcticFoxEngine.Scripts;
using Silk.NET.OpenGL;

namespace ArcticFoxEngine;

public class Core
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SceneManager _sceneManager;

    private readonly ILogger _logger;

    private ResourceManager _resourceManager;

    public Core(GraphicsDevice graphics)
    {
        _graphicsDevice = graphics;
        _sceneManager = new SceneManager();

        _logger = new ConsoleLogger();
    }

    public void OnLoad()
    {
        _resourceManager = new ResourceManagerBuilder(_logger)
            .WithLoader(new TextureLoader(_graphicsDevice))
            .WithLoader(new ScriptFactoryLoader(_logger))
            .Build();

        var scene = new Scene(_graphicsDevice, _resourceManager);
        scene.MainCamera = new Camera2D();

        var player = scene.CreateEntity("player");
        
        var plyTransform = player.AddComponent<TransformComponent>();
        plyTransform.Position = new Vector3(600, 10, 0);

        var plySprite = player.AddComponent<SpriteComponent>();
        plySprite.Size = new Vector2i(40, 300);
        plySprite.Texture = "128x.png";

        var plyScript = _resourceManager.GetResource<ScriptFactory>("TestScript.cs");
        if (plyScript.Data is not null)
        {
            player.AddScript(plyScript.Data.CreateScript(player));
        }

        var enemy = scene.CreateEntity("enemy");

        var eTransform = enemy.AddComponent<TransformComponent>();
        eTransform.Position = new Vector3(0, 0, 0);

        var eSprite = enemy.AddComponent<SpriteComponent>();
        eSprite.Size = new Vector2i(100, 100);
        eSprite.TextureRegion = new Rectangle(96, 96, 32, 32);
        eSprite.Texture = "128x.png";
        _sceneManager.ChangeScene(scene);
    }

    public void OnUpdate(double dt)
    {
        _sceneManager.Update(dt);
    }

    public unsafe void OnRender(double dt)
    {
        _graphicsDevice.GL.ClearColor(System.Drawing.Color.SteelBlue);
        _graphicsDevice.GL.Clear(ClearBufferMask.ColorBufferBit);

        _sceneManager.Render(dt);
    }

    public void OnClose()
    {
        _resourceManager.Dispose();
    }

    public void OnResize(Vector2i size)
    {
        _sceneManager?.CurrentScene?.MainCamera.UpdateAspectRatio(size);
    }
}
