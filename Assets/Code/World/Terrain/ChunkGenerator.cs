using System;
using System.Collections.Generic;
using Assets.Code.SimplexNoise;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using CoherentNoise;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Generation.Voronoi;
using UnityEngine;

namespace Assets.Code.World.Terrain
{
    class ChunkGenerator
    {
        private const float MeteoriteAverageHeight = 60;
        private const float MeteoriteMaxHeight = 20;
        private const float MeteoriteMinHeight = 40;
        private const float MeteoriteStretch = 0.05f;
        private const float MeteoriteBaseNoiseHeight = 50;

        private const float RockBaseHeight = -24;
        private const float StoneBaseNoise = 0.05f;
        private const float StoneBaseNoiseHeight = 4;

        private const float MountainHeight = 48;
        private const float MountainFrequency = 0.008f;
        private const float StoneMinHeight = -12;

        private const float DirtBaseHeight = 0;
        private const float DirtNoise = 0.04f;
        private const float DirtNoiseHeight = 3;

        private const float CaveFrequency = 0.025f;
        private const int CaveSize = 7;

        private const float TreeFrequency = 0.2f;
        private const int TreeDensity = 3;

        private WorldPosition _worldPosition;

        private static readonly Generator MainLandNoise = new GradientNoise2D(123456789);
        private static readonly Generator MeteoriteNoise = new BillowNoise(123456789);
        private static readonly Generator CaveNoise = new GradientNoise(123456789);

        public List<KeyValuePair<WorldPosition, Block>> Blocks = new List<KeyValuePair<WorldPosition, Block>>();

        public ChunkGenerator(WorldPosition worldPosition)
        {
            _worldPosition = worldPosition;
        }

        public void Generate()
        {
            for (int x = _worldPosition.x; x < _worldPosition.x + Chunk.ChunkSize; x++)
            {
                for (int z = _worldPosition.z; z < _worldPosition.z + Chunk.ChunkSize; z++)
                {
                    GenerateColumnInChunk(x, z);
                }
            }
        }

        private static float GetMeteoriteNoise(WorldPosition position, float scale, int max)
        {
            return MeteoriteNoise.GetValue(position.x*scale, position.y*scale, position.z*scale) *(max/2f);
        }

        private static int GetSurfaceNoise(WorldPosition position, float scale, int max)
        {
            return Mathf.FloorToInt((Mathf.Clamp(MainLandNoise.GetValue(position.x * scale, position.z * scale, 0), -1, 1) + 1) * (max / 2f));
        }

        private static float GetCaveNoise(WorldPosition position, float scale, int max)
        {
            return CaveNoise.GetValue(position.x * scale, position.y * scale, position.z * scale) * (max / 2f);
        }

        private void SetBlock(WorldPosition position, Block block, bool replaceBlocks = false)
        {
            position -= _worldPosition;

            Blocks.Add(new KeyValuePair<WorldPosition, Block>(position, block));
        }

        private void GenerateColumnInChunk(int x, int z)
        {
            WorldPosition columnPosition = new WorldPosition(x, 0, z);

            GenerateColumn(columnPosition);
        }

        private void GenerateColumn(WorldPosition columnPosition)
        {
            int stoneHeight = Mathf.FloorToInt(RockBaseHeight);
            stoneHeight += GetSurfaceNoise(columnPosition, MountainFrequency, Mathf.FloorToInt(MountainHeight));

            stoneHeight += GetSurfaceNoise(columnPosition, StoneBaseNoise, Mathf.FloorToInt(StoneBaseNoiseHeight));

            stoneHeight = Math.Abs(stoneHeight);

            int dirtHeight = stoneHeight + Mathf.FloorToInt(DirtBaseHeight);
            dirtHeight += GetSurfaceNoise(columnPosition + new WorldPosition(0, 100, 0), DirtNoise, Mathf.FloorToInt(DirtNoiseHeight));

            for (int y = _worldPosition.y - Chunk.ChunkSize; y < _worldPosition.y + Chunk.ChunkSize; y++)
            {
                WorldPosition blockPosition = new WorldPosition(columnPosition);

                blockPosition.y = y;

                int caveChance = GetSurfaceNoise(blockPosition, CaveFrequency, 100);

                if (y <= stoneHeight && GetCaveNoise(blockPosition, 0.1f, 1) >= 0.2f)
                {
                    SetBlock(blockPosition, new BlockAir());
                }
                else if (y <= stoneHeight)
                {
                    SetBlock(blockPosition, new Block());
                }
                else if (y <= dirtHeight)
                {
                    SetBlock(blockPosition, new BlockGrass());
                }
                else if (GetMeteoriteNoise(blockPosition, 0.01f, 10) >= 8)
                {
                    SetBlock(blockPosition, new Block());
                }
                else
                {
                    SetBlock(blockPosition, new BlockAir());
                }
            }
        }

        private void CreateTree(WorldPosition position)
        {
            int height = GetSurfaceNoise(position, 2, 8);

            //create trunk
            for (int yt = 0; yt < height; yt++)
            {
                WorldPosition tmpPosition = new WorldPosition(position);

                tmpPosition.y += yt;

                SetBlock(tmpPosition, new BlockWood(), true);
            }

            WorldPosition noisePosition = new WorldPosition(position);
            noisePosition.y += height;

            float radius = GetSurfaceNoise(noisePosition, 2, height / 2) + 3;

            for (int xi = (int)Math.Floor(-radius); xi <= (int)Math.Ceiling(radius); xi++)
            {
                for (int yi = (int)Math.Floor(-radius); yi <= (int)Math.Ceiling(radius); yi++)
                {
                    for (int zi = (int)Math.Floor(-radius); zi <= (int)Math.Ceiling(radius); zi++)
                    {
                        WorldPosition leavePosition = new WorldPosition(position);

                        leavePosition.x += xi;
                        leavePosition.y += yi;
                        leavePosition.z += zi;

                        if (Vector3.Distance(new Vector3(position.x, position.y + height, position.z), leavePosition.ToVector3()) < radius)
                            CreateLeave(leavePosition);
                    }
                }
            }
        }

        private void CreateLeave(WorldPosition position)
        {
            SetBlock(position, new BlockLeaves(), true);
        }
    }
}
