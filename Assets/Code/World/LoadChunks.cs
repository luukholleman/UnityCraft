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

        private static List<Position> _chunkPositions = new List<Position>();

        private readonly List<Position> _buildList = new List<Position>();

        private readonly List<Position> _doneList = new List<Position>();

        public const int BatchChunkCount = 8;

        void Start()
        {
            int range = (int)Math.Ceiling((float)(World.ViewingRange/Chunk.ChunkSize));

            for (int x = -range; x < range; x++)
                for (int y = -range; y < range; y++)
                    for (int z = -range; z < range; z++)
                        _chunkPositions.Add(new Position(x * Chunk.ChunkSize, y * Chunk.ChunkSize, z * Chunk.ChunkSize));   

            _chunkPositions = _chunkPositions.OrderBy(w => w.ToVector3().magnitude).ToList();

            //StartCoroutine("FindChunksToLoad");
            //StartCoroutine("LoadAndRenderChunks");
            //StartCoroutine("DeleteChunks");
            StartCoroutine("GetChunksFromGenerationEngine");
        }

        IEnumerator GetChunksFromGenerationEngine()
        {
            for (;;)
            {
                if (World.Generator == null)
                    yield return null;

                World.Generator.SetPlayerPosition(new Position(transform.position));

                foreach (Position chunkPosition in _chunkPositions)
                {
                    Position newChunkPosition = new Position(World.Generator.PlayerPosition + chunkPosition);

                    //Get the chunk in the defined position
                    GenerationEngine.Chunk newChunk = World.Generator.GetChunk(newChunkPosition);

                    //If the chunk already exists and it's already
                    //Rendered or in queue to be Rendered continue
                    if (newChunk == null)
                        continue;

                    if (!CreateNewChunkPrefab(newChunkPosition))
                        break;
                }

                yield return null;
            }
        }
        
        IEnumerator FindChunksToLoad()
        {
            for (;;)
            {
                Vector3 origPos = transform.position;

                Position playerPosition = new Position(
                    Mathf.FloorToInt(transform.position.x / Chunk.ChunkSize) * Chunk.ChunkSize,
                    Mathf.FloorToInt(transform.position.y / Chunk.ChunkSize) * Chunk.ChunkSize,
                    Mathf.FloorToInt(transform.position.z / Chunk.ChunkSize) * Chunk.ChunkSize
                );

                int i = 0;
                foreach (Position chunkPosition in _chunkPositions)
                {
                    Position newChunkPosition = new Position(playerPosition + chunkPosition);

                    //Get the chunk in the defined position
                    Chunk newChunk = World.GetChunk(newChunkPosition);

                    //If the chunk already exists and it's already
                    //Rendered or in queue to be Rendered continue
                    if (newChunk != null || _doneList.Contains(newChunkPosition))
                        continue;
                    
                    _buildList.Add(newChunkPosition);

                    if (++i >= BatchChunkCount)
                    {
                        yield return null;

                        if (Vector3.Distance(origPos,transform.position) > Chunk.ChunkSize)
                            break;
                    }
                }

                yield return new WaitForSeconds(Random.value / 4);
            }
        }

        bool CreateNewChunkPrefab(Position position)
        {
            if (World.Generator.GetChunk(position) != null)
            {
                World.CreateNewChunkPrefab(position);

                return true;
            }

            return false;
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
            yield return null;
            for (;;)
            {
                break;
                var chunksToDelete = new List<Position>();

                foreach (var chunk in World.Chunks)
                {
                    float distance = chunk.Value.Position.ManhattanDistance(new Position(transform.position));

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
