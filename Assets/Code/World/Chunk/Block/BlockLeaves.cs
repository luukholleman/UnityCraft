using UnityEngine;
using System.Collections;
using System;
using Assets.Code.World.Chunk.Block;

[Serializable]
public class BlockLeaves : Block
{

    public BlockLeaves()
        : base()
    {

    }

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