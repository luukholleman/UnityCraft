﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static.Plants
{
    class Flower : StaticCross
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
    }
}