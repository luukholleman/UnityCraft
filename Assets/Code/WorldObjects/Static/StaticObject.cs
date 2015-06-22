using System;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects.Static
{
    [Serializable]
    public abstract class StaticObject : WorldObject
    {
        public abstract MeshData GetChunkMeshData(ChunkComponent chunk, Position position, MeshData meshData);
    }
}