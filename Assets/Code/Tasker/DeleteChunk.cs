using System;
using System.Collections;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Tasker
{
    class DeleteChunk : Task
    {
        private Position _position;

        public DeleteChunk(Position gameObject)
        {
            _position = gameObject;
        }

        public override IEnumerator Execute(Action taskdone)
        {
            yield return null;

            Chunk chunk = World.World.Instance.GetChunk(_position);

            if (chunk != null)
            {
                chunk.GetComponent<MeshFilter>().mesh = new Mesh();
                chunk.GetComponent<MeshCollider>().sharedMesh = new Mesh();

                PoolManager.Despawn(chunk.gameObject);

                World.World.Instance.Chunks.Remove(_position);
            }

            taskdone();
        }
    }
}
