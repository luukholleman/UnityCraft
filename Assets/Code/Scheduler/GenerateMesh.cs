using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Code.Scheduler
{
    class GenerateMesh : ScheduleTask
    {
        public ChunkData _chunkData;
        public MeshFilter _filter;
        public MeshCollider _collider;

        public override IEnumerator Execute(Action taskDone)
        {
            yield break;
            yield return null;

            MeshData meshdata = new MeshData();

            int i = 0;

            Dictionary<Position, StaticObject> blocks = _chunkData.GetStaticObjects();

            foreach (KeyValuePair<Position, StaticObject> block in blocks)
            {
                if (Helper.InChunk(block.Key))
                {
                    meshdata = block.Value.GetChunkMeshData(_chunkData, block.Key, meshdata);

                    if (++i % 8 == 0)
                        yield return null;
                }
            }

            Scheduler.Instance.Add(new BindMeshFilter() { MeshFilter = _filter, MeshData = meshdata });
            Scheduler.Instance.Add(new BindMeshCollider() { MeshCollider = _collider, MeshData = meshdata });

            taskDone();

            yield return null;
        }
    }
}
