using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using UnityEngine;

namespace Assets.Code.Scheduler
{
    class SpawnColliderCube : ScheduleTask
    {
        public ChunkData Chunk;
        public List<KeyValuePair<Vector3, Vector3>> Colliders; 
        public Vector3 Position;
        public Vector3 Scale;

        private World.World _instance;

        public SpawnColliderCube()
        {
            _instance = World.World.Instance;
        }

        public override IEnumerator Execute(Action taskdone)
        {
            yield return null;

            int i = 0;

            foreach (KeyValuePair<Vector3, Vector3> collider in Colliders)
            {
                GameObject cube = PoolManager.Spawn(_instance.ColliderCube);

                cube.transform.position = Chunk.Position.ToVector3() + collider.Key;
                cube.transform.localScale = collider.Value;

                if (++i%5 == 0)
                    yield return null;
            }

            taskdone();
        }
    }
}
