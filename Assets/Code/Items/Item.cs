using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.Items
{
    public abstract class Item
    {
        public enum ItemType
        {
            Block,
            Tool,
            Usable
        }

        public const float TileSize = 0.0625f;

        public abstract Mesh GetMesh();

        public abstract bool Interact(Position position, IInteractable interactable);

        public abstract ItemType Type();

        public abstract bool DestroyOnUse();

        public abstract int GetStackSize();

        public abstract bool AdjacentCast();
    }
}
