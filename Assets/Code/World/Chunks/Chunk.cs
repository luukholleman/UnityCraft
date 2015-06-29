using System.Collections;
using System.Collections.Generic;
using Assets.Code.GenerationEngine;
using Assets.Code.Items;
using Assets.Code.Scheduler;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.World.Chunks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Chunk : MonoBehaviour, Interactable
    {
        private ChunkData _chunkData;

        public GameObject DynamicObjectPrefab = Resources.Load<GameObject>("Prefabs/DynamicObject");

        private bool _rebuild;

        private MeshFilter _filter;
        private MeshCollider _collider;

        public World World;

        public Position Position;

        private int _iterationsPerFrame = 100;

        private const int UpdateIterationsPerFrame = 4000;

        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _collider = gameObject.GetComponent<MeshCollider>();
            
            transform.name = "ChunkData " + Position;
        }

        IEnumerator PlaceDynamicObjects()
        {
            var dynamicObjects = _chunkData.GetDynamicObjects();
            int i = 0;

            foreach (KeyValuePair<Position, DynamicObject> worldObject in dynamicObjects)
            {
                if (Helper.InChunk(worldObject.Key))
                {
                    GameObject newDynamicObject = PoolManager.Spawn(DynamicObjectPrefab);

                    newDynamicObject.transform.position = Position.ToVector3() + new Vector3(worldObject.Key.x, worldObject.Key.y, worldObject.Key.z);

                    DynamicObjectComponent dOC = newDynamicObject.GetComponent<DynamicObjectComponent>();

                    dOC.DynamicObject = worldObject.Value;
                    dOC.Chunk = this;

                    yield return null;
                }
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
            MeshData meshdata = new MeshData();

            int i = 0;

            Dictionary<Position, StaticObject> blocks = _chunkData.GetStaticObjects();

            foreach (KeyValuePair<Position, StaticObject> block in blocks)
            {
                if (Helper.InChunk(block.Key))
                {
                    meshdata = block.Value.GetChunkMeshData(_chunkData, block.Key, meshdata);
                }

                if (++i % _iterationsPerFrame == 0)
                    yield return null;
            }

            Scheduler.Scheduler.Instance.Add(new BindMeshFilter() { MeshFilter = _filter, MeshData = meshdata });
            Scheduler.Scheduler.Instance.Add(new BindMeshCollider() { MeshCollider = _collider, MeshData = meshdata });

            _iterationsPerFrame = UpdateIterationsPerFrame;
        }

        void OnDestroy()
        {
            //Serialization.SaveChunk(this);
        }

        public WorldObject GetObject(Position position)
        {
            if (Helper.InChunk(position) && _chunkData.HasObjectAtPosition(position))
            {
                return _chunkData.GetObject(position);
            }

            return World.GetObject(Position + position);
        }

        public bool SetObject(Position position, WorldObject block, bool replace = false)
        {
            if (Helper.InChunk(position - Position))
            {
                _chunkData.SetObject(position, block, replace);

                DoRebuild();

                return true;
            }

            World.SetObject(position, block);

            return false;
        }
        
        public void SetBlocksUnmodified()
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

        public void Action(RaycastHit hit)
        {
            Position position = Helper.GetBlockPos(hit);

            WorldObject block = World.GetObject(position);

            Position pos = Helper.GetBlockPos(hit);

            SetObject(pos, new Air(), true);

            Item droppedItem = block.GetItem();

            if (droppedItem != null)
            {
                GameObject droppedItemGo = Instantiate(Resources.Load<GameObject>("Prefabs/Item"), pos.ToVector3(), new Quaternion()) as GameObject;

                droppedItemGo.GetComponent<DroppedItem>().Position = pos;
                droppedItemGo.GetComponent<DroppedItem>().Item = droppedItem;
            }
        }

        public void Interact()
        {
            
        }

        public void SetChunkData(ChunkData chunkData)
        {
            _chunkData = chunkData;
        }
    }
}