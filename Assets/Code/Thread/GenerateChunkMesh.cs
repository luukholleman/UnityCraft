﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.Tasker;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Static;
using Assets.CoherentNoise.Generation.Displacement;
using Frankfort.Threading;
using UnityEngine;

namespace Assets.Code.Thread
{
    class GenerateChunkMesh
    {
        private ChunkData _chunk;

        public MeshData MeshData;

        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] Uv;

        public Vector3[] ColVertices;
        public int[] ColTriangles;

        public MeshFilter FilterMesh;
        public MeshCollider CollMesh;

        public GenerateChunkMesh(ChunkData chunk, MeshFilter filter, MeshCollider coll)
        {
            _chunk = chunk;

            MeshData = new MeshData();

            FilterMesh = filter;
            CollMesh = coll;
        }
        
        public void Execute(object state)
        {
            MeshData meshdata = new MeshData();

            Dictionary<Position, StaticObject> blocks = _chunk.GetStaticObjects();

            foreach (KeyValuePair<Position, StaticObject> block in blocks)
            {
                if (Helper.InChunk(block.Key))
                {
                    meshdata = block.Value.GetChunkMeshData(_chunk, block.Key, meshdata);
                }
            }
            
            meshdata.Prepare();

            Tasker.Tasker.Instance.Add(new BindMeshFilter() { MeshFilter = FilterMesh, MeshData = meshdata });
            Tasker.Tasker.Instance.Add(new BindMeshCollider() { MeshCollider = CollMesh, MeshData = meshdata });
        }

        public void AbortThreadedWork()
        {

        }
    }
}
