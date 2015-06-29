using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Code.Thread;
using Assets.Code.World;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    public class Generator : ThreadedJob
    {
        // Generation parameters
        public const int MaxHorizontalGenerationDistance = WorldSettings.ViewingRange / WorldSettings.ChunkSize;
        public const int MaxVerticalGenerationDistance = WorldSettings.ViewingRange / WorldSettings.ChunkSize / 3;

        // Thread parameters
        public List<Position> Generating = new List<Position>(); 
        private bool _aborted;

        // variables for generating and holding chunks
        private Dictionary<Position, ChunkData> _chunks = new Dictionary<Position, ChunkData>();

        private readonly Dictionary<Position, ChunkData> _toAdd = new Dictionary<Position, ChunkData>();
        private readonly List<Position> _toRemove = new List<Position>();

        private readonly List<Position> _chunkScope = new List<Position>((MaxHorizontalGenerationDistance * 2) * (MaxVerticalGenerationDistance * 2) * (MaxHorizontalGenerationDistance * 2));

        // player position
        public Position PlayerPosition = new Position(0, 0, 0);

        private object _lock = new object();

        public Generator()
        {
            //ThreadPool.SetMaxThreads(8, 8);
            for (int x = -MaxHorizontalGenerationDistance; x < MaxHorizontalGenerationDistance; x++)
                for (int y = -MaxVerticalGenerationDistance; y < MaxVerticalGenerationDistance; y++)
                    for (int z = -MaxHorizontalGenerationDistance; z < MaxHorizontalGenerationDistance; z++)
                        if (Vector3.Distance(Vector3.zero, new Position(x * WorldSettings.ChunkSize, y * WorldSettings.ChunkSize, z * WorldSettings.ChunkSize).ToVector3()) < MaxHorizontalGenerationDistance * WorldSettings.ChunkSize)
                            _chunkScope.Add(new Position(x * WorldSettings.ChunkSize, y * WorldSettings.ChunkSize, z * WorldSettings.ChunkSize));

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
                while (!_aborted)
                {
                    lock (_lock)
                    {
                    Position currentPlayerPosition;

                    lock (PlayerPosition)
                    {
                        currentPlayerPosition = PlayerPosition;
                    }

                    Debug.Log("still running for " + currentPlayerPosition);


                    Position playerChunk = Helper.SnapToGrid(currentPlayerPosition);

                    foreach (Position chunkPosition in _chunkScope)
                    {
                        Position absoluteChunkPosition = new Position(playerChunk + chunkPosition);

                        bool notGenerating;

                        lock (Generating)
                        {
                            notGenerating = !Generating.Any(p => p.Equals(absoluteChunkPosition));
                        }

                        if (!ChunkExists(absoluteChunkPosition) && notGenerating)
                        {
                            GenerateChunk generateChunk = new GenerateChunk(absoluteChunkPosition, Callback);

                            ThreadPool.QueueUserWorkItem(generateChunk.Generate);

                            lock (Generating)
                            {
                                Generating.Add(absoluteChunkPosition);
                            }
                        }
                    }

                    //List<KeyValuePair<Position, ChunkData>> tmpRoRemove = new List<KeyValuePair<Position, ChunkData>>();

                    //Dictionary<Position, ChunkData> tmpChunks;
                    //lock (_chunks)
                    //{
                    //    tmpChunks = new Dictionary<Position, ChunkData>(_chunks);
                    //}

                    //foreach (KeyValuePair<Position, ChunkData> chunk in tmpChunks)
                    //{
                    //    if (Vector3.Distance(chunk.Key.ToVector3(), PlayerPosition.ToVector3()) > MaxHorizontalGenerationDistance * WorldSettings.ChunkSize)
                    //    {
                    //        lock (_toRemove)
                    //        {
                    //            _toRemove.Add(chunk.Key);
                    //        }

                    //        tmpRoRemove.Add(chunk);
                    //    }
                    //}

                    //foreach (KeyValuePair<Position, ChunkData> pair in tmpRoRemove)
                    //{
                    //    lock (_chunks)
                    //    {
                    //        _chunks.Remove(pair.Key);
                    //    }
                    //}

                    lock (_chunks)
                    {
                        _chunks = new Dictionary<Position, ChunkData>(_chunks);
                    }

                    }

                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }

        public void Callback(Position position, ChunkData chunkData)
        {
            lock (_lock)
            {
                lock (_toAdd)
                {
                    _toAdd[position] = chunkData;
                }

                lock (_chunks)
                {
                    _chunks[position] = chunkData;
                }

                lock (Generating)
                {
                    Generating.Remove(position);
                }
            }
        }

        public IEnumerable<KeyValuePair<Position, ChunkData>> GetNewChunks()
        {
            lock (_lock)
            {
                List<KeyValuePair<Position, ChunkData>> tmp;

                lock (_toAdd)
                {
                    tmp = new List<KeyValuePair<Position, ChunkData>>(_toAdd);
                    _toAdd.Clear();
                }

                return tmp;
            }
        }

        //public IEnumerable<KeyValuePair<Position, ChunkData>> GetNewChunks()
        //{
        //    for (; ; )
        //    {
        //        KeyValuePair<Position, ChunkData> toAdd;

        //        lock (_toAdd)
        //        {
        //            toAdd = _toAdd.FirstOrDefault();

        //            if (toAdd.Equals(default(KeyValuePair<Position, ChunkData>)))
        //                yield break;
        //            _toAdd.Remove(toAdd.Key);
        //        }

        //        yield return toAdd;
        //    }
        //}

        public IEnumerable<Position> GetOutOfRangeChunks()
        {
            lock (_lock)
            {
                List<Position> tmp;

                lock (_toRemove)
                {
                    tmp = new List<Position>(_toRemove);
                    _toRemove.Clear();
                }

                return tmp;
            }
        }

        private bool ChunkExists(Position position)
        {
            bool exists;

            lock (_chunks)
                exists = _chunks.ContainsKey(position);

            lock (_toAdd)
                 exists = exists || _toAdd.ContainsKey(position);

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
