using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static
{
    class Water : StaticBlock
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.x = 13;
            tile.y = 3;

            return tile;
        }

        public override Item GetItem()
        {
            return null;
        }
    }
}
