using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static.Plants
{
	[Serializable]
    class Grass : StaticCross
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.x = 1;
            tile.y = 13;

            return tile;
        }

        public override Item GetItem()
        {
            return null;
        }

    }
}
