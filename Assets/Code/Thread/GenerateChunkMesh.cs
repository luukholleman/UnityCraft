using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Blocks;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Thread
{
    class GenerateChunkMesh : ThreadedJob
    {
        private Chunk _chunk;
        private Block[,,] _blocks;

        public MeshData MeshData;

        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] Uv;

        public Vector3[] ColVertices;
        public int[] ColTriangles;

        public GenerateChunkMesh(Chunk chunk, Block[, ,] blocks)
        {
            _chunk = chunk;
            _blocks = blocks;

            MeshData = new MeshData();
        }

        protected override void ThreadFunction()
        {
            for (int x = 0; x < World.World.ChunkSize; x++)
                for (int y = 0; y < World.World.ChunkSize; y++)
                    for (int z = 0; z < World.World.ChunkSize; z++)
                        MeshData = _blocks[x, y, z].Blockdata(_chunk, x, y, z, MeshData);

            Vertices = MeshData.Vertices.ToArray();
            Triangles = MeshData.Triangles.ToArray();
            Uv = MeshData.Uv.ToArray();

            ColVertices = MeshData.ColVertices.ToArray();
            ColTriangles = MeshData.ColTriangles.ToArray();
        }

        protected override void OnFinished()
        {
            throw new NotImplementedException();
        }
    }
}
