using System;
using Assets.Code.Items;

namespace Assets.Code.Blocks.Plants
{
    [Serializable]
    public class Leaves : Block
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

        public override Item GetItem()
        {
            return null;
        }
    }
}