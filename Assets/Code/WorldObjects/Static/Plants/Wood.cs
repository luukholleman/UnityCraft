using System;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static.Plants
{
    [Serializable]
    public class Wood : StaticBlock
    {
        public override Item GetItem()
        {
            return new Items.Blocks.Wood();
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