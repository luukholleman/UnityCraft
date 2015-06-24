using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using Assets.Code.WorldObjects.Dynamic.Defaults;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    class Chest : DynamicBlock
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            switch (direction)
            {
                case Direction.Up:
                    tile.x = 9;
                    tile.y = 14;
                    break;
                case Direction.Down:
                    tile.x = 9;
                    tile.y = 14;
                    break;
                default:
                    tile.x = 10;
                    tile.y = 14;
                    break;
            }

            return tile;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        public override BaseStatemachine GetBehaviour()
        {
            return new ChestStatemachine();
        }

        public override Item GetItem()
        {
            return null;
        }
    }
}
