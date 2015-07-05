using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Tasker;
using Assets.Code.Thread;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    class Feeder : ThreadedJob
    {
        private bool _aborted = false;

        private List<KeyValuePair<Position, ChunkData>> _toAdd = new List<KeyValuePair<Position, ChunkData>>();

        public Generator Generator;

        protected override void ThreadFunction()
        {
            try
            {
                while (!_aborted)
                {
                    lock (_toAdd)
                    {
                        if (_toAdd.Any())
                        {
                            //_toAdd = _toAdd.OrderBy(w => w.Key.ManhattanDistance(Generator.PlayerPosition)).ToList();

                            KeyValuePair<Position, ChunkData> chunk = _toAdd.First();

                            Tasker.Tasker.Instance.Add(new NewChunkPrefab(chunk, World.World.Instance.CreateNewChunkCallback));

                            _toAdd.Remove(chunk);
                        }
                    }

                    System.Threading.Thread.Sleep(20);
                }

                Debug.Log("Aborted");
            }
            catch (Exception e)
            {
                Debug.Log("Feeder " + e);

                Abort();
            }
        }

        public void Add(KeyValuePair<Position, ChunkData> chunk)
        {
            lock (_toAdd)
            {
                _toAdd.Add(chunk);
            }
        }

        protected override void OnFinished()
        {
            throw new Exception("This should not be finished");
        }

        public override void Abort()
        {
            _aborted = true;

            base.Abort();
        }
    }
}
