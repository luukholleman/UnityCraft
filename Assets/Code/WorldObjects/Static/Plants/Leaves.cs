using System;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static.Plants
{
    [Serializable]
    public class Leaves : StaticBlock
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