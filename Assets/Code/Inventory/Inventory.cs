using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.Messenger;
using UnityEngine;

namespace Assets.Code.Inventory
{
    class Inventory
    {
        public Item[][] Items;

        public Inventory()
        {
            Items = new Item[60][];

            for (int i = 0; i < Items.Count(); i++)
            {
                Items[i] = new Item[100];
            }
        }

        public bool AddItem(Item item)
        {
            int index = FindFirstEmptySpot(item);

            Debug.Log(index);

            if (index != -1)
            {
                for (int i = 0; i < Items[index].Count(); i++)
                {
                    if (Items[index][i] == null)
                    {
                        Items[index][i] = item;
                        Postman.Broadcast<int, Item, int>("item added to inventory", index, item, i);

                        return true;
                    }
                }
            }

            return false;
        }

        public int FindFirstEmptySpot(Item item)
        {
            for (int i = 0; i < Items.Count(); i++)
            {
                if (Items[i][0] == null)
                {
                    return i;
                }

                Debug.Log(Items[i][0].GetType() == item.GetType());

                if (Items[i][0].GetType() == item.GetType())
                {
                    if (Items[i].Count(it => it != null) < 100)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
