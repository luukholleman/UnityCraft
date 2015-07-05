using System;
using System.Collections.Generic;
using Assets.Code.GenerationEngine;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects.Static
{
    [Serializable]
    public abstract class StaticObject : WorldObject
    {
        public abstract MeshData GetChunkMeshData(ChunkData chunk, Position position, MeshData meshData);

        public override void Interact()
        {

        }

        public override void Action()
        {
            Chunk.DestroyBlock(Position);
        }
    }
}