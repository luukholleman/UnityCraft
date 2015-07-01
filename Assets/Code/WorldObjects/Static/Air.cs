using System;
using System.Collections.Generic;
using Assets.Code.GenerationEngine;
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

        public override MeshData GetChunkMeshData(ChunkData chunk, Position position, MeshData meshData)
        {
            return meshData;
        }

        public override List<KeyValuePair<Vector3, Vector3>> GetChunkCollider(ChunkData chunk, Position position, List<KeyValuePair<Vector3, Vector3>> colliders)
        {
            return colliders;
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