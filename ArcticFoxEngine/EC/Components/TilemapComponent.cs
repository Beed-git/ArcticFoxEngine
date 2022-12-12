using ArcticFoxEngine.EC.Models;
using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.EC.Components;

public class TilemapComponent : IComponentModel
{
    public Point MapSize { get; set; }
    public string SpriteSheet { get; set; }
    public int[][] TileIds { get; set; }
    public int[] Collisions { get; set; }

    public TilemapComponent()
    {
        MapSize = Point.Zero;
        SpriteSheet = "default";
        TileIds = Array.Empty<int[]>();
    }
}
