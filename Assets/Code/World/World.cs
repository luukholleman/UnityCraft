using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using Assets.Code.World.Terrain;
using Assets.Code.World.Thread;
using UnityEngine;

namespace Assets.Code.World
{
    public class World : MonoBehaviour
    {
        public Dictionary<WorldPosition, Chunk> Chunks = new Dictionary<WorldPosition, Chunk>();

        public GameObject ChunkPrefab;

        public const int ViewingRange = 16 * 15;

        public const int LevelHeight = 4;

        public string WorldName = "world";

        private readonly List<FillChunkJob> _jobs = new List<FillChunkJob>();

        private const int ConcurrentJobCount = 8;

        private const int JobsFinishPerFrame = 10;

        public void CreateNewChunkPrefab(WorldPosition worldPosition)
        {
            //Instantiate the chunk at the coordinates using the chunk prefab
            GameObject newChunkObject = Instantiate(
                            ChunkPrefab, worldPosition.ToVector3(),
                            Quaternion.Euler(Vector3.zero)
                        ) as GameObject;

            Chunk newChunk = newChunkObject.GetComponent<Chunk>();

            newChunk.WorldPosition = worldPosition;
            newChunk.World = this;

            Chunks.Add(worldPosition, newChunk);

            FillChunkJob chunkJobFiller = new FillChunkJob(worldPosition);
            
            _jobs.Add(chunkJobFiller);
        }

        void Update()
        {
            foreach (FillChunkJob job in _jobs.Take(ConcurrentJobCount))
            {
                job.Start();
            }

            List<FillChunkJob> done = new List<FillChunkJob>();

            int i = 0;

            foreach (FillChunkJob job in _jobs)
            {
                if (job.IsDone)
                {
                    Chunk newChunk = GetChunk(job.WorldPosition);

                    // chunk can be deleted meanwhile
                    if (newChunk != null)
                    {
                        newChunk.SetBlocks(job.Blocks.ToList());

                        newChunk.SetBlocksUnmodified();

                        Serialization.Load(newChunk);

                        newChunk.Built = true;
                        newChunk.Rebuild = true;

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

        public Chunk GetChunk(WorldPosition position)
        {
            WorldPosition absolutePosition = new WorldPosition();

            absolutePosition.x = Mathf.FloorToInt(position.x / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
            absolutePosition.y = Mathf.FloorToInt(position.y / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
            absolutePosition.z = Mathf.FloorToInt(position.z / (float)Chunk.ChunkSize) * Chunk.ChunkSize;

            Chunk containerChunk;

            Chunks.TryGetValue(absolutePosition, out containerChunk);

            return containerChunk;
        }

        public Block GetBlock(WorldPosition position)
        {
            Chunk containerChunk = GetChunk(position);

            if (containerChunk != null)
            {
                Block block = containerChunk.GetBlock(
                    position.x - containerChunk.WorldPosition.x,
                    position.y - containerChunk.WorldPosition.y,
                    position.z - containerChunk.WorldPosition.z);

                return block;
            }
            
            return new BlockAir();
        }

        public void SetBlock(WorldPosition position, Block block)
        {
            Chunk chunk = GetChunk(position);
            
            if (chunk != null)
            {
                if(chunk.SetBlock(position, block))
                    chunk.Rebuild = true;

                UpdateIfEqual(position.x - chunk.WorldPosition.x, 0, new WorldPosition(position.x - 1, position.y, position.z));
                UpdateIfEqual(position.x - chunk.WorldPosition.x, Chunk.ChunkSize - 1, new WorldPosition(position.x + 1, position.y, position.z));
                UpdateIfEqual(position.y - chunk.WorldPosition.y, 0, new WorldPosition(position.x, position.y - 1, position.z));
                UpdateIfEqual(position.y - chunk.WorldPosition.y, Chunk.ChunkSize - 1, new WorldPosition(position.x, position.y + 1, position.z));
                UpdateIfEqual(position.z - chunk.WorldPosition.z, 0, new WorldPosition(position.x, position.y, position.z - 1));
                UpdateIfEqual(position.z - chunk.WorldPosition.z, Chunk.ChunkSize - 1, new WorldPosition(position.x, position.y, position.z + 1));
            }
        }

        public void DestroyChunk(int x, int y, int z)
        {
            Chunk chunk;

            if (Chunks.TryGetValue(new WorldPosition(x, y, z), out chunk))
            {
                Serialization.SaveChunk(chunk);
                Destroy(chunk.gameObject);
                Chunks.Remove(new WorldPosition(x, y, z));
            }
        }

        void UpdateIfEqual(int value1, int value2, WorldPosition position)
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
