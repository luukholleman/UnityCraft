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
        public const int MaxHorizontalGenerationDistance = World.WorldSettings.ViewingRange / World.WorldSettings.ChunkSize;
        public const int MaxVerticalGenerationDistance = World.WorldSettings.ViewingRange / World.WorldSettings.ChunkSize / 5;

        // Thread parameters
        public List<GenerateChunk> ChunkGenerators = new List<GenerateChunk>();
        private bool _aborted = false;
        private readonly int _maxActiveThreads;

        // variables for generating and holding chunks
        private Dictionary<Position, ChunkData> _chunks = new Dictionary<Position, ChunkData>();

        private readonly Dictionary<Position, ChunkData> _toAdd = new Dictionary<Position, ChunkData>();
        private readonly List<Position> _toRemove = new List<Position>();

        private readonly List<Position> _chunkScope = new List<Position>((MaxHorizontalGenerationDistance * 2) * (MaxVerticalGenerationDistance * 2) * (MaxHorizontalGenerationDistance * 2));

        // player position
        public Position PlayerPosition = new Position(0, 0, 0);
        
        public Generator()
        {
            _maxActiveThreads = Math.Max(2, Environment.ProcessorCount);

            for (int x = -MaxHorizontalGenerationDistance / 2; x < MaxHorizontalGenerationDistance; x++)
                for (int y = -MaxVerticalGenerationDistance / 2; y < MaxVerticalGenerationDistance; y++)
                    for (int z = -MaxHorizontalGenerationDistance / 2; z < MaxHorizontalGenerationDistance; z++)
                        if (Vector3.Distance(Vector3.zero, new Position(x * World.WorldSettings.ChunkSize, y * World.WorldSettings.ChunkSize, z * World.WorldSettings.ChunkSize).ToVector3()) < MaxHorizontalGenerationDistance * World.WorldSettings.ChunkSize)
                            _chunkScope.Add(new Position(x * World.WorldSettings.ChunkSize, y * World.WorldSettings.ChunkSize, z * World.WorldSettings.ChunkSize));

            _chunkScope = _chunkScope.OrderBy(w => w.ToVector3().magnitude).ToList();
        }

        public void SetPlayerPosition(Position playerPosition)
        {
            PlayerPosition = Helper.SnapToGrid(playerPosition);
        }

        protected override void ThreadFunction()
        {
            try
            {
                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;

                while (!_aborted)
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

                        if (!Equals(currentPlayerPosition, PlayerPosition))
                            break;
                    }

                    foreach (GenerateChunk chunkGenerator in ChunkGenerators.Where(g => g.IsDone))
                    {
                        lock (_toAdd)
                        {
                            _toAdd[chunkGenerator.Position] = chunkGenerator.ChunkData;
                        }

                        _chunks.Add(chunkGenerator.Position, chunkGenerator.ChunkData);
                    }

                    ChunkGenerators.RemoveAll(g => g.IsDone);

                    List<KeyValuePair<Position, ChunkData>> tmpRoRemove = new List<KeyValuePair<Position, ChunkData>>();

                    foreach (KeyValuePair<Position, ChunkData> chunk in _chunks)
                    {
                        if (Vector3.Distance(chunk.Key.ToVector3(), PlayerPosition.ToVector3()) > MaxHorizontalGenerationDistance * World.WorldSettings.ChunkSize)
                        {
                            lock (_toRemove)
                            {
                                _toRemove.Add(chunk.Key);
                            }

                            tmpRoRemove.Add(chunk);
                        }
                    }

                    foreach (KeyValuePair<Position, ChunkData> pair in tmpRoRemove)
                    {
                        _chunks.Remove(pair.Key);
                    }

                    _chunks = new Dictionary<Position, ChunkData>(_chunks);

                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }

        public IEnumerable<KeyValuePair<Position, ChunkData>> GetNewChunks()
        {
            for (; ; )
            {
                KeyValuePair<Position, ChunkData> chunk;

                lock (_toAdd)
                {
                    chunk = _toAdd.FirstOrDefault();

                    if (chunk.Equals(default(KeyValuePair<Position, ChunkData>)))
                        yield break;
                    _toAdd.Remove(chunk.Key);
                }

                yield return chunk;
            }
        }

        public IEnumerable<Position> GetOutOfRangeChunks()
        {
            List<Position> tmp;

            lock (_toRemove)
            {
                tmp = new List<Position>(_toRemove);
                _toRemove.Clear();
            }

            return tmp;
            //for (; ; )
            //{
            //    KeyValuePair<Position, ChunkData> chunk;

            //    lock (_toRemove)
            //    {
            //        chunk = _toRemove.FirstOrDefault();

            //        if (chunk.Equals(default(KeyValuePair<Position, ChunkData>)))
            //            yield break;

            //        _toRemove.Remove(chunk.Key);
            //    }

            //    yield return chunk.Key;
            //}
        }

        private bool ChunkExists(Position position)
        {
            bool exists = false;

            lock (_chunks)
                exists = _chunks.ContainsKey(position) || _toAdd.ContainsKey(position);

            return exists;
        }

        private bool ChunkIsGenerating(Position position)
        {
            return ChunkGenerators.Any(g => Equals(g.Position, position));
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
