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
    class BindMeshFilter : ScheduleTask
    {
        public MeshFilter MeshFilter;
        public MeshData MeshData;

        public override IEnumerator Execute(Action taskDone)
        {
            yield return null;

            //MeshFilter.mesh.Clear();
            MeshFilter.mesh.vertices = MeshData.ArrVertices;
            MeshFilter.mesh.triangles = MeshData.ArrTriangles;
            MeshFilter.mesh.uv = MeshData.ArrUv;
            MeshFilter.mesh.RecalculateNormals();

            taskDone();

            yield return null;
        }
    }
}
