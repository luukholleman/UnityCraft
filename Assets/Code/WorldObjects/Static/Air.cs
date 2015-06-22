using System;
using Assets.Code.Items;
using Assets.Code.World.Chunks;

namespace Assets.Code.WorldObjects.Static
{
    [Serializable]
    public class Air : StaticObject
    {
        public override MeshData GetMeshData
            (ChunkComponent chunkComponent, int x, int y, int z, MeshData meshData)
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