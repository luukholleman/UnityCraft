﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static.Plants
{
    class Grass : StaticCross
    {
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.y = 10;
            tile.x = new Random().Next(8, 12);

            return tile;
        }

        public override Item GetItem()
        {
            return null;
        }

    }
}
