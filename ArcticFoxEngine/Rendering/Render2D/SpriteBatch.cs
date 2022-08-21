using ArcticFoxEngine.Math;
using ArcticFoxEngine.Rendering.Camera;
using ArcticFoxEngine.Rendering.OpenGL;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public class SpriteBatch : ISpriteBatch
{
    private const int _maxSprites = 2000;
    private Matrix4 _mvp;

    private readonly Shader _shader;
    private readonly Dictionary<ITexture2D, Batch> _batchs;
    private Queue<BatchLifetime> _batchPool;

    public SpriteBatch()
    {
        _mvp = Matrix4.Identity;
        _shader = new Shader(vert, frag);
        _batchs = new Dictionary<ITexture2D, Batch>();
        _batchPool = new Queue<BatchLifetime>();

        TextureBatchLifetime = 4;
    }

    public bool Drawing { get; private set; }
    public int TextureBatchLifetime { get; set; }

    public void BeginDraw(Matrix4 mvp)
    {
        if (Drawing)
        {
            throw new InvalidOperationException("Spritebatch is already drawing, can't call SpriteBatch.BeginDraw() while drawing.");
        }
        Drawing = true;
        _mvp = mvp;
    }

    public void BeginDraw(ICamera camera)
    {
        BeginDraw(camera.ViewMatrix * camera.ProjectionMatrix);
    }

    public void DrawRectangle(ITexture2D texture, Rectangle destination, Rectangle source, Color color)
    {
        if (!Drawing)
        {
            throw new InvalidOperationException("SpriteBatch isn't drawing, can't call SpriteBatch.DrawRectangle() while not drawing.");
        }

        if (_batchs.TryGetValue(texture, out var batchs))
        {
            int key = batchs.SpriteCount / _maxSprites;
            if (key < batchs.Batchs.Count)
            {
                batchs.Batchs[key].DrawRectangle(destination, source, color);
            }
            else
            {
                var batch = GetBatch();
                batch.BeginDraw(texture);
                batch.DrawRectangle(destination, source, color);
                batchs.Batchs.Add(batch);
            }
            batchs.SpriteCount++;
        }
        else
        {
            var batch = GetBatch();
            batch.BeginDraw(texture);
            batch.DrawRectangle(destination, source, color);

            batchs = new Batch(1, batch); 

            _batchs.Add(texture, batchs);
        }
    }

    public void DrawRectangle(ITexture2D texture, Rectangle destination, Color color)
    {
        DrawRectangle(texture, destination, texture.Bounds, color);
    }

    public void EndDraw()
    {
        if (!Drawing)
        {
            throw new InvalidOperationException("SpriteBatch isn't drawing, can't call SpriteBatch.EndDraw() while not drawing.");
        }
        Drawing = false;

        // Clear any remaining pool members.
        var pool = new Queue<BatchLifetime>();
        foreach (var lifetime in _batchPool)
        {
            if (lifetime.Lifetime <= 0)
            {
                lifetime.Batch.Dispose();
            }
            else
            {
                lifetime.Lifetime--;
                pool.Enqueue(lifetime);
            }
        }
        _batchPool = pool;

        foreach (var texture in _batchs.Keys)
        {
            var batchs = _batchs[texture];
            foreach (var batch in batchs.Batchs)
            {
                batch.EndDraw(_shader, _mvp);
                _batchPool.Enqueue(new BatchLifetime(TextureBatchLifetime, batch));
            }
        }
        _batchs.Clear();
    }

    private ITextureBatch GetBatch()
    {
        if (_batchPool.Count > 0)
        {
            var b = _batchPool.Dequeue();
            return b.Batch;
        }
        return new TextureBatch(_maxSprites);
    }

    private class Batch
    {
        public Batch(int spriteCount, ITextureBatch batch)
        {
            SpriteCount = spriteCount;
            Batchs = new List<ITextureBatch>
            {
                batch
            };
        }

        public int SpriteCount;
        public List<ITextureBatch> Batchs;
    }

    private class BatchLifetime
    {
        public BatchLifetime(int lifetime, ITextureBatch batch)
        {
            Lifetime = lifetime;
            Batch = batch;
        }

        public int Lifetime;
        public ITextureBatch Batch;
    }

    private const string vert = @"
#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec4 aColor;
layout(location = 2) in vec2 aTexCoord;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 mvp;

void main(void)
{
    fColor = aColor;
    fTexCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0) * mvp;
}
";

    private const string frag = @"
#version 330

in vec4 fColor;
in vec2 fTexCoord;

out vec4 outputColor;

uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, fTexCoord) * fColor;
}
";
}