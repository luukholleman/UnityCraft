using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic.Behaviours;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    class DynamicObjectComponent : MonoBehaviour
    {
        public DynamicObject DynamicObject;

        public BaseBehaviour BaseBehaviour;

        public ChunkComponent ChunkComponent;

        private MeshFilter _filter;
        private MeshCollider _coll;

        void Start()
        {
            BuildMesh();

            BaseBehaviour = DynamicObject.GetBehaviour();

            if (BaseBehaviour != null)
            {
                BaseBehaviour.Setup(this);

                BaseBehaviour.Start();
            }
        }

        public void BuildMesh()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();

            MeshData meshData = new MeshData();

            meshData = DynamicObject.GetMeshData();

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

        void Update()
        {
            if(BaseBehaviour != null)
                BaseBehaviour.Update();
        }

        public void Action()
        {
            if (BaseBehaviour != null)
                BaseBehaviour.Action();
        }

        public void Interact()
        {
            if (BaseBehaviour != null)
                BaseBehaviour.Interact();
        }

        void OnGUI()
        {
            if(BaseBehaviour != null)
                BaseBehaviour.OnGUI();
        }
    }
}
