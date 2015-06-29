using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Scheduler
{
    class BindMeshCollider : IScheduleTask
    {
        public MeshCollider MeshCollider;
        public MeshData MeshData;

        public void Execute()
        {
            Mesh mesh = new Mesh();

            mesh.vertices = MeshData.ColVertices.ToArray();
            mesh.triangles = MeshData.ColTriangles.ToArray();
            mesh.RecalculateNormals();

            MeshCollider.sharedMesh = mesh;
        }
    }
}
