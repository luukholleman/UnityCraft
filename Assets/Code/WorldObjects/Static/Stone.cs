using System;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static.Defaults;

namespace Assets.Code.WorldObjects.Static
{
	[Serializable]
    class Stone : StaticBlock
    {
        public override Item GetItem()
        {
            return new Items.Blocks.Stone();
        }
        
        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 1;
            tile.y = 1;

            return tile;
        }
    }
}
