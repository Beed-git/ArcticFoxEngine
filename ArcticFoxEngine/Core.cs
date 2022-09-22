using ArcticFoxEngine.EC;
using ArcticFoxEngine.EC.Components;
using ArcticFoxEngine.EC.Converters;
using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Textures;
using ArcticFoxEngine.Resources;
using ArcticFoxEngine.Scripts;
using Silk.NET.OpenGL;
using System.Reflection;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArcticFoxEngine;

public class Core
{
    private readonly GraphicsDevice _graphicsDevice;

    private readonly ILogger? _logger;

    private FileManager _projectManager;
    private ResourceManager _resourceManager;
    private SceneManager _sceneManager;

    public Core(GraphicsDevice graphics, ILogger? logger = null)
    {
        _logger = logger;
        _graphicsDevice = graphics;
    }

    public Scene? CurrentScene => _sceneManager?.CurrentScene;
    public ResourceManager ResourceManager => _resourceManager;

    public void OnLoad()
    {
        _projectManager = new FileManager("projects/project1");

        _sceneManager = new SceneManager();

        var assemblies = new List<Assembly>();
        var files = Directory.GetFiles(_projectManager.ProjectDirectory, "*.dll");
        foreach (var file in files)
        {
            assemblies.Add(Assembly.LoadFile(file));
        }

        _resourceManager = new ResourceManagerBuilder(_projectManager)
            .WithLogger(_logger)
            .WithLoader(new TextureLoader(_graphicsDevice))
            .WithLoader(new ScriptFactoryLoader(_logger, assemblies))
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

            var scene = SceneModelConverter.ToScene(_graphicsDevice, _resourceManager, sceneModel);
            _sceneManager.ChangeScene(scene);

            var model = SceneModelConverter.ToModel(scene);
            File.WriteAllText("out.yml", serializer.Serialize(model));
        }
    }

    public void OnUpdate(double dt)
    {
        _sceneManager.Update(dt);
    }

    public unsafe void OnRender(double dt)
    {
        if (CurrentScene is not null)
        {

        }
        var backgroundColor = CurrentScene is null ? Color.SteelBlue : CurrentScene.BackgroundColor;

        _graphicsDevice.GL.ClearColor(backgroundColor);
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
