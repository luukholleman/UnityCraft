using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    class DynamicObjectComponent : MonoBehaviour
    {
        public DynamicObject DynamicObject;

        public ChunkComponent ChunkComponent;

        private MeshFilter _filter;
        private MeshCollider _coll;

        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();

            MeshData meshData = new MeshData();

            Position pos = Helper.InnerChunkPosition(new Position((int) transform.position.x, (int) transform.position.y, (int) transform.position.z));

            meshData = DynamicObject.PropData(meshData);

            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.Vertices.ToArray();
            _filter.mesh.triangles = meshData.Triangles.ToArray();
            _filter.mesh.uv = meshData.Uv.ToArray();
            _filter.mesh.RecalculateNormals();

            _coll.sharedMesh = null;
            
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.ColVertices.ToArray();
            mesh.triangles = meshData.ColTriangles.ToArray();
            mesh.RecalculateNormals();

            _coll.sharedMesh = mesh;
        }

    }
}
