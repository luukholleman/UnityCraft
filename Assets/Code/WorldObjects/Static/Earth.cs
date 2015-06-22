using System;
using Assets.Code.Items;

namespace Assets.Code.WorldObjects.Static
{
    [Serializable]
    public class Earth : StaticObject
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            switch (direction)
            {
                case Direction.Up:
                    tile.x = 0;
                    tile.y = 15;
                    return tile;
                case Direction.Down:
                    tile.x = 2;
                    tile.y = 15;
                    return tile;
            }

            tile.x = 3;
            tile.y = 15;

            return tile;
        }

        public override Item GetItem()
        {
            return new Items.Blocks.Earth();
        }
    }
}