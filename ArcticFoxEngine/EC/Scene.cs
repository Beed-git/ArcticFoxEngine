using ArcticFoxEngine.Components;
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

    private readonly EntityManager _entityManager;
    private bool _started;

    public Scene(GraphicsDevice graphicsDevice, ResourceManager resourceManager)
    {
        _graphicsDevice = graphicsDevice;
        _resourceManager = resourceManager;

        _spriteBatch = new SpriteBatch(graphicsDevice);
        _entityManager = new EntityManager();
        _started = false;
    }

    public ICamera MainCamera { get; set; }

    public void Start()
    {
        _started = true;
        foreach (var ent in _entityManager.GetEntities())
        {
            foreach (var script in ent.Scripts)
            {
                script.OnCreate();
            }
        }
    }

    public void Update(double dt)
    {
        foreach (var ent in _entityManager.GetEntities())
        {
            foreach (var script in ent.Scripts)
            {
                script.OnUpdate(dt);
            }
        }
    }

    public void Render(double dt)
    {
        _spriteBatch.BeginDraw(MainCamera);

        var entities = _entityManager.GetEntities();
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
        var ent = _entityManager.CreateEntity();
        ent.Started = _started;
        return ent;
    }

    public Entity CreateEntity(string name)
    {
        var ent = _entityManager.CreateEntity(name);
        ent.Started = _started;
        return ent;
    }

    public void RemoveEntity(Entity entity)
    {
        _entityManager.RemoveEntity(entity);
    }
}
