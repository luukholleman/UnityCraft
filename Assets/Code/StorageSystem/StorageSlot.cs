using Assets.Code.Items;
using Assets.Code.Messenger;

namespace Assets.Code.StorageSystem
{
    class StorageSlot
    {
        public Item Item { get; private set; }
        public int Count { get; private set; }

        public StorageSlot()
        {

        }
        
        public StorageSlot(int count, Item item)
        {
            SetItem(count, item);
        }

        public void SetItem(int count, Item item)
        {
            Item = item;
            Count = count;
            
            Postman.Broadcast<StorageSlot>("item changed", this);
        }

        public void Clear()
        {
            Count = 0;
            Item = null;
        }
    }
}
