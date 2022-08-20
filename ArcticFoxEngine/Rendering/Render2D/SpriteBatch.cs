using ArcticFoxEngine.Math;
using ArcticFoxEngine.Services.TextureManager;
using OpenTK.Mathematics;

namespace ArcticFoxEngine.Rendering.Render2D;

public class SpriteBatch : ISpriteBatch
{
    private const int _spriteBatchSize = 2000;

    private readonly Dictionary<ITexture2D, IList<ITextureBatch>> _textureBatchCache;
    private readonly Dictionary<ITexture2D, int> _spriteCountsPerTexture;

    private Matrix4 _mvp;

    // Need to hook into resize event.
    public SpriteBatch()
    {
        _textureBatchCache = new Dictionary<ITexture2D, IList<ITextureBatch>>();
        _spriteCountsPerTexture = new Dictionary<ITexture2D, int>();
    }

    public bool Drawing { get; private set; }

    public void BeginDraw(Matrix4 mvp)
    {
        if (Drawing)
        {
            throw new InvalidOperationException("Spritebatch is already drawing, can't call SpriteBatch.BeginDraw() while drawing.");
        }
        Drawing = true;
        _mvp = mvp;
        _spriteCountsPerTexture.Clear();
        foreach (var texture in _textureBatchCache.Keys)
        {
            _spriteCountsPerTexture.Add(texture, 0);

            foreach (var batch in _textureBatchCache[texture])
            {
                batch.BeginDraw(mvp);
            }
        }
    }

    public void DrawRectangle(ITexture2D texture, Rectangle destination, Color color)
    {
        DrawRectangle(texture, destination, texture.Bounds, color);
    }

    public void DrawRectangle(ITexture2D texture, Rectangle destination, Rectangle source, Color color)
    {
        if (!Drawing)
        {
            throw new InvalidOperationException("SpriteBatch isn't drawing, can't call SpriteBatch.DrawRectangle() while not drawing.");
        }
        int key = 0;
        if (_spriteCountsPerTexture.TryGetValue(texture, out int spriteCount))
        {
            key = spriteCount / _spriteBatchSize;
        }
        else
        {
            _spriteCountsPerTexture.Add(texture, 0);
            _textureBatchCache.Add(texture, new List<ITextureBatch>());
        }

        _spriteCountsPerTexture[texture]++;
        var cache = _textureBatchCache[texture];
        if (cache.Count > key)
        {
            cache[key].DrawRectangle(destination, source, color);
        }
        else
        {
            var batch = new TextureBatch(texture, _spriteBatchSize);
            batch.BeginDraw(_mvp);
            batch.DrawRectangle(destination, source, color);
            cache.Add(batch);
        }
    }

    public void EndDraw()
    {
        if (!Drawing)
        {
            throw new InvalidOperationException("SpriteBatch isn't drawing, can't call SpriteBatch.EndDraw() while not drawing.");
        }
        Drawing = false;
        foreach (var texture in _textureBatchCache.Keys)
        {
            var spriteCount = _spriteCountsPerTexture[texture];
            var count = spriteCount / _spriteBatchSize + 1;
            if (spriteCount % _spriteBatchSize == 0)
            {
                count--;
            }
            var cache = _textureBatchCache[texture];
            for (int i = 0; i < cache.Count; i++)
            {
                cache[i].EndDraw();
                if (i >= count)
                {
                    cache[i].Dispose();
                    cache.RemoveAt(i);
                }
            }
            if (count == 0)
            {
                _textureBatchCache.Remove(texture);
            }
        }
    }
}
