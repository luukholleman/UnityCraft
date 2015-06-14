using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.World
{
    class LoadChunks : MonoBehaviour
    {
        public World World;

        int timer;

        private static List<WorldPosition> _chunkPositions = new List<WorldPosition>();

        private readonly List<WorldPosition> _updateList = new List<WorldPosition>();
        private readonly List<WorldPosition> _buildList = new List<WorldPosition>();

        public const int BatchChunkCount = 1;

        void Start()
        {
            int range = (int)Math.Ceiling((float)(World.ViewingRange/Chunk.ChunkSize));

            for (int x = -range; x < range; x++)
            {
                for (int y = -range; y < range; y++)
                {
                    for (int z = -range; z < range; z++)
                    {
                        _chunkPositions.Add(new WorldPosition(x * Chunk.ChunkSize, y * Chunk.ChunkSize, z * Chunk.ChunkSize));   
                    }
                }
            }

            _chunkPositions = _chunkPositions.OrderBy(w => w.ToVector3().magnitude).ToList();
        }

        void Update()
        {
            if (DeleteChunks())
                return;

            FindChunksToLoad();
            LoadAndRenderChunks();
        }

        void FindChunksToLoad()
        {
            WorldPosition playerPosition = new WorldPosition(
                Mathf.FloorToInt(transform.position.x / Chunk.ChunkSize) * Chunk.ChunkSize,
                Mathf.FloorToInt(transform.position.y / Chunk.ChunkSize) * Chunk.ChunkSize,
                Mathf.FloorToInt(transform.position.z/ Chunk.ChunkSize) * Chunk.ChunkSize
            );
            
            if (!_updateList.Any())
            {
                int newChunkCount = 0;
                foreach (WorldPosition chunkPosition in _chunkPositions)
                {
                    WorldPosition newChunkWorldPosition = new WorldPosition(playerPosition + chunkPosition);
                    //WorldPosition newChunkWorldPosition = new WorldPosition(_chunkPositions[i].x * Chunk.ChunkSize + playerPosition.x, _chunkPositions[i].y * Chunk.ChunkSize + playerPosition.y, _chunkPositions[i].z * Chunk.ChunkSize + playerPosition.z);

                    //Get the chunk in the defined position
                    Chunk newChunk = World.GetChunk(newChunkWorldPosition);

                    //If the chunk already exists and it's already
                    //Rendered or in queue to be Rendered continue
                    if (newChunk != null && (newChunk.Rendered || _updateList.Contains(newChunkWorldPosition)))
                        continue;

                    //load a column of chunks in this position
                    //for (int x = chunkPosition.x - Chunk.ChunkSize; x <= chunkPosition.x + Chunk.ChunkSize; x += Chunk.ChunkSize)
                    //{
                    //    for (int y = chunkPosition.y - Chunk.ChunkSize; y <= chunkPosition.y + Chunk.ChunkSize; y += Chunk.ChunkSize)
                    //    {
                    //        for (int z = chunkPosition.z - Chunk.ChunkSize; z <= chunkPosition.z + Chunk.ChunkSize; z += Chunk.ChunkSize)
                    //        {
                    _buildList.Add(new WorldPosition(newChunkWorldPosition));

                    _updateList.Add(new WorldPosition(newChunkWorldPosition));
                    
                    //        }
                    //    }
                    //}

                    if (++newChunkCount >= BatchChunkCount)
                        return;
                }
            }
        }

        void CreateNewChunkPrefab(WorldPosition position)
        {
            if (World.GetChunk(position) == null)
                World.CreateNewChunkPrefab(position);
        }

        void LoadAndRenderChunks()
        {
            if (_buildList.Count != 0)
            {
                for (int i = 0; i < _buildList.Count && i < 8; i++)
                {
                    CreateNewChunkPrefab(_buildList[0]);
                    _buildList.RemoveAt(0);
                }

                return;
            }

            if (_updateList.Count != 0)
            {
                Chunk chunk = World.GetChunk(new WorldPosition(_updateList[0].x, _updateList[0].y, _updateList[0].z));
                if (chunk != null)
                    chunk.Rebuild = true;
                _updateList.RemoveAt(0);
            }
        }

        bool DeleteChunks()
        {
            if (timer == 10)
            {
                var chunksToDelete = new List<WorldPosition>();
                foreach (var chunk in World.Chunks)
                {
                    float distance = Vector3.Distance(new Vector3(chunk.Value.WorldPosition.x, 0, chunk.Value.WorldPosition.z), new Vector3(transform.position.x, 0, transform.position.z));

                    if (distance > 256)
                        chunksToDelete.Add(chunk.Key);
                }

                foreach (var chunk in chunksToDelete)
                    World.DestroyChunk(chunk.x, chunk.y, chunk.z);

                timer = 0;
                return true;
            }

            timer++;
            return false;
        }
    }
}
