using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    public class Chunk
    {
        public Block[, ,] Blocks = new Block[Generator.ChunkSize, Generator.ChunkSize, Generator.ChunkSize];

        public List<KeyValuePair<Position, Block>> BeyondChunkBlocks = new List<KeyValuePair<Position, Block>>(); 


        private MeshFilter _filter;
        private MeshCollider _collider;
        //public Block[, ,] Blocks = new Block[World.Chunks.Chunk.ChunkSize, World.Chunks.Chunk.ChunkSize, World.Chunks.Chunk.ChunkSize];

        public Position Position;

        private const float RockBaseHeight = -24;
        private const float StoneBaseNoise = 0.05f;
        private const float StoneBaseNoiseHeight = 4;

        private const float MountainHeight = 48;
        private const float MountainFrequency = 0.008f;
        private const float StoneMinHeight = -12;

        private const float DirtBaseHeight = 0;
        private const float DirtNoise = 0.04f;
        private const float DirtNoiseHeight = 3;

        private static readonly CoherentNoise.Generator MainLandNoise = new GradientNoise2D(123456789);
        private static readonly CoherentNoise.Generator MeteoriteNoise = new BillowNoise(123456789);
        private static readonly CoherentNoise.Generator CaveNoise = new GradientNoise(123456789);

        public MeshData MeshData = new MeshData();

        public Chunk(Position position)
        {
            Position = position;
        }

        public void Generate()
        {
            for (int x = Position.x; x < Position.x + Generator.ChunkSize; x++)
            {
                for (int z = Position.z; z < Position.z + Generator.ChunkSize; z++)
                {
                    GenerateColumnInChunk(x, z);
                }
            }
        }
        private void SetBlock(Position position, Block block, bool replaceBlocks = false)
        {
            position -= Position;

            if (Helper.InChunk(position))
            {
                Blocks[position.x, position.y, position.z] = block;
            }
            else
            {
                BeyondChunkBlocks.Add(new KeyValuePair<Position, Block>(position, block));
            }
        }

        private static float GetMeteoriteNoise(Position position, float scale, int max)
        {
            return MeteoriteNoise.GetValue(position.x * scale, position.y * scale, position.z * scale) * (max / 2f);
        }

        private static int GetSurfaceNoise(Position position, float scale, int max)
        {
            return Mathf.FloorToInt((Mathf.Clamp(MainLandNoise.GetValue(position.x * scale, position.z * scale, 0), -1, 1) + 1) * (max / 2f));
        }

        private static float GetCaveNoise(Position position, float scale, int max)
        {
            return CaveNoise.GetValue(position.x * scale, position.y * scale, position.z * scale) * (max / 2f);
        }

        private void GenerateColumnInChunk(int x, int z)
        {
            Position columnPosition = new Position(x, 0, z);

            GenerateColumn(columnPosition);
        }

        private void GenerateColumn(Position columnPosition)
        {
            int stoneHeight = Mathf.FloorToInt(RockBaseHeight);
            stoneHeight += GetSurfaceNoise(columnPosition, MountainFrequency, Mathf.FloorToInt(MountainHeight));

            stoneHeight += GetSurfaceNoise(columnPosition, StoneBaseNoise, Mathf.FloorToInt(StoneBaseNoiseHeight));

            stoneHeight = Math.Abs(stoneHeight);

            int dirtHeight = stoneHeight + Mathf.FloorToInt(DirtBaseHeight);
            dirtHeight += GetSurfaceNoise(columnPosition + new Position(0, 100, 0), DirtNoise, Mathf.FloorToInt(DirtNoiseHeight));

            for (int y = Position.y; y < Position.y + Generator.ChunkSize; y++)
            {
                Position blockPosition = new Position(columnPosition);

                blockPosition.y = y;

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
    }
}
