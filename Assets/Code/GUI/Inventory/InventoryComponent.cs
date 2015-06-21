using System.Collections.Generic;
using System.Linq;
using Assets.Code.Items;
using Assets.Code.Messenger;
using UnityEngine;

namespace Assets.Code.GUI.Inventory
{
    class InventoryComponent : MonoBehaviour
    {
        public Inventory Inventory = new Inventory();

        private readonly KeyValuePair<int, GameObject>[] ItemBar = new KeyValuePair<int, GameObject>[10];

        public Camera GUICamera;

        private float _height;
        private float _width;

        private int _selectedIndex = 0;

        void Start()
        {
            Postman.AddListener<Item>("picked up item", PickedUpItem);
            Postman.AddListener<int, Item, int>("item added to inventory", ItemAddedToInventory);
            Postman.AddListener<int, Item, int>("item removed from inventory", ItemRemovedFromInventory);

            _height = 2f * GUICamera.orthographicSize;
            _width = _height * GUICamera.aspect;
        }

        private void PickedUpItem(Item item)
        {
            Inventory.AddItem(item);
        }

        public Item PopSelectedItem()
        {
            return Inventory.PopItem(_selectedIndex);
        }

        private void ItemAddedToInventory(int index, Item item, int count)
        {
            if (index >= 10)
                return;

            if (ItemBar[index].Value != new KeyValuePair<int, GameObject>().Value)
            {
                Destroy(ItemBar[index].Value);
            }

            GameObject uiitem = Resources.Load<GameObject>("Prefabs/UIItem");

            GameObject newItem = Instantiate(uiitem, GetWorldBarPosition(index), new Quaternion()) as GameObject;

            newItem.GetComponent<ItemComponent>().Item = item;

            ItemBar[index] = new KeyValuePair<int, GameObject>(count, newItem);

            ChangeSelectedIndex(_selectedIndex);
        }

        private void Update()
        {
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");


            if (scroll > 0)
                ChangeSelectedIndex(_selectedIndex + 1);
            else if(scroll < 0)
                ChangeSelectedIndex(_selectedIndex - 1);

            for (int i = 0; i < 10; ++i)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    ChangeSelectedIndex(i - 1);
                }
            }
        }

        private void ChangeSelectedIndex(int newIndex)
        {
            if (newIndex > 9)
                newIndex = 0;
            else if (newIndex < 0)
                newIndex = 9;

            if (!Equals(ItemBar[_selectedIndex], default(KeyValuePair<int, GameObject>)))
            {
                ItemBar[_selectedIndex].Value.GetComponent<ItemComponent>().StopPulsing();
            }

            if (!Equals(ItemBar[newIndex], default(KeyValuePair<int, GameObject>)))
            {
                ItemBar[newIndex].Value.GetComponent<ItemComponent>().StartPulsing();
            }

            _selectedIndex = newIndex;
        }

        void OnGUI()
        {
            int i = 0;
            foreach (Item[] item in Inventory.Items.Take(10))
            {
                Vector3 point = GUICamera.WorldToScreenPoint(GetWorldBarPosition(i));

                UnityEngine.GUI.Label(GetGUIBarPosition(i, 200, 200), item.Count(it => it != default(Item)).ToString());

                i++;
            }
        }

        private Vector3 GetWorldBarPosition(int index)
        {
            return new Vector3(0 + (index - 5), -4.2f, 2);
        }

        private Rect GetGUIBarPosition(int index, int width, int height)
        {
            return new Rect((index - 5) * (Screen.width / _width) + Screen.width / 2, Screen.height - 40, width, height);
        }

        private void ItemRemovedFromInventory(int index, Item item, int count)
        {
            if (index >= 10 || count > 0)
                return;

            Destroy(ItemBar[index].Value);
        }
    }
}
