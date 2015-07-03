using Assets.Code.Items;
using Assets.Code.WorldObjects.Dynamic.Plants.Statemachines;

namespace Assets.Code.WorldObjects.Dynamic.Plants
{
    class Wheat : BasePlant
    {
        public Wheat()
        {
            Statemachine = new WheatStatemachine();
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            tile.x = 8 + GrowLevel;
            tile.y = 10;

            return tile;
        }

        public override Item GetItem()
        {
            return null;
        }

    }
}
