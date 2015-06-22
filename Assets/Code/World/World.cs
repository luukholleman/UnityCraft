using System.Collections.Generic;
using System.Linq;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.World
{
    public class World : MonoBehaviour
    {
        public Dictionary<Position, ChunkComponent> Chunks = new Dictionary<Position, ChunkComponent>();

        public GameObject ChunkPrefab;

        public static Generator Generator;

        public const int ViewingRange = 16*20;

        public const int LevelHeight = 4;

        public string WorldName = "world";

        public static int ChunkSize = 16;

        public bool CreateNewChunkPrefab(Position position)
        {
            if (Chunks.ContainsKey(position))
                return false;

            GameObject newChunkObject = PoolManager.Spawn(ChunkPrefab);

            if (newChunkObject != null)
            {
                newChunkObject.transform.position = position.ToVector3();

                ChunkComponent newChunkComponent = newChunkObject.GetComponent<ChunkComponent>();

                newChunkComponent.Position = position;
                newChunkComponent.World = this;

                newChunkComponent.Blocks = Generator.GetChunk(position).Objects;

                //foreach (KeyValuePair<Position, StaticObject> block in Generator.GetChunk(position).BeyondChunkBlocks)
                //{
                //    newChunkComponent.SetObject(block.Key, block.Value);
                //}

                newChunkComponent.SetBlocksUnmodified();

                //Serialization.Load(newChunkComponent);

                newChunkComponent.Rebuild = true;

                Chunks.Add(position, newChunkComponent);
            }

            return true;
        }
        
        void Start()
        {
            Generator = new Generator();
            Generator.Start();
        }

        void Update()
        {
            //Debug.Log("----");
            //Debug.Log(Generator.ChunkGenerators.Count);
            //Debug.Log(Generator.ChunkGenerators.Count(g => g.IsDone));
        }

        void OnDestroy()
        {
            Generator.Abort();
        }
        
        public ChunkComponent GetChunk(Position position, bool alreadyAbsolute = false)
        {
            Position absolutePosition;

            if (!alreadyAbsolute)
            {
                absolutePosition = new Position();

                absolutePosition.x = Mathf.FloorToInt(position.x / (float)ChunkSize) * ChunkSize;
                absolutePosition.y = Mathf.FloorToInt(position.y / (float)ChunkSize) * ChunkSize;
                absolutePosition.z = Mathf.FloorToInt(position.z / (float)ChunkSize) * ChunkSize;
            }
            else
            {
                absolutePosition = position;
            }

            ChunkComponent containerChunkComponent;

            Chunks.TryGetValue(absolutePosition, out containerChunkComponent);

            return containerChunkComponent;
        }

        public WorldObject GetObject(Position position)
        {
            ChunkComponent containerChunkComponent = GetChunk(position);

            if (containerChunkComponent != null)
            {
                WorldObject block = containerChunkComponent.GetObject(
                    position.x - containerChunkComponent.Position.x,
                    position.y - containerChunkComponent.Position.y,
                    position.z - containerChunkComponent.Position.z);

                return block;
            }
            
            return new Air();
        }

        public void SetObject(Position position, WorldObject @object)
        {
            ChunkComponent chunkComponent = GetChunk(position);
            
            if (chunkComponent != null)
            {
                if (chunkComponent.SetObject(position, @object))
                {
                    chunkComponent.Rebuild = true;
                }

                UpdateIfEqual(position.x - chunkComponent.Position.x, 0, new Position(position.x - 1, position.y, position.z));
                UpdateIfEqual(position.x - chunkComponent.Position.x, ChunkSize - 1, new Position(position.x + 1, position.y, position.z));
                UpdateIfEqual(position.y - chunkComponent.Position.y, 0, new Position(position.x, position.y - 1, position.z));
                UpdateIfEqual(position.y - chunkComponent.Position.y, ChunkSize - 1, new Position(position.x, position.y + 1, position.z));
                UpdateIfEqual(position.z - chunkComponent.Position.z, 0, new Position(position.x, position.y, position.z - 1));
                UpdateIfEqual(position.z - chunkComponent.Position.z, ChunkSize - 1, new Position(position.x, position.y, position.z + 1));
            }
        }

        public void DestroyChunk(Position position)
        {
            ChunkComponent chunkComponent = GetChunk(position);

            if (chunkComponent != null)
            {
                chunkComponent.GetComponent<MeshFilter>().mesh = new Mesh();
                PoolManager.Despawn(chunkComponent.gameObject);
                Chunks.Remove(position);
            }
        }

        void UpdateIfEqual(int value1, int value2, Position position)
        {
            if (value1 == value2)
            {
                ChunkComponent chunkComponent = GetChunk(position);

                if (chunkComponent != null)
                    chunkComponent.Rebuild = true;
            }
        }
    }
}
