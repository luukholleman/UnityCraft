using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Code.Tasker;
using Assets.Code.Thread;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    public class Generator : ThreadedJob
    {
        // Generation parameters
        public const int MaxHorizontalGenerationDistance = WorldSettings.ViewingRange / WorldSettings.ChunkSize;
        public const int MaxVerticalGenerationDistance = 2;

        // Thread parameters
        private bool _aborted;

        // variables for generating and holding chunks
        private Dictionary<Position, ChunkData> _chunks = new Dictionary<Position, ChunkData>();

        private readonly List<Position> _chunkScope = new List<Position>((MaxHorizontalGenerationDistance * 2) * (MaxVerticalGenerationDistance * 2) * (MaxHorizontalGenerationDistance * 2));

        // player position
        public Position PlayerPosition = new Position(0, 0, 0);

        private object _lock = new object();

        private Feeder _feeder = new Feeder();

        private List<Position> _generating = new List<Position>(); 

        public Generator()
        {
            PlayerPosition = new Position(GameObject.FindWithTag("Player").transform.position);

            for (int x = -MaxHorizontalGenerationDistance; x < MaxHorizontalGenerationDistance; x++)
                for (int y = -MaxVerticalGenerationDistance; y < MaxVerticalGenerationDistance; y++)
                    for (int z = -MaxHorizontalGenerationDistance; z < MaxHorizontalGenerationDistance; z++)
                        if (Vector3.Distance(Vector3.zero, new Position(x * WorldSettings.ChunkSize, y * WorldSettings.ChunkSize, z * WorldSettings.ChunkSize).ToVector3()) < MaxHorizontalGenerationDistance * WorldSettings.ChunkSize)
                            _chunkScope.Add(new Position(x * WorldSettings.ChunkSize, y * WorldSettings.ChunkSize, z * WorldSettings.ChunkSize));

            _chunkScope = _chunkScope.OrderBy(w => w.ToVector3().magnitude).ToList();

            _feeder.Generator = this;
            _feeder.Start();
        }

        public void SetPlayerPosition(Position playerPosition)
        {
            lock (_lock)
            {
                PlayerPosition = Helper.SnapToGrid(playerPosition);
            }
        }

        protected override void ThreadFunction()
        {
            try
            {
                while (!_aborted)
                {
                    Position currentPlayerPosition;

                    lock (_lock)
                    {
                        currentPlayerPosition = PlayerPosition;
                    }

                    foreach (Position chunkPosition in _chunkScope)
                    {
                        if (_aborted)
                            break;

                        Position absoluteChunkPosition = Helper.SnapToGrid(new Position(currentPlayerPosition + chunkPosition));

                        if (!ChunkExists(absoluteChunkPosition))
                        {
                            GenerateChunk generateChunk = new GenerateChunk(absoluteChunkPosition, Callback);

                            ThreadPool.QueueUserWorkItem(generateChunk.Generate);

                            _generating.Add(absoluteChunkPosition);

                            lock (_lock)
                            {
                                if (!Equals(currentPlayerPosition, PlayerPosition))
                                    break;
                            }

                            System.Threading.Thread.Sleep(10);
                        }
                    }

                    List<Position> toRemove = new List<Position>();

                    lock (_lock)
                    {
                        foreach (KeyValuePair<Position, ChunkData> chunk in _chunks)
                        {
                            if (Vector3.Distance(chunk.Key.ToVector3(), currentPlayerPosition.ToVector3()) > MaxHorizontalGenerationDistance * WorldSettings.ChunkSize)
                            {
                                toRemove.Add(chunk.Key);

                                Tasker.Tasker.Instance.Add(new DeleteChunk(chunk.Key));
                            }
                        }   
                    }

                    foreach (Position position in toRemove)
                    {
                        _chunks.Remove(position);
                    }

                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Generator " + e);

                Abort();
            }
        }

        public void Callback(Position position, ChunkData chunkData)
        {
            lock (_feeder)
            {
                _feeder.Add(new KeyValuePair<Position, ChunkData>(position, chunkData));
            }

            lock (_lock)
            {
                _chunks[position] = chunkData;

                _generating.Remove(position);
            }
        }

        private bool ChunkExists(Position position)
        {
            lock (_lock)
            {
                return _chunks.ContainsKey(position) || _generating.Contains(position);
            }
        }

        public override void Abort()
        {
            _aborted = true;

            lock (_feeder)
            {
                _feeder.Abort();
            }

            base.Abort();
        }

        protected override void OnFinished()
        {
            throw new Exception("This should not be finished");
        }
    }
}
