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
	[Serializable]
    public abstract class WorldObject : IInteractable
    {
		[NonSerialized]
		public Chunk Chunk;
        public Position Position { get; set; }
        public enum Direction { North, East, South, West, Up, Down };

        public const float TileSize = 0.0625f;
        public struct Tile { public int x; public int y;}

        public bool Changed = true;
        public abstract MeshData GetMeshData();
        public abstract Tile TexturePosition(Direction direction);
        public abstract bool IsSolid(Direction direction);
        public abstract Item GetItem();
        public abstract void Action();
        public abstract void Interact();
    }
}
