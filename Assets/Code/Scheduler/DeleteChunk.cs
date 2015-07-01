using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Scheduler
{
    class DeleteChunk : ScheduleTask
    {
        private Chunk _chunk;

        public DeleteChunk(Chunk gameObject)
        {
            _chunk = gameObject;
        }

        public override IEnumerator Execute(Action taskdone)
        {
            yield return null;

            _chunk.GetComponent<MeshFilter>().mesh = new Mesh();

            PoolManager.Despawn(_chunk.gameObject);

            taskdone();

            yield break;
        }
    }
}
