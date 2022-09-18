using ArcticFoxEngine.Components;
using ArcticFoxEngine.EC;
using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Textures;
using ArcticFoxEngine.Resources;
using ArcticFoxEngine.Scripts;
using Silk.NET.OpenGL;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArcticFoxEngine;

public class Core
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SceneManager _sceneManager;

    private readonly ILogger _logger;

    private ProjectManager _projectManager;
    private ResourceManager _resourceManager;

    public Core(GraphicsDevice graphics)
    {
        _graphicsDevice = graphics;
        _sceneManager = new SceneManager();

        _logger = new ConsoleLogger();
    }

    public void OnLoad()
    {
        _projectManager = new ProjectManager("projects/project1");

        _resourceManager = new ResourceManagerBuilder(_projectManager)
            .WithLogger(_logger)
            .WithLoader(new TextureLoader(_graphicsDevice))
            .WithLoader(new ScriptFactoryLoader(_logger))
            .Build();


        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .DisableAliases()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithTagMapping(new TagName("!sprite"), typeof(SpriteComponent))
            .WithTagMapping(new TagName("!transform"), typeof(TransformComponent))
            .Build();

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping(new TagName("!sprite"), typeof(SpriteComponent))
            .WithTagMapping(new TagName("!transform"), typeof(TransformComponent))
            .Build();

        var scenePath = Path.Combine(_projectManager.AssetFolder, "scene.yml");
        if (File.Exists(scenePath))
        {
            var sceneText = File.ReadAllText(scenePath);
            var sceneModel = deserializer.Deserialize<SceneModel>(sceneText);

            var scene = new Scene(_graphicsDevice, _resourceManager);
            if (sceneModel.Camera is not null && sceneModel.Camera.ToLower() == "camera2d")
            {
                scene.MainCamera = new Camera2D();
            }
            else
            {
                _logger?.Error($"Unknown camera '{scene.MainCamera}'");
                scene.MainCamera = new Camera2D();
            }

            foreach (var (name, entModel) in sceneModel.Entities)
            {
                var ent = scene.CreateEntity(name);
                if (entModel.Components is not null)
                {
                    foreach (var component in entModel.Components)
                    {
                        ent.AddComponent(component);
                    }
                }
                if (entModel.Scripts is not null)
                {
                    foreach (var script in entModel.Scripts)
                    {
                        var scriptResource = _resourceManager.GetResource<ScriptFactory>(script);
                        if (scriptResource.Data is not null)
                        {
                            ent.AddScript(scriptResource.Data.CreateScript(ent));
                        }
                    }
                }
            }
            _sceneManager.ChangeScene(scene);
        }
        //var player = scene.CreateEntity("player");
        
        //var plyTransform = player.AddComponent<TransformComponent>();
        //plyTransform.Position = new Vector3(600, 10, 0);

        //var plySprite = player.AddComponent<SpriteComponent>();
        //plySprite.Size = new Vector2i(40, 300);
        //plySprite.Texture = "128x.png";

        //var plyScript = _resourceManager.GetResource<ScriptFactory>("TestScript.cs");
        //if (plyScript.Data is not null)
        //{
        //    player.AddScript(plyScript.Data.CreateScript(player));
        //}

        //var enemy = scene.CreateEntity("enemy");

        //var eTransform = enemy.AddComponent<TransformComponent>();
        //eTransform.Position = new Vector3(0, 0, 0);

        //var eSprite = enemy.AddComponent<SpriteComponent>();
        //eSprite.Size = new Vector2i(100, 100);
        //eSprite.TextureRegion = new Rectangle(96, 96, 32, 32);
        //eSprite.Texture = "128x.png";
        //_sceneManager.ChangeScene(scene);
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
