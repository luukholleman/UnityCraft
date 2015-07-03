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
                HandleLeftClick();
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            RaycastHit hit;

            Item item = Inventory.PeekSelectedItem();

            if (item != null)
            {
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100, LayerMask.GetMask("Chunks", "DynamicObjects")))
                {
                    Position position = Helper.GetBlockPos(hit, item.AdjacentCast());

                    Interactable interactable = Helper.GetMonoBehaviour(hit) as Interactable;
                    
                    if (interactable != null)
                    {
                        bool interacted = item.Interact(position, interactable);

                        interactable.Interact();

                        if (item.DestroyOnUse() && interacted)
                        {
                            Inventory.PopSelectedItem();
                        }

                        if (interacted)
                        {
                            interactable.DoRebuild();
                        }
                    }
                }
            }
        }

        private void HandleLeftClick()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100, LayerMask.GetMask("Chunks", "DynamicObjects")))
            {
                if (hit.collider.CompareTag("Chunk"))
                {
                    Chunk chunk = Helper.GetMonoBehaviour(hit) as Chunk;

                    chunk.Action(hit);
                }
                else if (hit.collider.CompareTag("DynamicObject"))
                {
                    DynamicObjectComponent component = Helper.GetMonoBehaviour(hit) as DynamicObjectComponent;

                    component.Action();
                }
            }
        }
    }
}
