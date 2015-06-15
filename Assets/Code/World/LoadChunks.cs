using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.World.Chunks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.World
{
    class LoadChunks : MonoBehaviour
    {
        public World World;

        int timer;

        private static List<WorldPosition> _chunkPositions = new List<WorldPosition>();

        private readonly List<WorldPosition> _buildList = new List<WorldPosition>();

        private readonly List<WorldPosition> _doneList = new List<WorldPosition>();

        public const int BatchChunkCount = 8;

        void Start()
        {
            int range = (int)Math.Ceiling((float)(World.ViewingRange/Chunk.ChunkSize));

            for (int x = -range; x < range; x++)
                for (int y = -range; y < range; y++)
                    for (int z = -range; z < range; z++)
                        _chunkPositions.Add(new WorldPosition(x * Chunk.ChunkSize, y * Chunk.ChunkSize, z * Chunk.ChunkSize));   

            _chunkPositions = _chunkPositions.OrderBy(w => w.ToVector3().magnitude).ToList();

            StartCoroutine("FindChunksToLoad");
            StartCoroutine("LoadAndRenderChunks");
            StartCoroutine("DeleteChunks");
        }
        
        IEnumerator FindChunksToLoad()
        {
            for (;;)
            {
                Vector3 origPos = transform.position;

                WorldPosition playerPosition = new WorldPosition(
                    Mathf.FloorToInt(transform.position.x / Chunk.ChunkSize) * Chunk.ChunkSize,
                    Mathf.FloorToInt(transform.position.y / Chunk.ChunkSize) * Chunk.ChunkSize,
                    Mathf.FloorToInt(transform.position.z / Chunk.ChunkSize) * Chunk.ChunkSize
                );

                int i = 0;
                foreach (WorldPosition chunkPosition in _chunkPositions)
                {
                    WorldPosition newChunkWorldPosition = new WorldPosition(playerPosition + chunkPosition);

                    //Get the chunk in the defined position
                    Chunk newChunk = World.GetChunk(newChunkWorldPosition);

                    //If the chunk already exists and it's already
                    //Rendered or in queue to be Rendered continue
                    if (newChunk != null || _doneList.Contains(newChunkWorldPosition))
                        continue;
                    
                    _buildList.Add(newChunkWorldPosition);

                    if (++i >= BatchChunkCount)
                    {
                        yield return null;

                        if (origPos != transform.position)
                            break;
                    }
                }

                yield return new WaitForSeconds(Random.value);
            }
        }

        void CreateNewChunkPrefab(WorldPosition position)
        {
            if (World.GetChunk(position) == null)
                World.CreateNewChunkPrefab(position);
        }

        IEnumerator LoadAndRenderChunks()
        {
            for (;;)
            {
                if (_buildList.Count != 0)
                {
                    for (int i = 0; i < _buildList.Count && i < 8; i++)
                    {
                        CreateNewChunkPrefab(_buildList[0]);
                        _doneList.Add(_buildList[0]);
                        _buildList.RemoveAt(0);
                    }
                }
                
                yield return null;
            }
        }

        IEnumerator DeleteChunks()
        {
            for (;;)
            {
                var chunksToDelete = new List<WorldPosition>();

                foreach (var chunk in World.Chunks)
                {
                    float distance = Vector3.Distance(chunk.Value.WorldPosition.ToVector3(), new Vector3(transform.position.x, transform.position.y, transform.position.z));

                    if (distance > World.ViewingRange)
                        chunksToDelete.Add(chunk.Key);
                }

                foreach (var chunk in chunksToDelete)
                {
                    World.DestroyChunk(chunk.x, chunk.y, chunk.z);
                    _doneList.Remove(chunk);
                }

                timer = 0;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
