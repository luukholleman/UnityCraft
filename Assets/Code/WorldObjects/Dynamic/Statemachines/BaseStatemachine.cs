using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Code.WorldObjects.Dynamic.Statemachines
{
    public abstract class BaseStatemachine
    {
        protected Chunk Chunk { get; private set; }
        protected DynamicObjectComponent DynamicObjectComponent { get; private set; }

        protected DynamicObject DynamicObject { get; private set; }

        protected GameObject GameObject { get; private set; }

        public void Setup(DynamicObjectComponent dynamicObjectComponent)
        {
            DynamicObjectComponent = dynamicObjectComponent;

            DynamicObject = DynamicObjectComponent.DynamicObject;

            GameObject = DynamicObjectComponent.gameObject;

            Chunk = DynamicObjectComponent.Chunk;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void Interact();
        public abstract void Action();
        public abstract void Destroy();

        public void DropItem()
        {
            DynamicObjectComponent.Chunk.RemoveObject(new Position(DynamicObjectComponent.transform.position));

            Object.Destroy(DynamicObjectComponent.gameObject);

            Item droppedItem = DynamicObjectComponent.DynamicObject.GetItem();

            if (droppedItem != null)
            {
                GameObject droppedItemGo = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Item"), DynamicObjectComponent.transform.position, new Quaternion()) as GameObject;

                if (droppedItemGo != null)
                {
                    droppedItemGo.GetComponent<DroppedItem>().Position = new Position(DynamicObjectComponent.transform.position);
                    droppedItemGo.GetComponent<DroppedItem>().Item = droppedItem;
                }
            }
        }
    }
}
