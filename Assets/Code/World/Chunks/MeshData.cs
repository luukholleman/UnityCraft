//MeshData.cs

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.World.Chunks
{
    public class MeshData
    {
        public List<Vector3> Vertices = new List<Vector3>();
        public List<int> Triangles = new List<int>();
        public List<Vector2> Uv = new List<Vector2>();

        public List<Vector3> ColVertices = new List<Vector3>();
        public List<int> ColTriangles = new List<int>();
        
        public Vector3[] ArrVertices;
        public int[] ArrTriangles;
        public Vector2[] ArrUv;

        public Vector3[] ArrColVertices;
        public int[] ArrColTriangles;

        public bool UseRenderDataForCol;

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

        public void Optimize()
        {
            
        }

        public void Prepare()
        {
            ArrVertices = Vertices.ToArray();
            ArrTriangles = Triangles.ToArray();
            ArrUv = Uv.ToArray();

            ArrColVertices = ColVertices.ToArray();
            ArrColTriangles = ColTriangles.ToArray();
        }
    }
}