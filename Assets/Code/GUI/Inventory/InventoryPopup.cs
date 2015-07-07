using System.Collections.Generic;
using Assets.Code.Items;
using Assets.Code.Messenger;
using Assets.Code.Player;
using Assets.Code.StorageSystem;
using UnityEngine;

namespace Assets.Code.GUI.Inventory
{
    class InventoryPopup : MonoBehaviour
    {
        public GameObject StorageSlotObject;

        public GameObject Content;

        private bool _open;

        void Start()
        {
            int i = 0;

            foreach (StorageSlot storageSlot in Bag.Instance.Items())
            {
                GameObject storageSlotObject = Instantiate(StorageSlotObject, Vector3.zero, new Quaternion()) as GameObject;

                if (storageSlotObject != null)
                {
                    storageSlotObject.transform.parent = Content.transform;
                    storageSlotObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    storageSlotObject.transform.localPosition = new Vector3(-470 + (1040 / 10 * (i % 10)), 150 - (940 / 10 * (i / 10)), 0);

                    storageSlotObject.GetComponent<StorageSlotBus>().StorageSlot = storageSlot;
                }

                i++;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!_open)
                {
                    GetComponent<UIWidget>().alpha = 1;
                }
                else
                {
                    GetComponent<UIWidget>().alpha = 0;
                }

                _open = !_open;
            }
        }

        private void InventoryClosed()
        {
            GetComponent<UIWidget>().alpha = 0;

            foreach (Transform transform in Content.transform)
            {
                Destroy(transform.gameObject);
            }
        }

        private void InventoryOpened(Storage inventory)
        {
            GetComponent<UIWidget>().alpha = 1;
        }
    }
}
