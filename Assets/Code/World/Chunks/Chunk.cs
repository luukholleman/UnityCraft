using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Assets.Code.GenerationEngine;
using Assets.Code.GUI.Inventory;
using Assets.Code.Items;
using Assets.Code.Messenger;
using Assets.Code.Thread;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Code.World.Chunks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class Chunk : MonoBehaviour
    {
        private ChunkData _chunkData;

        public GameObject DynamicObjectPrefab;

        private bool _rebuild;

        private MeshFilter _filter;
        private MeshCollider _collider;

        private World _world;

        public Position Position;

        private readonly List<KeyValuePair<Position, GameObject>> _dynamicGos = new List<KeyValuePair<Position, GameObject>>(); 

        void Start()
        {
            _world = World.Instance;

            _filter = gameObject.GetComponent<MeshFilter>();
            _collider = gameObject.GetComponent<MeshCollider>();

            transform.name = "ChunkData " + Position;

            Postman.AddListener<Position>("action with worldobject", WorldObjectAction);
            Postman.AddListener<Position, Item>("interact with worldobject", WorldObjectInteract);
        }

        private void WorldObjectAction(Position position)
        {
            Position localPosition = new Position(position - Position);

            if (Helper.InChunk(localPosition))
            {
                IInteractable interactable = GetObject(localPosition);

                if (interactable != null)
                {
                    interactable.Action();

                    DoRebuild();
                }
            }
        }
        private void WorldObjectInteract(Position position, Item item)
        {
            if (Helper.InChunk(position - Position))
            {
                IInteractable interactable = GetObject(position - Position);

                if (interactable != null)
                {
                    bool interacted = item.Interact(position, interactable);

                    interactable.Interact();

                    if (item.DestroyOnUse() && interacted)
                    {
                        Inventory.Instance.PopSelectedItem();
                    }

                    if (interacted)
                    {
                        DoRebuild();
                    }
                }
            }
        }

        IEnumerator PlaceDynamicObjects()
        {
            var dynamicObjects = _chunkData.GetDynamicObjects();

            foreach (KeyValuePair<Position, DynamicObject> worldObject in dynamicObjects)
            {
                DynamicObjectComponent dynamicObjectComponent;

                if (Helper.InChunk(worldObject.Key) && !_dynamicGos.Any(o => o.Key == worldObject.Key))
                {
                    GameObject newDynamicObject = PoolManager.Spawn(DynamicObjectPrefab);

                    newDynamicObject.transform.position = Position.ToVector3() +
                                                          new Vector3(worldObject.Key.x, worldObject.Key.y,
                                                              worldObject.Key.z);

                    dynamicObjectComponent = newDynamicObject.GetComponent<DynamicObjectComponent>();

                    dynamicObjectComponent.Position = worldObject.Key;
                    dynamicObjectComponent.DynamicObject = worldObject.Value;
                    dynamicObjectComponent.Chunk = this;

                    _dynamicGos.Add(new KeyValuePair<Position, GameObject>(worldObject.Key, newDynamicObject));
                }
                else
                {
                    dynamicObjectComponent = _dynamicGos.Find(o => o.Key == worldObject.Key).Value.GetComponent<DynamicObjectComponent>();
                }

                dynamicObjectComponent.BuildMesh();

                yield return null;
            }
        }
        
        void Update()
        {
            if (_rebuild)
            {
                _rebuild = false;

                StartCoroutine("GenerateMesh");
                StartCoroutine("PlaceDynamicObjects");
            }
        }

        public void DoRebuild()
        {
            StopCoroutine("GenerateMesh");

            StopCoroutine("PlaceDynamicObjects");

            _rebuild = true;
        }

        IEnumerator GenerateMesh()
        {
            GenerateChunkMesh gcm = new GenerateChunkMesh(_chunkData, _filter, _collider);

            ThreadPool.QueueUserWorkItem(gcm.Execute);

            //Tasker.Tasker.Instance.Add(new GenerateMesh() {_chunkData = _chunkData, _filter = _filter, _collider = _collider});

            yield break;
        }

        void OnDestroy()
        {
            //Serialization.SaveChunk(this);

            foreach (KeyValuePair<Position, GameObject> dynamicGo in _dynamicGos)
            {
                Destroy(dynamicGo.Value);
            }
        }

        public WorldObject GetObject(Position position)
        {
            if (Helper.InChunk(position))
            {
                return _chunkData.GetObject(position);
            }

            return _world.GetObject(Position + position);
        }

        public bool SetObject(Position position, WorldObject block, bool replace = false)
        {
            if (Helper.InChunk(position - Position))
            {
                if (_chunkData.SetObject(position, block, replace))
                {
                    block.Position = position;
                }
                
                DoRebuild();

                return true;
            }

            _world.SetObject(position, block);

            return false;
        }
        
        public void InitializeBlocks()
        {
            _chunkData.SetBlocksUnmodified();
        }
        public bool HasObjectAtPosition(Position position)
        {
            return _chunkData.HasObjectAtPosition(position);
        }
        public void RemoveObject(Position position)
        {
            _chunkData.RemoveObject(position);
        }

        public void DestroyBlock(Position position)
        {
            WorldObject block = _world.GetObject(position);

            SetObject(position, new Air(), true);

            Item droppedItem = block.GetItem();

            if (droppedItem != null)
            {
                GameObject droppedItemGo = Instantiate(Resources.Load<GameObject>("Prefabs/Item"), position.ToVector3(), new Quaternion()) as GameObject;

                droppedItemGo.GetComponent<DroppedItem>().Position = position;
                droppedItemGo.GetComponent<DroppedItem>().Item = droppedItem;
            }
        }
        
        public void SetChunkData(ChunkData chunkData)
        {
            _chunkData = chunkData;
        }
    }
}