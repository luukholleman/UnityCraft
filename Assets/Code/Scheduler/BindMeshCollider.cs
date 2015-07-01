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
    class BindMeshCollider : ScheduleTask
    {
        public MeshCollider MeshCollider;
        public MeshData MeshData;

        public override IEnumerator Execute(Action taskDone)
        {
            yield return null;

            Mesh mesh = new Mesh();

            mesh.vertices = MeshData.ColVertices.ToArray();
            mesh.triangles = MeshData.ColTriangles.ToArray();
            mesh.RecalculateNormals();

            MeshCollider.sharedMesh = mesh;

            taskDone();

            yield return null;
        }
    }
}
