using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Dynamic.Defaults;
using Assets.Code.WorldObjects.Dynamic.Statemachines;

namespace Assets.Code.WorldObjects.Dynamic
{
    class TreeGrower : DynamicPlate
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.x = 6;
            tile.y = 10;

            return tile;
        }

        public override Item GetItem()
        {
            throw new NotImplementedException();
        }

        public override BaseStatemachine GetBehaviour()
        {
            return new TreeGrowerStateMachine();
        }
    }
}
