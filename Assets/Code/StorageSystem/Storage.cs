using System.Linq;
using Assets.Code.Items;

namespace Assets.Code.StorageSystem
{
    class Storage
    {
		public readonly StorageSlot[] Items;

		public object Id { get; private set; }

		public int Size { get; private set; }

        public Storage(object id, int size)
        {
			Id = id;

			Size = size;

            Items = new StorageSlot[Size];

			for (int i = 0; i < Size; i++) 
			{
                Items[i] = new StorageSlot();
			}
        }

        public bool AddItem(Item item)
        {
            int index = FindFirstEmptySpot(item);

            if (index != -1)
            {
				Items[index].SetItem(Items[index].Count + 1, item);
            }

            return false;
        }

        public int FindFirstEmptySpot(Item item)
		{
			for (int i = 0; i < Items.Count(); i++)
			{
				if(Items[i].Count == 0)
				{
					return i;
				}

				if(Items[i].Item.GetType() == item.GetType())
				{
					if (Items[i].Count < item.GetStackSize())
					{
						return i;
					}
				}
			}

            return -1;
        }

        public Item PopItem(int index)
        {	
			if (Items[index].Count > 0)
			{
				Item item = Items[index].Item;

				Items[index].SetItem(Items[index].Count - 1, Items[index].Item);

				return item;
			}
			
			return null;
        }

		public bool Move(int oldIndex, int newIndex)
		{
			if (Items [oldIndex].Count > 0 && Items [newIndex].Count == 0)
			{
                // keep references
				Items[newIndex].SetItem(Items[oldIndex].Count, Items[oldIndex].Item);

				Items[oldIndex].Clear();

				return true;
			}

			return false;
		}

        public Item PeekItem(int index)
        {
            if (Items[index].Count > 0)
            {
				return Items[index].Item;
            }

            return null;
        }
    }
}
