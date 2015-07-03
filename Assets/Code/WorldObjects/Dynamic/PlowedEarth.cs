using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.Items.Blocks;
using Assets.Code.WorldObjects.Dynamic.Defaults;
using Assets.Code.WorldObjects.Dynamic.Statemachines;

namespace Assets.Code.WorldObjects.Dynamic
{
    class PlowedEarth : DynamicBlock
    {
        private bool _watered;

        public bool Watered
        {
            get { return _watered; }
            set { _watered = value; ((PlowedEarthStatemachine)Statemachine).SetWatered(value); }
        }

        public PlowedEarth()
        {
            Statemachine = new PlowedEarthStatemachine();
        }
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            switch (direction)
            {
                case Direction.Up:
                    tile.x = Watered ? 4 : 5;
                    tile.y = 10;
                    return tile;
            }

            tile.x = 2;
            tile.y = 15;

            return tile;
        }

        public override Item GetItem()
        {
            return new Earth();
        }
    }
}
