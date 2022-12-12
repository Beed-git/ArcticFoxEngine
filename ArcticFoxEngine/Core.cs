using ArcticFoxEngine.EC;
using ArcticFoxEngine.EC.Components;
using ArcticFoxEngine.Logging;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Resources;
using ArcticFoxEngine.Resources.Loaders;
using ArcticFoxEngine.Scripts;
using Silk.NET.OpenGL;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArcticFoxEngine;

internal class Core
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly GameWindow _window;
    private readonly ILogger? _logger;

    private readonly FileManager _fileManager;
    private readonly ResourceManager _resourceManager;
    private readonly SceneManager _sceneManager;

    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    private readonly List<Type> _scriptTypes;
    private readonly StartupModel? _startupModel;

    public Core(GameWindow window, GraphicsDevice graphics) : this(window, graphics, null)
    {
    }

    public Core(GameWindow window, GraphicsDevice graphics, ILogger? logger)
    {
        _window = window;
        _logger = logger;
        _graphicsDevice = graphics;

        _fileManager = new FileManager();
        _sceneManager = new SceneManager();
        _resourceManager = new ResourceManager(_fileManager, _logger);

        _scriptTypes = new List<Type>();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .DisableAliases()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithTagMapping(new TagName("!sprite"), typeof(SpriteComponent))
            .WithTagMapping(new TagName("!transform"), typeof(TransformComponent))
            .WithTagMapping(new TagName("!tilemap"), typeof(TilemapComponent))
            .Build();

        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping(new TagName("!sprite"), typeof(SpriteComponent))
            .WithTagMapping(new TagName("!transform"), typeof(TransformComponent))
            .WithTagMapping(new TagName("!tilemap"), typeof(TilemapComponent))
            .Build();

        var startup = Path.Combine(_fileManager.AssetFolder, "startup.yml");
        if (File.Exists(startup))
        {
            var startupText = File.ReadAllText(startup);
            _startupModel = _deserializer.Deserialize<StartupModel>(startupText);

            if (_startupModel.Title is not null)
            {
                _window.Title = _startupModel.Title;
            }
        }
        else
        {
            _startupModel = null;
            _logger?.Warn("No startup.yml found.");
        }
    }

    public Core WithScripts(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            if (type.IsAssignableTo(typeof(BaseScript)) && !type.IsAbstract && !type.IsGenericType)
            {
                _scriptTypes.Add(type);
            }
            else if (type.IsGenericType)
            {
                _logger?.Warn($"Invalid script provided, script cannot be generic: {type.Name}");
            }
            else if (type.IsAbstract)
            {
                _logger?.Warn($"Invalid script provided, script cannot be abstract: {type.Name}");
            }
            else
            {
                _logger?.Warn($"Invalid script provided, script needs to implement {nameof(BaseScript)}: {type.Name}");
            }
        }
        return this;
    }

    private Scene? CurrentScene => _sceneManager?.CurrentScene;

    public void OnLoad()
    {
        _resourceManager.AddLoader(new SpriteSheetLoader(_deserializer))
             .AddLoader(new ScriptFactoryLoader(_logger, _scriptTypes))
             .AddLoader(new TextureLoader(_graphicsDevice))
             .AddLoader(new SceneLoader(_resourceManager, _deserializer, _graphicsDevice));

        if (_startupModel?.StartScene is not null)
        {
            var scene = _resourceManager.GetResource<Scene>(_startupModel.StartScene);
            if (scene.Data is not null)
            {
                _sceneManager.ChangeScene(scene.Data);
            }
        }
    }

    public void OnUpdate(double dt)
    {
        _sceneManager.Update(dt);
    }

    public unsafe void OnRender(double dt)
    {
        var backgroundColor = CurrentScene is null ? Color.SteelBlue : CurrentScene.BackgroundColor;

        _graphicsDevice.GL.ClearColor(backgroundColor);
        _graphicsDevice.GL.Clear(ClearBufferMask.ColorBufferBit);

        _graphicsDevice.GL.Enable(EnableCap.Blend);
        _graphicsDevice.GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _sceneManager.Render(dt);

        _graphicsDevice.GL.Disable(EnableCap.Blend);
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
