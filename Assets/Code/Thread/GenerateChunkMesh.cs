using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.GenerationEngine;
using Assets.Code.Scheduler;
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

            int i = 0;

            Dictionary<Position, StaticObject> blocks = _chunk.GetStaticObjects();

            foreach (KeyValuePair<Position, StaticObject> block in blocks)
            {
                if (Helper.InChunk(block.Key))
                {
                    meshdata = block.Value.GetChunkMeshData(_chunk, block.Key, meshdata);
                }
            }
            

            //MeshData meshdata = new MeshData();

            //foreach (KeyValuePair<Position, WorldObject> block in _blocks)
            //{
            //    if (Helper.InChunk(block.Key) && block.Value is StaticObject)
            //    {
            //        StaticObject so = block.Value as StaticObject;

            //        meshdata = so.GetChunkMeshData(_chunk, block.Key, meshdata);
            //    }
            //}

            Vertices = meshdata.Vertices.ToArray();
            Triangles = meshdata.Triangles.ToArray();
            Uv = meshdata.Uv.ToArray();

            meshdata.Prepare();

            //ColVertices = meshdata.ColVertices.ToArray();
            //ColTriangles = meshdata.ColTriangles.ToArray();

            Scheduler.Scheduler.Instance.Add(new BindMeshFilter() { MeshFilter = FilterMesh, MeshData = meshdata });
            //Scheduler.Scheduler.Instance.Add(new SpawnColliderCube() { Chunk = _chunk, Colliders = Colliders });

            Scheduler.Scheduler.Instance.Add(new BindMeshCollider() { MeshCollider = CollMesh, MeshData = meshdata });
        }

        public void AbortThreadedWork()
        {

        }
    }
}
