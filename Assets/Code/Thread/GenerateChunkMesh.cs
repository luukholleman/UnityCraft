using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using Frankfort.Threading;
using UnityEngine;

namespace Assets.Code.Thread
{
    class GenerateChunkMesh : IThreadWorkerObject
    {
        private ChunkData _chunk;

        private Dictionary<Position, WorldObject> _blocks = new Dictionary<Position, WorldObject>();

        public MeshData MeshData;

        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] Uv;

        public Vector3[] ColVertices;
        public int[] ColTriangles;

        public MeshFilter FilterMesh;
        public MeshCollider CollMesh;

        public GenerateChunkMesh(ChunkData chunk, Dictionary<Position, WorldObject> blocks, MeshFilter filter, MeshCollider coll)
        {
            _chunk = chunk;
            _blocks = blocks;

            MeshData = new MeshData();

            FilterMesh = filter;
            CollMesh = coll;
        }
        
        public void ExecuteThreadedWork()
        {
            MeshData meshdata = new MeshData();

            foreach (KeyValuePair<Position, WorldObject> block in _blocks)
            {
                if (Helper.InChunk(block.Key) && block.Value is StaticObject)
                {
                    StaticObject so = block.Value as StaticObject;

                    meshdata = so.GetChunkMeshData(_chunk, block.Key, meshdata);
                }
            }

            Vertices = meshdata.Vertices.ToArray();
            Triangles = meshdata.Triangles.ToArray();
            Uv = meshdata.Uv.ToArray();

            ColVertices = meshdata.ColVertices.ToArray();
            ColTriangles = meshdata.ColTriangles.ToArray();
        }

        public void AbortThreadedWork()
        {

        }
    }
}
