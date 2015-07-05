using System;
using System.Collections;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Tasker
{
    class BindMeshFilter : Task
    {
        public MeshFilter MeshFilter;
        public MeshData MeshData;

        public override IEnumerator Execute(Action taskDone)
        {
            yield return null;

            MeshFilter.mesh.Clear();
            MeshFilter.mesh.vertices = MeshData.ArrVertices;
            MeshFilter.mesh.triangles = MeshData.ArrTriangles;
            MeshFilter.mesh.uv = MeshData.ArrUv;
            MeshFilter.mesh.RecalculateNormals();

            taskDone();

            yield return null;
        }
    }
}
