using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    public class DynamicObjectComponent : MonoBehaviour, Interactable
    {
        public DynamicObject DynamicObject;

        public BaseStatemachine Statemachine;

        public Position Position;

        public Chunk Chunk;

        private MeshFilter _filter;
        private MeshCollider _coll;

        void Start()
        {
            BuildMesh();

            Statemachine = DynamicObject.Statemachine;

            if (Statemachine != null)
            {
                Statemachine.Setup(this);

                Statemachine.Start();
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
            if(Statemachine != null)
                Statemachine.Update();
        }

        public void Action()
        {
            if (Statemachine != null)
                Statemachine.Action();
        }

        public void Interact()
        {
            if (Statemachine != null)
                Statemachine.Interact();
        }

        public void OnDestroy()
        {
            if (Statemachine != null)
                Statemachine.Destroy();
        }

        public void DoRebuild()
        {
            Chunk.DoRebuild();
        }
    }
}
