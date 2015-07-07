using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.Messenger;
using Assets.Code.StorageSystem;
using UnityEngine;

namespace Assets.Code.GUI.Inventory
{
    class StorageSlotBus : MonoBehaviour
    {
        private UITexture _itemTexture;

        public StorageSlot StorageSlot;

        public int SelectedWidth;
        public int SelectedHeight;

        private int origWidth;
        private int origHeight;

        private bool _pulsing;

        private float _pulseStart;

        void Awake()
        {
            _itemTexture = transform.FindChild("ItemTexture").GetComponent<UITexture>();

            origWidth = _itemTexture.width;
            origHeight = _itemTexture.height;
        }

        void Start()
        {
            Postman.AddListener<StorageSlot>("item changed", ItemChanged);
        }

        private void ItemChanged(StorageSlot storageSlot)
        {
            if (Equals(StorageSlot, storageSlot))
            {
                GetComponentInChildren<UILabel>().text = "";

                GetComponentInChildren<UITexture>().uvRect = new Rect(new Vector2(0, 0), new Vector2(Item.TileSize, Item.TileSize));

                if (storageSlot.Count > 0)
                {
                    GetComponentInChildren<UILabel>().text = storageSlot.Count.ToString();

                    Tile tile = storageSlot.Item.TexturePosition();

                    GetComponentInChildren<UITexture>().uvRect = new Rect(new Vector2(Item.TileSize*tile.x, Item.TileSize*tile.y), new Vector2(Item.TileSize, Item.TileSize));
                }
            }
        }

        void Update()
        {
            if (_pulsing)
            {
                int width = (int)(origWidth + ((float)((SelectedWidth - origWidth) * (1 + Math.Sin(2 * Math.PI * Time.time - _pulseStart))) - (origWidth - SelectedWidth) / 2f));
                int height = (int)(origHeight + ((float)((SelectedHeight - origHeight) * (1 + Math.Sin(2 * Math.PI * Time.time - _pulseStart))) - (origHeight - SelectedHeight) / 2f));

                _itemTexture.width = width;
                _itemTexture.height = height;
            }
        }

        public void Select()
        {
            _pulsing = true;

            _pulseStart = Time.time;
        }

        public void Unselect()
        {
            _pulsing = false;

            _itemTexture.width = origWidth;
            _itemTexture.height = origHeight;
        }
    }
}
