using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Code.GenerationEngine;
using Assets.Code.World;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Assets.Code.Player
{
    class ChunkLoader : MonoBehaviour
    {
        public World.World World;

        private static List<Position> _chunkPositions = new List<Position>();

        public const int BatchChunkCount = 2;

        private const int MaxMillisecondtime = 5;

        void Start()
        {
            int range = (int)Math.Floor((float)(Code.World.World.ViewingRange/Code.World.World.ChunkSize));

            for (int x = -range; x < range; x++)
                for (int y = -range / 2; y < range / 2; y++)
                    for (int z = -range; z < range; z++)
                        _chunkPositions.Add(new Position(x * Code.World.World.ChunkSize, y * Code.World.World.ChunkSize, z * Code.World.World.ChunkSize));   

            _chunkPositions = _chunkPositions.OrderBy(w => w.ToVector3().magnitude).ToList();

            StartCoroutine("GetChunksFromGenerationEngine");
            StartCoroutine("DeleteChunks");
        }

        IEnumerator GetChunksFromGenerationEngine()
        {
            for (;;)
            {
                if (Code.World.World.Generator == null)
                    yield return null; 
                
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Code.World.World.Generator.SetPlayerPosition(new Position(transform.position));

                int i = 0;

                foreach (Position chunkPosition in _chunkPositions)
                {
                    if (sw.ElapsedMilliseconds >= MaxMillisecondtime)
                        break;

                    Position newChunkPosition = new Position(Code.World.World.Generator.PlayerPosition + chunkPosition);

                    //Get the chunk in the defined position
                    GenerationEngine.Chunk newChunk = Code.World.World.Generator.GetChunk(newChunkPosition);

                    //If the chunk already exists and it's already
                    //Rendered or in queue to be Rendered continue
                    if (newChunk == null)
                        continue;

                    if (Code.World.World.Generator.GetChunk(newChunkPosition) == null)
                    {
                        break;
                    }

                    if (World.CreateNewChunkPrefab(newChunkPosition))
                    {
                        if (++i > BatchChunkCount)
                            break;
                    }
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

                    if (distance > Code.World.World.ViewingRange + Generator.ChunkSize)
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
