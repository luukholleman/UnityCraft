using System;
using System.Collections;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Tasker
{
    class DeleteChunk : Task
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
            _chunk.GetComponent<MeshCollider>().sharedMesh = new Mesh();

            PoolManager.Despawn(_chunk.gameObject);

            taskdone();
        }
    }
}
