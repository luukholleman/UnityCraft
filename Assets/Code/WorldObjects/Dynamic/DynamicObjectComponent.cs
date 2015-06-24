using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    class DynamicObjectComponent : MonoBehaviour
    {
        public DynamicObject DynamicObject;

        public BaseStatemachine BaseStatemachine;

        public ChunkComponent ChunkComponent;

        private MeshFilter _filter;
        private MeshCollider _coll;

        void Start()
        {
            BuildMesh();

            BaseStatemachine = DynamicObject.GetBehaviour();

            if (BaseStatemachine != null)
            {
                BaseStatemachine.Setup(this);

                BaseStatemachine.Start();
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
            if(BaseStatemachine != null)
                BaseStatemachine.Update();
        }

        public void Action()
        {
            if (BaseStatemachine != null)
                BaseStatemachine.Action();
        }

        public void Interact()
        {
            if (BaseStatemachine != null)
                BaseStatemachine.Interact();
        }

        //void OnGUI()
        //{
        //    if(BaseStatemachine != null)
        //        BaseStatemachine.OnGUI();
        //}
    }
}
