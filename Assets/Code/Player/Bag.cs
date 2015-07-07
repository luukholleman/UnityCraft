using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GUI.Inventory;
using Assets.Code.Items;
using Assets.Code.Messenger;
using Assets.Code.StorageSystem;
using UnityEngine;

namespace Assets.Code.Player
{
    class Bag : MonoBehaviour
    {
        public static Bag Instance;

        private readonly Storage _storage = new Storage("inventory", 50);

        private int _selectedIndex;

        void Awake()
        {
            Instance = this;

            Postman.AddListener<Item>("picked up item", PickedUpItem);
        }

        void Start()
        {
            ChangeSelectedIndex(0);
        }

        void Update()
        {
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scroll > 0)
                ChangeSelectedIndex(_selectedIndex + 1);
            else if (scroll < 0)
                ChangeSelectedIndex(_selectedIndex - 1);

            for (int i = 0; i < 10; ++i)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    ChangeSelectedIndex(i - 1);
                }
            }
        }

        private void PickedUpItem(Item item)
        {
            _storage.AddItem(item);
        }

        public Item PopSelectedItem()
        {
            return _storage.PopItem(_selectedIndex);
        }

        public Item PeekSelectedItem()
        {
            return _storage.PeekItem(_selectedIndex);
        }

        public StorageSlot[] Items()
        {
            return _storage.Items;
        }

        private void ChangeSelectedIndex(int newIndex)
        {
            if (newIndex > 9)
                newIndex = 0;
            else if (newIndex < 0)
                newIndex = 9;

            Postman.Broadcast<int>("item selected", newIndex);

            _selectedIndex = newIndex;
        }
    }
}
