using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects
{
    public abstract class WorldObject
    {
        public enum Direction { North, East, South, West, Up, Down };

        public const float TileSize = 0.0625f;

        public struct Tile { public int x; public int y;}

        public bool Changed = true;

        public abstract MeshData GetMeshData(ChunkComponent chunkComponent, int x, int y, int z, MeshData meshData);
        public abstract MeshData PropData(MeshData meshData);
        public abstract Tile TexturePosition(Direction direction);
        public abstract Vector2[] FaceUVs(Direction direction);

        public abstract bool IsSolid(Direction direction);

        public abstract Item GetItem();
    }
}
