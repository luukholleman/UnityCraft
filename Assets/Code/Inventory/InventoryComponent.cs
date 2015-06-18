using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.Items.Blocks;
using Assets.Code.Messenger;
using UnityEngine;

namespace Assets.Code.Inventory
{
    class InventoryComponent : MonoBehaviour
    {
        public Inventory Inventory = new Inventory();

        private KeyValuePair<int, GameObject>[] ItemsToShow = new KeyValuePair<int, GameObject>[10];

        void Start()
        {
            Postman.AddListener<Item>("picked up item", PickedUpItem);
            Postman.AddListener<int, Item, int>("item added to inventory", ItemAddedToInventory);
            Postman.AddListener<int, Item, int>("item removed from inventory", ItemRemovedFromInventory);
            
            GameObject item = Resources.Load<GameObject>("Prefabs/UIItem");

            GameObject newItem = Instantiate(item, new Vector3(-5, -4.2f, 2), new Quaternion()) as GameObject;

            newItem.GetComponent<ItemComponent>().Item = new StoneBlock();

            newItem.GetComponent<Rigidbody>().AddTorque(new Vector3(10, 10, 10));
        }

        private void PickedUpItem(Item item)
        {
            Inventory.AddItem(item);
        }

        private void ItemAddedToInventory(int index, Item item, int count)
        {
            if (index >= 10)
                return;

            if (ItemsToShow[index].Value != new KeyValuePair<int, GameObject>().Value)
            {
                Destroy(ItemsToShow[index].Value);
            }

            GameObject uiitem = Resources.Load<GameObject>("Prefabs/UIItem");

            GameObject newItem = Instantiate(uiitem, new Vector3(0 - (index - 5), -4.2f, 2), new Quaternion()) as GameObject;

            newItem.GetComponent<ItemComponent>().Item = item;

            ItemsToShow[index] = new KeyValuePair<int, GameObject>(count, newItem);
        }

        private void ItemRemovedFromInventory(int index, Item item, int count)
        {
            if (index >= 10)
                return;

            Destroy(ItemsToShow[index].Value);
        }
    }
}
