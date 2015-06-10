//MeshData.cs

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.World.Chunk
{
    public class MeshData
    {
        public List<Vector3> Vertices = new List<Vector3>();
        public List<int> Triangles = new List<int>();
        public List<Vector2> Uv = new List<Vector2>();

        public List<Vector3> ColVertices = new List<Vector3>();
        public List<int> ColTriangles = new List<int>();

        public bool UseRenderDataForCol;

        public MeshData() { }
        public void AddQuadTriangles()
        {
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);

            if (UseRenderDataForCol)
            {
                ColTriangles.Add(ColVertices.Count - 4);
                ColTriangles.Add(ColVertices.Count - 3);
                ColTriangles.Add(ColVertices.Count - 2);
                ColTriangles.Add(ColVertices.Count - 4);
                ColTriangles.Add(ColVertices.Count - 2);
                ColTriangles.Add(ColVertices.Count - 1);
            }
        }
        public void AddVertex(Vector3 vertex)
        {
            Vertices.Add(vertex);

            if (UseRenderDataForCol)
            {
                ColVertices.Add(vertex);
            }

        }
        public void AddTriangle(int tri)
        {
            Triangles.Add(tri);

            if (UseRenderDataForCol)
            {
                ColTriangles.Add(tri - (Vertices.Count - ColVertices.Count));
            }
        }

    }
}