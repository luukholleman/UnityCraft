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
        // Generation parameters
        public const int ChunkSize = 16;
        public const int MaxHorizontalGenerationDistance = World.World.ViewingRange / ChunkSize;
        public const int MaxVerticalGenerationDistance = World.World.ViewingRange / ChunkSize / 5;

        // Thread parameters
        public List<GenerateChunk> ChunkGenerators = new List<GenerateChunk>();
        private bool _aborted = false;
        private readonly int _maxActiveThreads;

        // variables for generating and holding chunks
        private readonly Dictionary<Position, Chunk> _chunks = new Dictionary<Position, Chunk>();
        private readonly List<Position> _chunkScope = new List<Position>((MaxHorizontalGenerationDistance * 2) * (MaxVerticalGenerationDistance * 2) *(MaxHorizontalGenerationDistance * 2));

        // player position
        public Position PlayerPosition = new Position(0, 0, 0);

        public Generator()
        {
            _maxActiveThreads = Environment.ProcessorCount;

            for (int x = -MaxHorizontalGenerationDistance / 2; x < MaxHorizontalGenerationDistance; x++)
                for (int y = -MaxVerticalGenerationDistance / 2; y < MaxVerticalGenerationDistance; y++)
                    for (int z = -MaxHorizontalGenerationDistance / 2; z < MaxHorizontalGenerationDistance; z++)
                        _chunkScope.Add(new Position(x * 16, y * 16, z * 16));
            _chunkScope = _chunkScope.OrderBy(w => w.ToVector3().magnitude).ToList();
        }

        public void SetPlayerPosition(Position playerPosition)
        {
            PlayerPosition = Helper.SnapToGrid(playerPosition);
        }

        public Chunk GetChunk(Position position)
        {
            position = Helper.SnapToGrid(position);

            Chunk chunk;

            lock (_chunks)
            {
                _chunks.TryGetValue(position, out chunk);   
            }

            return chunk;
        }

        protected override void ThreadFunction()
        {
            while(!_aborted)
            {
                Position currentPlayerPosition = PlayerPosition;

                Position playerChunk = Helper.SnapToGrid(currentPlayerPosition);

                foreach (Position chunkPosition in _chunkScope)
                {
                    if (ChunkGenerators.Count > _maxActiveThreads)
                        break;

                    // extra check, otherwise the thread could go on for a while
                    if (_aborted)
                        break;

                    Position absoluteChunkPosition = new Position(playerChunk + chunkPosition);

                    if (!ChunkExists(absoluteChunkPosition) && !ChunkIsGenerating(absoluteChunkPosition))
                    {
                        GenerateChunk generateChunk = new GenerateChunk(absoluteChunkPosition);

                        generateChunk.Start();

                        ChunkGenerators.Add(generateChunk);
                    }

                    if(!Equals(currentPlayerPosition, PlayerPosition))
                        break;
                }

                foreach (GenerateChunk chunkGenerator in ChunkGenerators.Where(g => g.IsDone))
                {
                    lock (_chunks)
                    {
                        _chunks.Add(chunkGenerator.Position, chunkGenerator.Chunk);
                    }
                }

                ChunkGenerators.RemoveAll(g => g.IsDone);

                System.Threading.Thread.Sleep(100);
            }
        }

        public bool ChunkExists(Position position)
        {
            bool exists = false;

            lock (_chunks)
            {
                exists = _chunks.ContainsKey(position);
            }

            return exists;
        }

        public bool ChunkIsGenerating(Position position)
        {
            bool exists = false;

            lock (ChunkGenerators)
            {
               exists = ChunkGenerators.Any(g => Equals(g.Position, position));
            }

            return exists;
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
