using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
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

        public override bool Interact(Position position, IInteractable interactable)
        {
			WorldObject worldObject = interactable as WorldObject;

            StaticObject staticObject = GetBlock();

			if (worldObject != null && staticObject != null)
            {
                World.World.Instance.SetObject(position, staticObject, true);

                return true;
            }

            return false;
        }
    }
}
