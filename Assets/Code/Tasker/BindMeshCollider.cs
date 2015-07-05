using System;
using System.Collections;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Tasker
{
    class BindMeshCollider : Task
    {
        public MeshCollider MeshCollider;
        public MeshData MeshData;

        public override IEnumerator Execute(Action taskDone)
        {
            yield return null;

            Mesh mesh = new Mesh();

            mesh.vertices = MeshData.ArrColVertices;
            mesh.triangles = MeshData.ArrColTriangles;
            mesh.RecalculateNormals();

            MeshCollider.sharedMesh = mesh;

            taskDone();

            yield return null;
        }
    }
}
