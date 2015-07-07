using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Messenger;
using Assets.Code.Player;
using Assets.Code.StorageSystem;
using UnityEngine;

namespace Assets.Code.GUI.Inventory
{
    class ItemBar : MonoBehaviour
    {
        private int _selected;

        void Awake()
        {
            Postman.AddListener<int>("item selected", ItemSelected);
        }

        private void ItemSelected(int index)
        {
            transform.FindChild("Item" + _selected).GetComponent<StorageSlotBus>().Unselect();

            transform.FindChild("Item" + index).GetComponent<StorageSlotBus>().Select();

            _selected = index;
        }

        void Start()
        {
            int i = 0;

            foreach (StorageSlot storageSlot in Bag.Instance.Items().Take(10))
            {
                transform.FindChild("Item" + i).GetComponent<StorageSlotBus>().StorageSlot = storageSlot;

                i++;
            }
        }
    }
}
