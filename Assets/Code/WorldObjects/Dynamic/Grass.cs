using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using Assets.Code.WorldObjects.Dynamic.Defaults;

namespace Assets.Code.WorldObjects.Dynamic
{
    class Grass : DynamicCross
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.y = 10;
            tile.x = 15;

            return tile;
        }

        public override Item GetItem()
        {
            return null;
        }

        public override BaseStatemachine GetBehaviour()
        {
            return new GrassBaseStatemachine();
        }
    }
}
