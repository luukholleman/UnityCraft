using System;
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

        private const float DirtBaseHeight = 1;
        private const float DirtNoise = 0.04f;
        private const float DirtNoiseHeight = 3;

        private const float CaveFrequency = 0.025f;
        private const int CaveSize = 7;

        private const float TreeFrequency = 0.2f;
        private const int TreeDensity = 3;

        private Chunk _chunk;

        private static readonly Generator MainLandNoise = new GradientNoise2D(123456789);
        private static readonly Generator MeteoriteNoise = new BillowNoise(123456789);

        public ChunkGenerator(Chunk chunk)
        {
            _chunk = chunk;
        }

        public void FillChunk()
        {
            for (int x = _chunk.WorldPosition.x; x < _chunk.WorldPosition.x + Chunk.ChunkSize; x++)
            {
                for (int z = _chunk.WorldPosition.z; z < _chunk.WorldPosition.z + Chunk.ChunkSize; z++)
                {
                    GenerateColumnInChunk(x, z);
                }
            }

            _chunk.SetBlocksUnmodified();

            Serialization.Load(_chunk);

            _chunk.Built = true;
        }

        private static float GetMeteoriteNoise(WorldPosition position, float scale, int max)
        {
            //return Mathf.FloorToInt((_mainLandNoise.GetValue(position.x * scale, position.y * scale, position.z * scale) + _meteoriteNoise.GetValue(position.x * scale, position.y * scale, position.z * scale)) * (max / 4f));           
            //return Mathf.FloorToInt(_meteoriteNoise.GetValue(position.x * scale, position.y * scale, position.z * scale) * (max / 4f));
            return MeteoriteNoise.GetValue(position.x*scale, position.y*scale, position.z*scale) *(max/2f);
        }

        private static int GetPerlinNoise(WorldPosition position, float scale, int max)
        {
            //return Mathf.FloorToInt((Noise.Generate(position.x * scale, position.y * scale, position.z * scale) + 1f) * (max / 2f));
            //return  Mathf.FloorToInt(Mathf.PerlinNoise(position.x*scale, position.z*scale)*(max/2f));
            return Mathf.FloorToInt((Mathf.Clamp(MainLandNoise.GetValue(position.x * scale, position.z * scale, 0), -1, 1) + 1) * (max / 2f));
        }

        private void SetBlock(WorldPosition position, Block block, bool replaceBlocks = false)
        {
            position -= _chunk.WorldPosition;

            if (Chunk.InRange(position.x) && Chunk.InRange(position.y) && Chunk.InRange(position.z))
            {
                if (replaceBlocks || _chunk.Blocks[position.x, position.y, position.z] == null)
                    _chunk.SetBlock(position, block);
            }
        }

        private void GenerateColumnInChunk(int x, int z)
        {
            WorldPosition columnPosition = new WorldPosition(x, 0, z);

            GenerateMainLandColumn(columnPosition);
            //GenerateMeteoriteColumn(columnPosition);

        }

        public void GenerateMeteoriteColumn(WorldPosition columnPosition)
        {
            for (int y = _chunk.WorldPosition.y - Chunk.ChunkSize; y < _chunk.WorldPosition.y + Chunk.ChunkSize; y++)
            {
                WorldPosition blockPosition = new WorldPosition(columnPosition);

                blockPosition.y = y;
                float val = GetMeteoriteNoise(blockPosition, 0.004f, 10);

                if (val >= 2)
                {
                    SetBlock(blockPosition, new Block());
                }
                else
                {
                    SetBlock(blockPosition, new BlockAir());
                }
            }
            return;
            //int meteoriteHeight = Mathf.FloorToInt(MeteoriteAverageHeight);
            //meteoriteHeight += GetMeteoriteNoise(columnPosition, MeteoriteStretch, Mathf.FloorToInt(MeteoriteMaxHeight));

            //if (meteoriteHeight < MeteoriteMinHeight)
            //    meteoriteHeight = Mathf.FloorToInt(MeteoriteMinHeight);

            //meteoriteHeight += GetMeteoriteNoise(columnPosition, MeteoriteStretch, Mathf.FloorToInt(MeteoriteMaxHeight));

            //for (int y = _chunk.WorldPosition.y - Chunk.ChunkSize; y < _chunk.WorldPosition.y + Chunk.ChunkSize; y++)
            //{
            //    WorldPosition blockPosition = new WorldPosition(columnPosition);

            //    blockPosition.y = y;

            //    if (y <= meteoriteHeight && _chunk.WorldPosition.y > y)
            //    {
            //        SetBlock(blockPosition, new Block());
            //    }
            //    else
            //    {
            //        SetBlock(blockPosition, new BlockAir());
            //    }
            //}



            //for (int y = _chunk.WorldPosition.y - Chunk.ChunkSize; y < _chunk.WorldPosition.y + Chunk.ChunkSize; y++)
            //{
            //    WorldPosition blockPosition = new WorldPosition(columnPosition);

            //    blockPosition.y = y;
            //    float val = GetMeteoriteNoise(blockPosition, 2f, 0);
            //    if (val >= 0)
            //    {
            //        SetBlock(blockPosition, new Block());
            //    }
            //    else
            //    {
            //        SetBlock(blockPosition, new BlockAir());
            //    }
            //}
        }

        private void GenerateMainLandColumn(WorldPosition columnPosition)
        {
            int stoneHeight = Mathf.FloorToInt(RockBaseHeight);
            stoneHeight += GetPerlinNoise(columnPosition, MountainFrequency, Mathf.FloorToInt(MountainHeight));

            //if (stoneHeight < StoneMinHeight)
            //    stoneHeight = Mathf.FloorToInt(StoneMinHeight);

            stoneHeight += GetPerlinNoise(columnPosition, StoneBaseNoise, Mathf.FloorToInt(StoneBaseNoiseHeight));

            stoneHeight = Math.Abs(stoneHeight);

            int dirtHeight = stoneHeight + Mathf.FloorToInt(DirtBaseHeight);
            dirtHeight += GetPerlinNoise(columnPosition + new WorldPosition(0, 100, 0), DirtNoise, Mathf.FloorToInt(DirtNoiseHeight));

            if (columnPosition.x == 17 && columnPosition.z == 13)
            {
                Debug.Log(stoneHeight);
            }
            if (columnPosition.x == 32 && columnPosition.z == 16)
            {
                Debug.Log(stoneHeight);
            }

            for (int y = _chunk.WorldPosition.y - Chunk.ChunkSize; y < _chunk.WorldPosition.y + Chunk.ChunkSize; y++)
            {
                WorldPosition blockPosition = new WorldPosition(columnPosition);

                blockPosition.y = y;

                int caveChance = GetPerlinNoise(blockPosition, CaveFrequency, 100);

                if (y <= stoneHeight)
                {
                    SetBlock(blockPosition, new Block());
                }
                else if (y <= dirtHeight)
                {
                    SetBlock(blockPosition, new BlockGrass());
                }
                else if (GetMeteoriteNoise(blockPosition, 0.01f, 10) >= 5)
                {
                    SetBlock(blockPosition, new Block());
                }
                else
                {
                    SetBlock(blockPosition, new BlockAir());
                }
            }
        }
        //private void GenerateColumnInChunk(int x, int z)
        //{
        //    WorldPosition columnPosition = new WorldPosition(x, 0, z);

        //    int stoneHeight = Mathf.FloorToInt(RockBaseHeight);
        //    stoneHeight += GetMeteoriteNoise(columnPosition, MountainFrequency, Mathf.FloorToInt(MountainHeight));

        //    if (stoneHeight < StoneMinHeight)
        //        stoneHeight = Mathf.FloorToInt(StoneMinHeight);

        //    stoneHeight += GetMeteoriteNoise(columnPosition, StoneBaseNoise, Mathf.FloorToInt(StoneBaseNoiseHeight));

        //    int dirtHeight = stoneHeight + Mathf.FloorToInt(DirtBaseHeight);
        //    dirtHeight += GetMeteoriteNoise(columnPosition + new WorldPosition(0, 100, 0), DirtNoise, Mathf.FloorToInt(DirtNoiseHeight));

        //    for (int y = _chunk.WorldPosition.y - Chunk.ChunkSize; y < _chunk.WorldPosition.y + Chunk.ChunkSize; y++)
        //    {
        //        WorldPosition blockPosition = new WorldPosition(x, y, z);

        //        int caveChance = GetMeteoriteNoise(blockPosition, CaveFrequency, 100);

        //        if (y <= stoneHeight && CaveSize < caveChance)
        //        {
        //            SetBlock(blockPosition, new Block());
        //        }
        //        else if (y <= dirtHeight && CaveSize < caveChance)
        //        {
        //            SetBlock(blockPosition, new BlockGrass());

        //            if (y == dirtHeight && GetMeteoriteNoise(columnPosition, TreeFrequency, 100) < TreeDensity)
        //            {
        //                WorldPosition treePosition = new WorldPosition(blockPosition);

        //                //treePosition.y += 1;
        //                //CreateTree(treePosition);
        //            }
        //        }
        //        else
        //        {
        //            SetBlock(blockPosition, new BlockAir());
        //        }
        //    }
        //}

        private void CreateTree(WorldPosition position)
        {
            int height = GetPerlinNoise(position, 2, 8);

            //create trunk
            for (int yt = 0; yt < height; yt++)
            {
                WorldPosition tmpPosition = new WorldPosition(position);

                tmpPosition.y += yt;

                SetBlock(tmpPosition, new BlockWood(), true);
            }

            WorldPosition noisePosition = new WorldPosition(position);
            noisePosition.y += height;

            float radius = GetPerlinNoise(noisePosition, 2, height / 2) + 3;

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
