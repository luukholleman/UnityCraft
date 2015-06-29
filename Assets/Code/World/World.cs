using System.Collections.Generic;
using System.Linq;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using Frankfort.Threading;
using UnityEngine;

namespace Assets.Code.World
{
    public class World : MonoBehaviour
    {
        public Dictionary<Position, Chunk> Chunks = new Dictionary<Position, Chunk>();

        public GameObject ChunkPrefab;

        public static Generator Generator;
        
        public bool CreateNewChunk(KeyValuePair<Position, ChunkData> chunk)
        {
            if (Chunks.ContainsKey(chunk.Key))
                return false;

            GameObject newChunkObject = PoolManager.Spawn(ChunkPrefab);

            if (newChunkObject != null)
            {
                newChunkObject.transform.position = chunk.Key.ToVector3();

                Chunk newChunk = newChunkObject.GetComponent<Chunk>();

                newChunk.Position = chunk.Key;
                newChunk.World = this;

                newChunk.SetChunkData(chunk.Value);

                newChunk.SetBlocksUnmodified();
                
                newChunk.DoRebuild();

                Chunks.Add(chunk.Key, newChunk);
            }

            return true;
        }
        
        void Awake()
        {
            Generator = new Generator();
            Generator.Start();
        }

        void OnDestroy()
        {
            Generator.Abort();
        }
        
        public Chunk GetChunk(Position position)
        {
            Position snappedPosition = new Position
            {
                x = Mathf.FloorToInt(position.x/(float) WorldSettings.ChunkSize)*WorldSettings.ChunkSize,
                y = Mathf.FloorToInt(position.y/(float) WorldSettings.ChunkSize)*WorldSettings.ChunkSize,
                z = Mathf.FloorToInt(position.z/(float) WorldSettings.ChunkSize)*WorldSettings.ChunkSize
            };

            Chunk containerChunk;

            Chunks.TryGetValue(snappedPosition, out containerChunk);

            return containerChunk;
        }

        public WorldObject GetObject(Position position)
        {
            Chunk containerChunk = GetChunk(position);

            if (containerChunk != null)
            {
                WorldObject block = containerChunk.GetObject(position - containerChunk.Position);

                return block;
            }
            
            return new Air();
        }

        public void SetObject(Position position, WorldObject worldObject)
        {
            Chunk chunk = GetChunk(position);
            
            if (chunk != null)
            {
                chunk.SetObject(position, worldObject);

                UpdateIfNeighbour(position.x - chunk.Position.x, 0, new Position(position.x - 1, position.y, position.z));
                UpdateIfNeighbour(position.x - chunk.Position.x, WorldSettings.ChunkSize - 1, new Position(position.x + 1, position.y, position.z));
                UpdateIfNeighbour(position.y - chunk.Position.y, 0, new Position(position.x, position.y - 1, position.z));
                UpdateIfNeighbour(position.y - chunk.Position.y, WorldSettings.ChunkSize - 1, new Position(position.x, position.y + 1, position.z));
                UpdateIfNeighbour(position.z - chunk.Position.z, 0, new Position(position.x, position.y, position.z - 1));
                UpdateIfNeighbour(position.z - chunk.Position.z, WorldSettings.ChunkSize - 1, new Position(position.x, position.y, position.z + 1));
            }
        }

        public void DestroyChunk(Position position)
        {
            Chunk chunk = GetChunk(position);

            if (chunk != null)
            {
                chunk.GetComponent<MeshFilter>().mesh = new Mesh();
                PoolManager.Despawn(chunk.gameObject);
                Chunks.Remove(position);
            }
        }

        void UpdateIfNeighbour(int value1, int value2, Position position)
        {
            if (value1 == value2)
            {
                Chunk chunk = GetChunk(position);

                if (chunk != null)
                    chunk.DoRebuild();
            }
        }
    }
}
