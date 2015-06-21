using System;
using Assets.Code.Items;
using Assets.Code.World.Chunks;

namespace Assets.Code.Blocks
{
    [Serializable]
    public class Air : Block
    {
        public override MeshData Blockdata
            (Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            return meshData;
        }

        public override Item GetItem()
        {
            return null;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}