using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic.Behaviours;
using Assets.Code.WorldObjects.Dynamic.Defaults;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    class Flower : DynamicCross
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.x = 12;
            tile.y = 15;

            return tile;
        }
        
        public override Item GetItem()
        {
            return null;
        }

        public override BaseBehaviour GetBehaviour()
        {
            return null;
        }
    }
}
