using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Thread;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.World.Chunks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]

    public class ChunkComponent : MonoBehaviour
    {
        public WorldObject[, ,] Blocks = new WorldObject[World.ChunkSize, World.ChunkSize, World.ChunkSize];

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

            PlaceDynamicObjects();
        }

        void PlaceDynamicObjects()
        {
            for (int x = 0; x < World.ChunkSize; x++)
            {
                for (int y = 0; y < World.ChunkSize; y++)
                {
                    for (int z = 0; z < World.ChunkSize; z++)
                    {
                        DynamicObject dynamicObject = Blocks[x,y,z] as DynamicObject;
                        if (dynamicObject != null)
                        {
                            GameObject dynObjectPrefab = Resources.Load<GameObject>("Prefabs/DynamicObject");

                            //GameObject newDynamicObject = Instantiate(dynObjectPrefab, transform.position + new Vector3(x, y, z), new Quaternion()) as GameObject;

                            GameObject newDynamicObject = PoolManager.Spawn(dynObjectPrefab);

                            newDynamicObject.transform.position = transform.position + new Vector3(x, y, z);

                            newDynamicObject.GetComponent<DynamicObjectComponent>().DynamicObject = dynamicObject;
                            newDynamicObject.GetComponent<DynamicObjectComponent>().ChunkComponent = this;
                            
                            Debug.Log("created");
                        }
                    }
                }
            }
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

        public WorldObject GetObject(Position position)
        {
            if (Helper.InChunk(position))
            {
                return Blocks[position.x, position.y, position.z];
            }

            return World.GetObject(Position + position);
        }

        public WorldObject GetObject(int x, int y, int z)
        {
            return GetObject(new Position(x, y, z));
        }

        public bool SetObject(Position position, WorldObject block)
        {
            Position innerblockPosition = Helper.InnerChunkPosition(position);

            if (Helper.InChunk(innerblockPosition))
            {
                Blocks[innerblockPosition.x, innerblockPosition.y, innerblockPosition.z] = block;

                return true;
            }

            World.SetObject(Position + position, block);

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
            foreach (WorldObject block in Blocks)
            {
                block.Changed = false;
            }
        }
    }
}