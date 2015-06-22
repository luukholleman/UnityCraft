using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.Thread
{
    class GenerateChunkMesh : ThreadedJob
    {
        private ChunkComponent _chunkComponent;
        private WorldObject[, ,] _blocks;

        public MeshData MeshData;

        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] Uv;

        public Vector3[] ColVertices;
        public int[] ColTriangles;

        public GenerateChunkMesh(ChunkComponent chunkComponent, WorldObject[, ,] blocks)
        {
            _chunkComponent = chunkComponent;
            _blocks = blocks;

            MeshData = new MeshData();
        }

        protected override void ThreadFunction()
        {
            for (int x = 0; x < World.World.ChunkSize; x++)
            {
                for (int y = 0; y < World.World.ChunkSize; y++)
                {
                    for (int z = 0; z < World.World.ChunkSize; z++)
                    {
                        if (_blocks[x, y, z] is StaticObject)
                        {
                            StaticObject so = _blocks[x, y, z] as StaticObject;

                            MeshData = so.GetChunkMeshData(_chunkComponent, new Position(x, y, z), MeshData);
                        }
                    }
                }
            }

            Vertices = MeshData.Vertices.ToArray();
            Triangles = MeshData.Triangles.ToArray();
            Uv = MeshData.Uv.ToArray();

            ColVertices = MeshData.ColVertices.ToArray();
            ColTriangles = MeshData.ColTriangles.ToArray();
        }

        protected override void OnFinished()
        {

        }
    }
}
