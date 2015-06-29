using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Scheduler
{
    class BindMeshFilter : IScheduleTask
    {
        public MeshFilter MeshFilter;
        public MeshData MeshData;

        public void Execute()
        {
            MeshFilter.mesh.Clear();
            MeshFilter.mesh.vertices = MeshData.Vertices.ToArray();
            MeshFilter.mesh.triangles = MeshData.Triangles.ToArray();
            MeshFilter.mesh.uv = MeshData.Uv.ToArray();
            MeshFilter.mesh.RecalculateNormals();
        }
    }
}
