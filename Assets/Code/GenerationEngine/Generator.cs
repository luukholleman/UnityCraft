using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Thread;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    public class Generator : ThreadedJob
    {
        public Position PlayerPosition = new Position(0,0,0);

        public int ThreadCount;

        public const int ChunkSize = 16;

        public const int MaxHorizontalGenerationDistance = 20;
        public const int MaxVerticalGenerationDistance = 10;

        private Dictionary<Position, Chunk> _chunks = new Dictionary<Position, Chunk>();

        private List<Position> _chunkScope = new List<Position>((MaxHorizontalGenerationDistance * 2) * (MaxVerticalGenerationDistance * 2) *(MaxHorizontalGenerationDistance * 2));

        private bool _aborted = false;

        public Generator()
        {
            ThreadCount = Environment.ProcessorCount;

            for (int x = -MaxHorizontalGenerationDistance / 2; x < MaxHorizontalGenerationDistance; x++)
                for (int y = -MaxVerticalGenerationDistance / 2; y < MaxVerticalGenerationDistance; y++)
                    for (int z = -MaxHorizontalGenerationDistance / 2; z < MaxHorizontalGenerationDistance; z++)
                        _chunkScope.Add(new Position(x * 16, y * 16, z * 16));

            _chunkScope = _chunkScope.OrderBy(w => w.ToVector3().magnitude).ToList();
        }

        public void SetPlayerPosition(Position playerPosition)
        {
            PlayerPosition = SnapToGrid(playerPosition);
        }

        public Chunk GetChunk(Position position)
        {
            position = SnapToGrid(position);
            Chunk chunk = null;

            lock (_chunks)
            {
                _chunks.TryGetValue(position, out chunk);   
            }

            return chunk;
        }

        public Position SnapToGrid(Position origPosition)
        {

            Position playerChunk = new Position(
                Mathf.FloorToInt(origPosition.x / ChunkSize) * ChunkSize,
                Mathf.FloorToInt(origPosition.y / ChunkSize) * ChunkSize,
                Mathf.FloorToInt(origPosition.z / ChunkSize) * ChunkSize
                );

            return playerChunk;
        }

        protected override void ThreadFunction()
        {
            while(!_aborted)
            {
                Position currentPlayerPosition = PlayerPosition;

                Position playerChunk = SnapToGrid(currentPlayerPosition);

                foreach (Position chunkPosition in _chunkScope)
                {
                    // extra check, otherwise the thread could go on for a while
                    if (_aborted)
                        break;

                    Position absoluteChunkPosition = new Position(playerChunk + chunkPosition);

                    bool chunkExists;

                    lock (_chunks)
                    {
                        chunkExists = _chunks.ContainsKey(absoluteChunkPosition);
                    }

                    if (!chunkExists)
                    {
                        Chunk newChunk = new Chunk(absoluteChunkPosition);

                        newChunk.Generate();

                        lock (_chunks)
                        {
                            _chunks.Add(absoluteChunkPosition, newChunk);
                        }
                    }

                    if(currentPlayerPosition != PlayerPosition)
                        break;
                }
            }
        }

        public override void Abort()
        {
            _aborted = true;
            base.Abort();
        }

        protected override void OnFinished()
        {
            throw new Exception("This should not be finished");
        }
    }
}
