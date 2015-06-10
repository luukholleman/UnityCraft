using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.SimplexNoise;
using Assets.Code.World.Chunk.Block;
using UnityEngine;

namespace Assets.Code.World.Terrain
{
    class TerrainGen
    {
        float stoneBaseHeight = -24;
        float stoneBaseNoise = 0.05f;
        float stoneBaseNoiseHeight = 4;

        float stoneMountainHeight = 48;
        float stoneMountainFrequency = 0.008f;
        float stoneMinHeight = -12;

        float dirtBaseHeight = 1;
        float dirtNoise = 0.04f;
        float dirtNoiseHeight = 3;
        
        float caveFrequency = 0.025f;
        int caveSize = 7;

        float treeFrequency = 0.2f;
        int treeDensity = 3;

        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }

        public Chunk.Chunk ChunkGen(Chunk.Chunk chunk)
        {
            for (int x = chunk.WorldPos.x - 3; x < chunk.WorldPos.x + Chunk.Chunk.ChunkSize + 3; x++)
            {
                for (int z = chunk.WorldPos.z - 3; z < chunk.WorldPos.z + Chunk.Chunk.ChunkSize + 3; z++)
                {
                    chunk = ChunkColumnGen(chunk, x, z);
                }
            }
            return chunk;
        }

        public static void SetBlock(int x, int y, int z, Chunk.Block.Block block, Chunk.Chunk chunk, bool replaceBlocks = false)
        {
            x -= chunk.WorldPos.x;
            y -= chunk.WorldPos.y;
            z -= chunk.WorldPos.z;

            if (Chunk.Chunk.InRange(x) && Chunk.Chunk.InRange(y) && Chunk.Chunk.InRange(z))
            {
                if (replaceBlocks || chunk.Blocks[x, y, z] == null)
                    chunk.SetBlock(x, y, z, block);
            }
        }

        public Chunk.Chunk ChunkColumnGen(Chunk.Chunk chunk, int x, int z)
        {
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
            stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

            if (stoneHeight < stoneMinHeight)
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);

            stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
            dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

            for (int y = chunk.WorldPos.y - 8; y < chunk.WorldPos.y + Chunk.Chunk.ChunkSize; y++)
            {
                int caveChance = GetNoise(x, y, z, caveFrequency, 100);

                if (y <= stoneHeight && caveSize < caveChance)
                {
                    SetBlock(x, y, z, new Block(), chunk);
                }
                else if (y <= dirtHeight && caveSize < caveChance)
                {
                    SetBlock(x, y, z, new BlockGrass(), chunk);

                    if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                        CreateTree(x, y + 1, z, chunk);
                }
                else
                {
                    SetBlock(x, y, z, new BlockAir(), chunk);
                }
            }

            return chunk;
        }

        void CreateTree(int x, int y, int z, Chunk.Chunk chunk)
        {
            //create leaves
            for (int xi = -2; xi <= 2; xi++)
            {
                for (int yi = 4; yi <= 8; yi++)
                {
                    for (int zi = -2; zi <= 2; zi++)
                    {
                        SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                    }
                }
            }

            //create trunk
            for (int yt = 0; yt < 6; yt++)
            {
                SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
            }
        }
    }
}
