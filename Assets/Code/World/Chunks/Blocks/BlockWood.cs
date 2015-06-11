using System;
using Assets.Code.World.Chunks.Blocks;

[Serializable]
public class BlockWood : Block
{
    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.Up:
                tile.x = 5;
                tile.y = 14;
                return tile;
            case Direction.Down:
                tile.x = 5;
                tile.y = 14;
                return tile;
        }

        tile.x = 4;
        tile.y = 14;

        return tile;
    }
}