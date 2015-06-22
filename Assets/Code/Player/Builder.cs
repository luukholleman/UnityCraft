using Assets.Code.GUI.Inventory;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.Player
{
    public class Builder : MonoBehaviour
    {
        Vector2 rot;

        private InventoryComponent Inventory;

        void Start()
        {
            Inventory = GameObject.Find("Inventory").GetComponent<InventoryComponent>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100, LayerMask.GetMask("Chunks")))
                {
                    WorldObject block = Helper.GetObject(hit);
                    Position pos = Helper.GetBlockPos(hit);

                    Helper.SetBlock(hit, new Air());

                    Item droppedItem = block.GetItem();

                    if (droppedItem != null)
                    {
                        GameObject droppedItemGo = Instantiate(Resources.Load<GameObject>("Prefabs/Item"), pos.ToVector3(), new Quaternion()) as GameObject;

                        droppedItemGo.GetComponent<DroppedItem>().Position = pos;
                        droppedItemGo.GetComponent<DroppedItem>().Item = droppedItem;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100))
                {
                    Item item = Inventory.PopSelectedItem();

                    if (item != null)
                    {
                        StaticObject staticObject = item.GetBlock();

                        if (staticObject != null)
                        {
                            Helper.SetBlock(hit, staticObject, true);
                        }
                    }
                }
            }

        }
    }
}
