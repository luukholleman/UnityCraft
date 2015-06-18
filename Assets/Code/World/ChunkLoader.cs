using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.GenerationEngine;
using UnityEngine;
using Chunk = Assets.Code.World.Chunks.Chunk;
using Random = UnityEngine.Random;

namespace Assets.Code.World
{
    class ChunkLoader : MonoBehaviour
    {
        public World World;

        private static List<Position> _chunkPositions = new List<Position>();

        public const int BatchChunkCount = 8;

        void Start()
        {
            int range = (int)Math.Floor((float)(World.ViewingRange/World.ChunkSize));

            for (int x = -range; x < range; x++)
                for (int y = -range; y < range; y++)
                    for (int z = -range; z < range; z++)
                        _chunkPositions.Add(new Position(x * World.ChunkSize, y * World.ChunkSize, z * World.ChunkSize));   

            _chunkPositions = _chunkPositions.OrderBy(w => w.ToVector3().magnitude).ToList();

            StartCoroutine("GetChunksFromGenerationEngine");
            StartCoroutine("DeleteChunks");
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

                    if (World.Generator.GetChunk(newChunkPosition) == null)
                    {
                        break;
                    }

                    World.CreateNewChunkPrefab(newChunkPosition);
                }

                yield return null;
            }
        }
        
        IEnumerator DeleteChunks()
        {
            for (;;)
            {
                var chunksToDelete = new List<Position>();

                foreach (var chunk in World.Chunks)
                {
                    float distance = chunk.Value.Position.ManhattanDistance(new Position(transform.position));

                    if (distance > World.ViewingRange + Generator.ChunkSize)
                        chunksToDelete.Add(chunk.Key);
                }

                foreach (var chunk in chunksToDelete)
                {
                    World.DestroyChunk(chunk);
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
