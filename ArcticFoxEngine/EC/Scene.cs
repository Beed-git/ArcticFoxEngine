using ArcticFoxEngine.EC.Components;
using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.Sprites;
using ArcticFoxEngine.Rendering.Textures;
using ArcticFoxEngine.Resources;

namespace ArcticFoxEngine.EC;

internal class Scene
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
            if (ent.TryGetComponent<TransformComponent>(out var transform))
            {
                if (ent.TryGetComponent<TilemapComponent>(out var tilemap))
                {
                    DrawTilemapComponent(tilemap, transform);
                }
                if (ent.TryGetComponent<SpriteComponent>(out var sprite))
                {
                    DrawSpriteComponent(sprite, transform);
                }
            }
        }

        _spriteBatch.EndDraw();
    }

    private void DrawTilemapComponent(TilemapComponent tilemap, TransformComponent transform)
    {
        var spritesheet = _resourceManager.GetResource<SpriteSheet>(tilemap.SpriteSheet);
        if (spritesheet.Data is not null)
        {
            var texture = _resourceManager.GetResource<Texture2D>(spritesheet.Data.Texture);
            if (texture.Data is not null)
            {
                for (int layer = 0; layer < tilemap.TileIds.Length; layer++)
                {
                    for (int y = 0; y < tilemap.MapSize.y; y++)
                    {
                        for (int x = 0; x < tilemap.MapSize.x; x++)
                        {
                            int pos = x + y * tilemap.MapSize.x;

                            if (pos < 0 || pos >= tilemap.TileIds[layer].Length)
                            {
                                continue;
                            }

                            var position = new Rectangle(
                                (int)transform.Position.x + x,
                                (int)transform.Position.y - y,
                                1,
                                1);

                            // If tileid is 0, we assume its an empty square.
                            int tileId = tilemap.TileIds[layer][pos];
                            if (tileId <= 0)
                            {
                                continue;
                            }
                            tileId--;

                            var source = new Rectangle(
                                (int)(tileId * spritesheet.Data.Size.x % texture.Data.Width),
                                (int)(tileId * spritesheet.Data.Size.x / texture.Data.Width * spritesheet.Data.Size.y),
                                spritesheet.Data.Size.x,
                                spritesheet.Data.Size.y);

                            var sprite = new Sprite(texture.Data, source, Color.White);

                            _spriteBatch.DrawSprite(sprite, position);
                        }
                    }
                }
            }
        }
    }

    private void DrawSpriteComponent(SpriteComponent sprite, TransformComponent transform)
    {
        var texture = _resourceManager.GetResource<Texture2D>(sprite.Texture);
        if (texture.Data is not null)
        {
            var region = sprite.TextureRegion == Rectangle.Zero ? texture.Data.Bounds : sprite.TextureRegion;
            var s = new Sprite(texture.Data, region, sprite.Color);
            var position = new Rectangle((int)transform.Position.x, (int)transform.Position.y, sprite.Size.x, sprite.Size.y);
            
            if (sprite.Centered)
            {
                position = new Rectangle(
                    position.x - sprite.Size.x / 2,
                    position.y - sprite.Size.y / 2,
                    position.width,
                    position.height);
            }

            _spriteBatch.DrawSprite(s, position);
        }
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
