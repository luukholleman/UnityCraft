using System;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects.Static
{
    [Serializable]
    public class Air : StaticObject
    {
        public override Item GetItem()
        {
            return null;
        }

        public override MeshData GetChunkMeshData(ChunkComponent chunk, Position position, MeshData meshData)
        {
            return meshData;
        }

        public override MeshData GetMeshData()
        {
            return new MeshData();
        }

        public override Tile TexturePosition(Direction direction)
        {
            throw new NotImplementedException();
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}