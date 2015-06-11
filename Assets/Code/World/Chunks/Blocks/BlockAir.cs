using System;

namespace Assets.Code.World.Chunks.Blocks
{
    [Serializable]
    public class BlockAir : Block
    {
        public override MeshData Blockdata
            (Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            return meshData;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}