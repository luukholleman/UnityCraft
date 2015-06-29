using System;
using Assets.Code.Items;
using Assets.Code.Items.Usables;
using Assets.Code.WorldObjects.Static.Defaults;
using Random = UnityEngine.Random;

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
            if (Random.value*10 < 1)
            {
                return new Seeder();
            }

            return null;
        }
    }
}