using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Blocks;
using Assets.Code.Thread;
using UnityEngine;

namespace Assets.Code.World.Chunks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Chunk : MonoBehaviour
    {
        public Block[, ,] Blocks = new Block[World.ChunkSize, World.ChunkSize, World.ChunkSize];

        public bool Rebuild;

        public bool Rendered;

        private MeshFilter _filter;
        private MeshCollider _coll;

        public World World;

        public Position Position;

        private GenerateChunkMesh _chunkMeshGenerator;

        // Use this for initialization
        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();
        }
        
        void Update()
        {
            if (Rebuild)
            {
                if (_chunkMeshGenerator == null)
                {
                    _chunkMeshGenerator = new GenerateChunkMesh(this, Blocks);
                    _chunkMeshGenerator.Start();
                }
                else if (_chunkMeshGenerator.IsDone)
                {
                    RenderMesh(_chunkMeshGenerator);

                    _chunkMeshGenerator.Abort();
                    _chunkMeshGenerator = null;

                    Rendered = true;
                    Rebuild = false;
                }
            }
        }

        void OnDestroy()
        {
            //Serialization.SaveChunk(this);
        }

        public Block GetBlock(Position position)
        {
            if (Helper.InChunk(position))
            {
                return Blocks[position.x, position.y, position.z];
            }

            return World.GetBlock(Position + position);
        }

        public Block GetBlock(int x, int y, int z)
        {
            return GetBlock(new Position(x, y, z));
        }
        
        public bool SetBlock(Position position, Block block)
        {
            Position innerblockPosition = Helper.InnerChunkPosition(position);

            if (Helper.InChunk(innerblockPosition))
            {
                Blocks[innerblockPosition.x, innerblockPosition.y, innerblockPosition.z] = block;

                return true;
            }

            World.SetBlock(Position + position, block);

            return false;
        }
        
        void RenderMesh(GenerateChunkMesh generateChunkMesh)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = generateChunkMesh.Vertices;
            _filter.mesh.triangles = generateChunkMesh.Triangles;
            _filter.mesh.uv = generateChunkMesh.Uv;
            _filter.mesh.RecalculateNormals();

            _coll.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = generateChunkMesh.ColVertices;
            mesh.triangles = generateChunkMesh.ColTriangles;
            mesh.RecalculateNormals();

            _coll.sharedMesh = mesh;
        }
        
        public void SetBlocksUnmodified()
        {
            foreach (Block block in Blocks)
            {
                block.Changed = false;
            }
        }
    }
}