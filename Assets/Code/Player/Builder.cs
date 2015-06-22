using Assets.Code.GUI.Inventory;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using UnityEngine;
using UnityEngineInternal;

namespace Assets.Code.Player
{
    public class Builder : MonoBehaviour
    {
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

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100, LayerMask.GetMask("Chunks", "DynamicObjects")))
                {
                    if (hit.collider.CompareTag("Chunk"))
                    {
                        ChunkComponent chunk = Helper.GetMonoBehaviour(hit) as ChunkComponent;

                        chunk.Action(hit);
                    }
                    else if (hit.collider.CompareTag("DynamicObject"))
                    {
                        DynamicObjectComponent component = Helper.GetMonoBehaviour(hit) as DynamicObjectComponent;

                        component.Action();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100))
                {
                    if (hit.collider.CompareTag("Chunk"))
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
                    else if(hit.collider.CompareTag("DynamicObject"))
                    {
                        DynamicObjectComponent component = Helper.GetMonoBehaviour(hit) as DynamicObjectComponent;

                        component.Interact();
                    }
                }
            }

        }
    }
}
