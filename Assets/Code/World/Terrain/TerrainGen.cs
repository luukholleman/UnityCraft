using System;
using Assets.Code.SimplexNoise;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.World.Terrain
{
    class TerrainGen
    {
        private const float StoneBaseHeight = -24;
        private const float StoneBaseNoise = 0.05f;
        private const float StoneBaseNoiseHeight = 4;

        private const float StoneMountainHeight = 48;
        private const float StoneMountainFrequency = 0.008f;
        private const float StoneMinHeight = -12;

        private const float DirtBaseHeight = 1;
        private const float DirtNoise = 0.04f;
        private const float DirtNoiseHeight = 3;

        private const float CaveFrequency = 0.025f;
        private const int CaveSize = 7;

        private const float TreeFrequency = 0.2f;
        private const int TreeDensity = 3;

        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }

        public Chunk ChunkGen(Chunk chunk)
        {
            for (int x = chunk.WorldPos.x - 3; x < chunk.WorldPos.x + Chunk.ChunkSize + 3; x++)
            {
                for (int z = chunk.WorldPos.z - 3; z < chunk.WorldPos.z + Chunk.ChunkSize + 3; z++)
                {
                    chunk = ChunkColumnGen(chunk, x, z);
                }
            }
            return chunk;
        }

        public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
        {
            x -= chunk.WorldPos.x;
            y -= chunk.WorldPos.y;
            z -= chunk.WorldPos.z;

            if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
            {
                if (replaceBlocks || chunk.Blocks[x, y, z] == null)
                    chunk.SetBlock(x, y, z, block);
            }
        }

        public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
        {
            int stoneHeight = Mathf.FloorToInt(StoneBaseHeight);
            stoneHeight += GetNoise(x, 0, z, StoneMountainFrequency, Mathf.FloorToInt(StoneMountainHeight));

            if (stoneHeight < StoneMinHeight)
                stoneHeight = Mathf.FloorToInt(StoneMinHeight);

            stoneHeight += GetNoise(x, 0, z, StoneBaseNoise, Mathf.FloorToInt(StoneBaseNoiseHeight));

            int dirtHeight = stoneHeight + Mathf.FloorToInt(DirtBaseHeight);
            dirtHeight += GetNoise(x, 100, z, DirtNoise, Mathf.FloorToInt(DirtNoiseHeight));

            for (int y = chunk.WorldPos.y - Chunk.ChunkSize; y < chunk.WorldPos.y + Chunk.ChunkSize; y++)
            {
                int caveChance = GetNoise(x, y, z, CaveFrequency, 100);

                if (y <= stoneHeight && CaveSize < caveChance)
                {
                    SetBlock(x, y, z, new Block(), chunk);
                }
                else if (y <= dirtHeight && CaveSize < caveChance)
                {
                    SetBlock(x, y, z, new BlockGrass(), chunk);

                    if (y == dirtHeight && GetNoise(x, 0, z, TreeFrequency, 100) < TreeDensity)
                        CreateTree(x, y + 1, z, chunk);
                }
                else
                {
                    SetBlock(x, y, z, new BlockAir(), chunk);
                }
            }

            return chunk;
        }

        void CreateTree(int x, int y, int z, Chunk chunk)
        {
            int height = GetNoise(x, y, z, 2, 8);
            //create trunk
            for (int yt = 0; yt < height; yt++)
            {
                SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
            }

            float radius = GetNoise(x, y + height, z, 2, height / 2) + 3;


            //create leaves
            for (int xi = (int)Math.Floor(-radius); xi <= (int)Math.Ceiling(radius); xi++)
            {
                for (int yi = (int)Math.Floor(-radius); yi <= (int)Math.Ceiling(radius); yi++)
                {
                    for (int zi = (int)Math.Floor(-radius); zi <= (int)Math.Ceiling(radius); zi++)
                    {
                        if (Vector3.Distance(new Vector3(x, y + height, z), new Vector3(x + xi, y + height + yi, z + zi)) < radius)
                            CreateLeave(x + xi, y + height + yi, z + zi, chunk);
                    }
                }
            }
        }

        void CreateLeave(int x, int y, int z, Chunk chunk)
        {
            SetBlock(x, y, z, new BlockLeaves(), chunk, true);
        }
    }
}
