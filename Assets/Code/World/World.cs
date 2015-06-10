using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunk.Block;
using Assets.Code.World.Terrain;
using UnityEngine;

namespace Assets.Code.World
{
    public class World : MonoBehaviour
    {
        public Dictionary<WorldPos, Chunk.Chunk> Chunks = new Dictionary<WorldPos, Chunk.Chunk>();

        public GameObject ChunkPrefab;

        public string WorldName = "world";
    
        public void CreateChunk(int x, int y, int z)
        {
            WorldPos worldPos = new WorldPos(x, y, z);

            //Instantiate the chunk at the coordinates using the chunk prefab
            GameObject newChunkObject = Instantiate(
                            ChunkPrefab, new Vector3(x, y, z),
                            Quaternion.Euler(Vector3.zero)
                        ) as GameObject;

            Chunk.Chunk newChunk = newChunkObject.GetComponent<Chunk.Chunk>();

            newChunk.WorldPos = worldPos;
            newChunk.World = this;

            //Add it to the chunks dictionary with the position as the key
            Chunks.Add(worldPos, newChunk);

            TerrainGen terrainGen = new TerrainGen();

            newChunk = terrainGen.ChunkGen(newChunk);

            newChunk.SetBlocksUnmodified();

            Serialization.Load(newChunk);
        }
        public Chunk.Chunk GetChunk(int x, int y, int z)
        {
            WorldPos pos = new WorldPos();
            float multiple = Chunk.Chunk.ChunkSize;
            pos.x = Mathf.FloorToInt(x / multiple) * Chunk.Chunk.ChunkSize;
            pos.y = Mathf.FloorToInt(y / multiple) * Chunk.Chunk.ChunkSize;
            pos.z = Mathf.FloorToInt(z / multiple) * Chunk.Chunk.ChunkSize;

            Chunk.Chunk containerChunk;

            Chunks.TryGetValue(pos, out containerChunk);

            return containerChunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Chunk.Chunk containerChunk = GetChunk(x, y, z);

            if (containerChunk != null)
            {
                Block block = containerChunk.GetBlock(
                    x - containerChunk.WorldPos.x,
                    y - containerChunk.WorldPos.y,
                    z - containerChunk.WorldPos.z);

                return block;
            }
            
            return new BlockAir();
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            Chunk.Chunk chunk = GetChunk(x, y, z);

            if (chunk != null)
            {
                chunk.SetBlock(x - chunk.WorldPos.x, y - chunk.WorldPos.y, z - chunk.WorldPos.z, block);
                chunk.update = true;

                UpdateIfEqual(x - chunk.WorldPos.x, 0, new WorldPos(x - 1, y, z));
                UpdateIfEqual(x - chunk.WorldPos.x, Chunk.Chunk.ChunkSize - 1, new WorldPos(x + 1, y, z));
                UpdateIfEqual(y - chunk.WorldPos.y, 0, new WorldPos(x, y - 1, z));
                UpdateIfEqual(y - chunk.WorldPos.y, Chunk.Chunk.ChunkSize - 1, new WorldPos(x, y + 1, z));
                UpdateIfEqual(z - chunk.WorldPos.z, 0, new WorldPos(x, y, z - 1));
                UpdateIfEqual(z - chunk.WorldPos.z, Chunk.Chunk.ChunkSize - 1, new WorldPos(x, y, z + 1));
            }
        }

        public void DestroyChunk(int x, int y, int z)
        {
            Chunk.Chunk chunk = null;
            if (Chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
            {
                Serialization.SaveChunk(chunk);
                Destroy(chunk.gameObject);
                Chunks.Remove(new WorldPos(x, y, z));
            }
        }

        void UpdateIfEqual(int value1, int value2, WorldPos pos)
        {
            if (value1 == value2)
            {
                Chunk.Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
                if (chunk != null)
                    chunk.update = true;
            }
        }
    }
}
