﻿using System;
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
        
        public override MeshData GetMeshData()
        {
            return new MeshData();
        }

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile();
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}