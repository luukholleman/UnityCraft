using UnityEngine;
using System.Collections;
using System;
using Assets.Code.World.Chunk.Block;

[Serializable]
public class BlockWood : Block
{

    public BlockWood()
        : base()
    {

    }

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