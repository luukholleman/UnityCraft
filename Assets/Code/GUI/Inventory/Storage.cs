using System.Linq;
using Assets.Code.Items;
using Assets.Code.Messenger;

namespace Assets.Code.GUI.Inventory
{
    class Storage
    {
        public Item[][] Items;

        public Storage()
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

        public Item PopItem(int index)
        {
            if (Items[index] != null)
            {
                for (int i = 99; i >= 0; i--)
                {
                    if (Items[index][i] != null)
                    {
                        Item item = Items[index][i];

                        Items[index][i] = null;

                        Postman.Broadcast<int, Item, int>("item removed from inventory", index, item, i);

                        return item;
                    }
                }
            }

            return null;
        }

        public Item PeekItem(int index)
        {
            if (Items[index] != null)
            {
                for (int i = 99; i >= 0; i--)
                {
                    if (Items[index][i] != null)
                    {
                        Item item = Items[index][i];

                        return item;
                    }
                }
            }

            return null;
        }
    }
}
