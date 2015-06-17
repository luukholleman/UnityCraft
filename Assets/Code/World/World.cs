using System.Collections.Generic;
using System.Linq;
using Assets.Code.GenerationEngine;
using Assets.Code.Thread;
using Assets.Code.World.Chunks.Blocks;
using UnityEngine;
using Chunk = Assets.Code.World.Chunks.Chunk;

namespace Assets.Code.World
{
    public class World : MonoBehaviour
    {
        public Dictionary<Position, Chunk> Chunks = new Dictionary<Position, Chunk>();

        public GameObject ChunkPrefab;

        public static Generator Generator;

        public const int ViewingRange = 16*10;

        public const int LevelHeight = 4;

        public string WorldName = "world";

        private readonly List<FillChunkJob> _jobs = new List<FillChunkJob>();

        private const int ConcurrentJobCount = 8;

        private const int JobsFinishPerFrame = 5;

        public void CreateNewChunkPrefab(Position position)
        {
            if (Chunks.ContainsKey(position))
                return;

            //Instantiate the chunk at the coordinates using the chunk prefab
            GameObject newChunkObject = Instantiate(
                            ChunkPrefab, position.ToVector3(),
                            Quaternion.Euler(Vector3.zero)
                        ) as GameObject;

            Chunk newChunk = newChunkObject.GetComponent<Chunk>();

            newChunk.Position = position;
            newChunk.World = this;

            Chunks.Add(position, newChunk);

            foreach (KeyValuePair<Position, Block> block in Generator.GetChunk(position).Blocks)
            {
                newChunk.SetBlock(block.Key, block.Value);
            }

            //newChunk.SetBlocksUnmodified();

            //Serialization.Load(newChunk);

            newChunk.Built = true;
            newChunk.Rebuild = true;
        }
        //public void CreateNewChunkPrefab(Position position)
        //{
        //    //Instantiate the chunk at the coordinates using the chunk prefab
        //    GameObject newChunkObject = Instantiate(
        //                    ChunkPrefab, position.ToVector3(),
        //                    Quaternion.Euler(Vector3.zero)
        //                ) as GameObject;

        //    Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        //    newChunk.Position = position;
        //    newChunk.World = this;

        //    Chunks.Add(position, newChunk);

        //    FillChunkJob chunkJobFiller = new FillChunkJob(position);

        //    _jobs.Add(chunkJobFiller);
        //}

        void Start()
        {
            Generator = new Generator();
            Generator.Start();
        }

        void OnDestroy()
        {
            Generator.Abort();
        }
        
        void Update()
        {
            foreach (FillChunkJob job in _jobs.Take(ConcurrentJobCount))
            {
                job.Start();
            }

            List<FillChunkJob> done = new List<FillChunkJob>();

            int i = 0;

            foreach (FillChunkJob job in _jobs.Take(ConcurrentJobCount))
            {
                if (job.IsDone)
                {
                    Chunk newChunk = GetChunk(job.Position);

                    // chunk can be deleted meanwhile
                    if (newChunk != null)
                    {
                        newChunk.FillWithPreBuiltBlocks(job.Blocks);

                        done.Add(job);
                    }

                    if (++i >= JobsFinishPerFrame)
                        break;
                }
            }

            foreach (FillChunkJob chunk in done)
            {
                _jobs.Remove(chunk);
            }
        }

        public Chunk GetChunk(Position position, bool alreadyAbsolute = false)
        {
            Position absolutePosition;

            if (!alreadyAbsolute)
            {
                absolutePosition = new Position();

                absolutePosition.x = Mathf.FloorToInt(position.x / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
                absolutePosition.y = Mathf.FloorToInt(position.y / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
                absolutePosition.z = Mathf.FloorToInt(position.z / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
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
            
            return new BlockAir();
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
                UpdateIfEqual(position.x - chunk.Position.x, Chunk.ChunkSize - 1, new Position(position.x + 1, position.y, position.z));
                UpdateIfEqual(position.y - chunk.Position.y, 0, new Position(position.x, position.y - 1, position.z));
                UpdateIfEqual(position.y - chunk.Position.y, Chunk.ChunkSize - 1, new Position(position.x, position.y + 1, position.z));
                UpdateIfEqual(position.z - chunk.Position.z, 0, new Position(position.x, position.y, position.z - 1));
                UpdateIfEqual(position.z - chunk.Position.z, Chunk.ChunkSize - 1, new Position(position.x, position.y, position.z + 1));
            }
        }

        public void DestroyChunk(int x, int y, int z)
        {
            Chunk chunk;

            if (Chunks.TryGetValue(new Position(x, y, z), out chunk))
            {
                Serialization.SaveChunk(chunk);
                Destroy(chunk.gameObject);
                Chunks.Remove(new Position(x, y, z));
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
