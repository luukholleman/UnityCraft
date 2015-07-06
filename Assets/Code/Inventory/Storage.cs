using System.Linq;
using Assets.Code.Items;
using Assets.Code.Messenger;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Inventory
{
    class Storage
    {
		public KeyValuePair<int, Item>[] Items;

		public object Id { get; private set; }

		public int Size { get; private set; }

        public Storage(object id, int size)
        {
			Id = id;

			Size = size;

			Items = new KeyValuePair<int, Item>[Size];

			for (int i = 0; i < Size; i++) 
			{
				Items[i] = new KeyValuePair<int, Item>(0, null);
			}
        }

        public bool AddItem(Item item)
        {
            int index = FindFirstEmptySpot(item);

            if (index != -1)
            {
				Items[index] = new KeyValuePair<int, Item>(Items[index].Key + 1, item);
				
				Postman.Broadcast<Storage, int, KeyValuePair<int, Item>>("item added to storage", this, index, Items[index]);
            }

            return false;
        }

        public int FindFirstEmptySpot(Item item)
		{
			for (int i = 0; i < Items.Count(); i++)
			{
				if(Items[i].Key == 0)
				{
					return i;
				}

				if(Items[i].Value.GetType() == item.GetType())
				{
					if (Items[i].Key < item.GetStackSize())
					{
						return i;
					}
				}
			}

            return -1;
        }

        public Item PopItem(int index)
        {	
			if (Items[index].Key > 0)
			{
				Item item = Items[index].Value;

				Items[index] = new KeyValuePair<int, Item>(Items[index].Key - 1, Items[index].Value);
				
				Postman.Broadcast<Storage, int, KeyValuePair<int, Item>>("item removed from storage", this, index, Items[index]);

				return item;
			}
			
			return null;
        }

		public bool MoveStack(int oldIndex, int newIndex)
		{
			if (Items [oldIndex].Key > 0 && Items [newIndex].Key == 0)
			{
				Items[newIndex] = Items[oldIndex];

				Items[oldIndex] = new KeyValuePair<int, Item>(0, null);

				return true;
			}

			return false;
		}

        public Item PeekItem(int index)
        {
            if (Items[index].Key > 0)
            {
				return Items[index].Value;
            }

            return null;
        }
    }
}
