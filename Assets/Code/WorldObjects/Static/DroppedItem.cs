using Assets.Code.Items;
using Assets.Code.Messenger;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.WorldObjects.Static
{
    class DroppedItem : MonoBehaviour
    {
        public Position Position;

        public Item Item;

        public float SelfDestroyTime = 300;

        private GameObject _player;

        private bool _pickedUp;

        void Start()
        {
            MeshFilter filter = GetComponent<MeshFilter>();

            filter.mesh = Item.GetMesh();

            GetComponent<Rigidbody>().velocity = new Vector3(Random.value * 2 - 1, Random.value + 5, Random.value * 2 - 1);

            _player = GameObject.FindWithTag("Player");

            Destroy(this, SelfDestroyTime);
        }

        void Update()
        {
            if (!_pickedUp && Vector3.Distance(transform.position, _player.transform.position) < 1.5f)
            {
                Postman.Broadcast<Item>("picked up item", Item);

                Destroy(gameObject);

                _pickedUp = true;
            }
        }
    }
}
