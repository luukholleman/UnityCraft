using System;
using Assets.Code.Items;

namespace Assets.Code.Blocks.Plants
{
    [Serializable]
    public class Wood : Block
    {
        public override Item GetItem()
        {
            return new Items.Plants.Wood();
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
}