using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.Items.Blocks
{
    abstract class BaseBlock : Item
    {
        public override ItemType Type()
        {
            return ItemType.Block;
        }

        public abstract StaticObject GetBlock();

        public override int GetStackSize()
        {
            return 100;
        }

        public override bool AdjacentCast()
        {
            return true;
        }

        public override bool Interact(Position position, Interactable interactable)
        {
            Chunk chunk = interactable as Chunk;

            StaticObject staticObject = GetBlock();

            if (chunk != null && staticObject != null)
            {
                chunk.SetObject(position, staticObject, true);

                return true;
            }

            return false;
        }
    }
}
