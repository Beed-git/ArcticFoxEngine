using ArcticFoxEngine.EC.Components;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Sprites;
using ArcticFoxEngine.Rendering.Textures;
using ArcticFoxEngine.Resources;

namespace ArcticFoxEngine.EC;

public class Scene
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly ResourceManager _resourceManager;

    private readonly SpriteBatch _spriteBatch;

    private bool _started;

    public Scene(GraphicsDevice graphicsDevice, ResourceManager resourceManager)
    {
        _graphicsDevice = graphicsDevice;
        _resourceManager = resourceManager;

        _spriteBatch = new SpriteBatch(graphicsDevice);
        EntityManager = new EntityManager();
        _started = false;
    }

    public string Name { get; set; }
    public ICamera MainCamera { get; set; }
    public Color BackgroundColor { get; set; }
    public EntityManager EntityManager { get; private init; }

    public void Start()
    {
        _started = true;
        foreach (var ent in EntityManager.GetEntities())
        {
            foreach (var script in ent.Scripts.Values)
            {
                script.OnCreate();
            }
        }
    }

    public void Update(double dt)
    {
        foreach (var ent in EntityManager.GetEntities())
        {
            foreach (var script in ent.Scripts.Values)
            {
                script.OnUpdate(dt);
            }
        }
    }

    public void Render(double dt)
    {
        _spriteBatch.BeginDraw(MainCamera);

        var entities = EntityManager.GetEntities();
        foreach (var ent in entities)
        {
            if (ent.TryGetComponent<TransformComponent>(out var transform) &&
                ent.TryGetComponent<SpriteComponent>(out var sprite))
            {
                var texture = _resourceManager.GetResource<Texture2D>(sprite.Texture);
                if (texture.Data is not null)
                {
                    var region = sprite.TextureRegion == Rectangle.Zero ? texture.Data.Bounds : sprite.TextureRegion;
                    var s = new Sprite(texture.Data, region, sprite.Color);
                    var position = new Rectangle((int)transform.Position.x - sprite.Size.x / 2, (int)transform.Position.y - sprite.Size.y / 2, sprite.Size.x, sprite.Size.y);
                    _spriteBatch.DrawSprite(s, position);
                }
            }
        }

        _spriteBatch.EndDraw();
    }

    public Entity CreateEntity()
    {
        var ent = EntityManager.CreateEntity();
        ent.Started = _started;
        return ent;
    }

    public Entity CreateEntity(string name)
    {
        var ent = EntityManager.CreateEntity(name);
        ent.Started = _started;
        return ent;
    }

    public void RemoveEntity(Entity entity)
    {
        EntityManager.RemoveEntity(entity);
    }
}
