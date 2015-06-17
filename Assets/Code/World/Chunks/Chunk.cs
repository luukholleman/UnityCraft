using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Thread;
using Assets.Code.World.Chunks.Blocks;
using UnityEngine;

namespace Assets.Code.World.Chunks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class Chunk : MonoBehaviour
    {
        public Block[, ,] Blocks = new Block[ChunkSize, ChunkSize, ChunkSize];
        public static int ChunkSize = 16;

        private bool _built;

        public bool Built
        {
            get { return _built; }
            set { _built = value; Rebuild = value; }
        }

        public bool Rebuild { get; set; }

        public bool Rendered { get; private set; }

        public List<KeyValuePair<Position, Block>> toBuild;

        //Use this for initialization
        MeshFilter _filter;
        MeshCollider _coll;

        public World World;
        public Position Position;

        private GenerateChunkMesh _chunkMeshGenerator;

        // Use this for initialization
        void Start()
        {
            _filter = gameObject.GetComponent<MeshFilter>();
            _coll = gameObject.GetComponent<MeshCollider>();

            //StartCoroutine("Fade");
        }
        IEnumerator Fade()
        {
            for (float f = 0f; f <= 1; f += 0.005f)
            {
                Color c = GetComponent<MeshRenderer>().material.color;
                c.a = f;
                c.g = f;
                c.b = f;
                GetComponent<MeshRenderer>().material.color = c;

                yield return new WaitForSeconds(1/60f);
            }
        }
        
        //Update is called once per frame
        void Update()
        {
            if (Built && Rebuild)
            {
                Rebuild = false;
                _chunkMeshGenerator = new GenerateChunkMesh(this, Blocks);
                _chunkMeshGenerator.Start();
            }

            if (_chunkMeshGenerator != null && _chunkMeshGenerator.IsDone)
            {
                RenderMesh(_chunkMeshGenerator.MeshData);

                Rendered = true;

                _chunkMeshGenerator.Abort();
                _chunkMeshGenerator = null;
            }
        }

        void OnDestroy()
        {
            Serialization.SaveChunk(this);
        }

        public Block GetBlock(Position position)
        {
            if (InRange(position.x) && InRange(position.y) && InRange(position.z))
            {
                if (Blocks[position.x, position.y, position.z] == null)
                {
                    return new BlockAir();
                }

                return Blocks[position.x, position.y, position.z];
            }

            return World.GetBlock(Position + position);
        }

        public Block GetBlock(int x, int y, int z)
        {
            return GetBlock(new Position(x, y, z));
        }

        public void FillWithPreBuiltBlocks(List<KeyValuePair<Position, Block>> blocks)
        {
            foreach (KeyValuePair<Position, Block> block in blocks)
            {
                SetBlock(block.Key, block.Value);
            }

            SetBlocksUnmodified();

            Serialization.Load(this);

            Built = true;
            Rebuild = true;
        }
        
        int Mod(int a, int n)
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }

        public Position InnerChunkPosition(Position pos)
        {
            return new Position(Mod(pos.x, ChunkSize), Mod(pos.y, ChunkSize), Mod(pos.z, ChunkSize));
        }

        public bool SetBlock(Position position, Block block)
        {
            Position innerblockPosition = InnerChunkPosition(position);

            if (InRange(innerblockPosition.x) && InRange(innerblockPosition.y) && InRange(innerblockPosition.z))
            {
                Blocks[innerblockPosition.x, innerblockPosition.y, innerblockPosition.z] = block;

                return true;
            }

            World.SetBlock(Position + position, block);

            return false;
        }
        
        // Sends the calculated mesh information
        // to the mesh and collision components
        void RenderMesh(MeshData meshData)
        {
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

        public static bool InRange(int index)
        {
            if (index < 0 || index >= ChunkSize)
                return false;

            return true;
        }

        public void SetBlocksUnmodified()
        {
            foreach (Block block in Blocks)
            {
                block.Changed = false;
            }
        }

        public override string ToString()
        {
            return "Chunk: " + Position.x + "," + Position.y + "," + Position.z;
        }
    }
}