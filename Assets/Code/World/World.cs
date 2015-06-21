using System.Collections.Generic;
using System.Linq;
using Assets.Code.Blocks;
using Assets.Code.GenerationEngine;
using UnityEngine;
using Chunk = Assets.Code.World.Chunks.Chunk;

namespace Assets.Code.World
{
    public class World : MonoBehaviour
    {
        public Dictionary<Position, Chunk> Chunks = new Dictionary<Position, Chunk>();

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

                Chunk newChunk = newChunkObject.GetComponent<Chunk>();

                newChunk.Position = position;
                newChunk.World = this;

                newChunk.Blocks = Generator.GetChunk(position).Blocks;

                foreach (KeyValuePair<Position, Block> block in Generator.GetChunk(position).BeyondChunkBlocks)
                {
                    newChunk.SetBlock(block.Key, block.Value);
                }

                newChunk.SetBlocksUnmodified();

                //Serialization.Load(newChunk);

                newChunk.Rebuild = true;

                Chunks.Add(position, newChunk);
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
        
        public Chunk GetChunk(Position position, bool alreadyAbsolute = false)
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

            Chunk containerChunk;

            Chunks.TryGetValue(absolutePosition, out containerChunk);

            return containerChunk;
        }

        public Block GetBlock(Position position)
        {
            Chunk containerChunk = GetChunk(position);

            if (containerChunk != null)
            {
                Block block = containerChunk.GetBlock(
                    position.x - containerChunk.Position.x,
                    position.y - containerChunk.Position.y,
                    position.z - containerChunk.Position.z);

                return block;
            }
            
            return new Air();
        }
        
        public void SetBlock(Position position, Block block)
        {
            Chunk chunk = GetChunk(position);
            
            if (chunk != null)
            {
                if (chunk.SetBlock(position, block))
                {
                    chunk.Rebuild = true;
                }

                UpdateIfEqual(position.x - chunk.Position.x, 0, new Position(position.x - 1, position.y, position.z));
                UpdateIfEqual(position.x - chunk.Position.x, ChunkSize - 1, new Position(position.x + 1, position.y, position.z));
                UpdateIfEqual(position.y - chunk.Position.y, 0, new Position(position.x, position.y - 1, position.z));
                UpdateIfEqual(position.y - chunk.Position.y, ChunkSize - 1, new Position(position.x, position.y + 1, position.z));
                UpdateIfEqual(position.z - chunk.Position.z, 0, new Position(position.x, position.y, position.z - 1));
                UpdateIfEqual(position.z - chunk.Position.z, ChunkSize - 1, new Position(position.x, position.y, position.z + 1));
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

        void UpdateIfEqual(int value1, int value2, Position position)
        {
            if (value1 == value2)
            {
                Chunk chunk = GetChunk(position);

                if (chunk != null)
                    chunk.Rebuild = true;
            }
        }
    }
}
