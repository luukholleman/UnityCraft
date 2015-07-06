using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Inventory;
using Assets.Code.Items;
using Assets.Code.Messenger;
using UnityEngine;

namespace Assets.Code.GUI
{
    class Inventory : MonoBehaviour
    {
        public GameObject IconBackground;

        public GameObject Content;

        void Awake()
        {
            Postman.AddListener<Storage>("inventory is opened", InventoryOpened);
            Postman.AddListener("inventory is closed", InventoryClosed);
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

            int i = 0;
            
            foreach (KeyValuePair<int, Item> item in inventory.Items)
            {
                GameObject background = Instantiate(IconBackground, Vector3.zero, new Quaternion()) as GameObject;

                if (background != null)
                {
                    background.transform.parent = Content.transform;

                    background.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                    background.transform.localPosition = new Vector3(-470 + (1040 / 10 * (i % 10)), 150 - (940 / 10 * (i / 10)), 0);

                    background.transform.FindChild("Label").GetComponent<UILabel>().text = item.Key.ToString();

                    if (item.Key > 0)
                    {
                        background.GetComponentInChildren<MeshFilter>().mesh = item.Value.GetMesh();
                    }
                }

                i++;
            }
        }
    }
}
