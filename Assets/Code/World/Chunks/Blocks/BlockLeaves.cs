using System;
using Assets.Code.World.Chunks.Blocks;

[Serializable]
public class BlockLeaves : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 4;
        tile.y = 12;

        return tile;
    }

    public override bool IsSolid(Direction direction)
    {
        return false;
    }
}