using Assets.Code.GUI.Inventory;
using Assets.Code.Items;
using Assets.Code.Messenger;
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
        private Inventory Inventory;

        void Start()
        {
            Inventory = Inventory.Instance;
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

                    Postman.Broadcast<Position, Item>("interact with worldobject", position, item);

                    //IInteractable interactable = Helper.GetMonoBehaviour(hit) as IInteractable;
                    
                    //if (interactable != null)
                    //{
                    //    bool interacted = item.Interact(position, interactable);

                    //    interactable.Interact();

                    //    if (item.DestroyOnUse() && interacted)
                    //    {
                    //        Storage.PopSelectedItem();
                    //    }

                    //    if (interacted)
                    //    {
                    //        interactable.DoRebuild();
                    //    }
                    //}
                }
            }
        }

        private void HandleLeftClick()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 100, LayerMask.GetMask("Chunks", "DynamicObjects")))
            {
                Postman.Broadcast<Position>("action with worldobject", Helper.GetBlockPos(hit));
            }
        }
    }
}
