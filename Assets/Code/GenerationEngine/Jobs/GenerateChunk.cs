﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Blocks;
using Assets.Code.Blocks.Plants;
using Assets.Code.Thread;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.CoherentNoise;
using Assets.CoherentNoise.Generation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.GenerationEngine.Jobs
{
    public class GenerateChunk : ThreadedJob
    {
        public Chunk Chunk;
        public Position Position;

        private MeshFilter _filter;
        private MeshCollider _collider;
        //public Block[, ,] Blocks = new Block[World.Chunks.Chunk.ChunkSize, World.Chunks.Chunk.ChunkSize, World.Chunks.Chunk.ChunkSize];
        
        private const float StoneBaseHeight = -24;
        private const float StoneBaseScale = 0.0005f;
        private const float StoneBaseNoiseHeight = 4;

        private const float MountainScale = 0.001f;
        private const float MountainHeightRange = 60;

        private const float HillScale = 0.05f;
        private const float HillHeightRange = 3;

        private const float DirtBaseHeight = 2;
        private const float DirtScale = 0.01f;
        private const float DirtHeightRange = 5;

        float treeFrequency = 0.2f;
        int treeDensity = 3;

        private static readonly CoherentNoise.Generator MainLandNoise = new GradientNoise2D(123456789);
        private static readonly Noise SimplexNoise = new Noise();
        private static readonly CoherentNoise.Generator CaveNoise = new GradientNoise(123456789);

        public MeshData MeshData = new MeshData();

        public GenerateChunk(Position position)
        {
            Position = position;
            Chunk = new Chunk(Position);
        }

        protected override void ThreadFunction()
        {
            Generate();
        }

        protected override void OnFinished()
        {

        }

        public void Generate()
        {
            for (int x = Position.x - Generator.ChunkSize; x < Position.x + Generator.ChunkSize + Generator.ChunkSize; x++)
            {
                for (int z = Position.z - Generator.ChunkSize; z < Position.z + Generator.ChunkSize + Generator.ChunkSize; z++)
                {
                    GenerateColumnInChunk(x, z);
                }
            }

            for (int x = 0; x < Generator.ChunkSize; x++)
            {
                for (int y = 0; y < Generator.ChunkSize; y++)
                {
                    for (int z = 0; z < Generator.ChunkSize; z++)
                    {
                        if (Chunk.Blocks[x, y, z] == null)
                        {
                            Chunk.Blocks[x,y,z] = new Air();
                        }
                    }
                }
            }
        }

        private static float Get2DNoise(Position position, float scale, int max)
        {
            return MainLandNoise.GetValue(position.x * scale, position.z * scale, 0) * max;
        }

        private static float GetCaveNoise(Position position, float scale, int max)
        {
            return (CaveNoise.GetValue(position.x * scale, position.y * scale, position.z * scale) + CaveNoise.GetValue(position.x * scale * 3, position.y * scale * 3, position.z * scale * 3)) * (max / 2f);
        }

        public static int GetSimpleNoise(Position position, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.Generate(position.x * scale, position.y * scale, position.z * scale) + 1f) * (max / 2f));
        }

        private void GenerateColumnInChunk(int x, int z)
        {
            Position columnPosition = new Position(x, 0, z);

            GenerateColumn(columnPosition);
        }

        private void GenerateColumn(Position columnPosition)
        {
            float stoneHeight = Mathf.FloorToInt(StoneBaseHeight);
            stoneHeight += Get2DNoise(columnPosition, StoneBaseScale, Mathf.FloorToInt(StoneBaseNoiseHeight));
            stoneHeight += Get2DNoise(columnPosition, HillScale, Mathf.FloorToInt(HillHeightRange));
            stoneHeight += Get2DNoise(columnPosition, MountainScale, Mathf.FloorToInt(MountainHeightRange));

            stoneHeight = Math.Abs(stoneHeight);

            float dirtHeight = Mathf.FloorToInt(DirtBaseHeight);
            dirtHeight += Get2DNoise(columnPosition + new Position(0, 100, 0), DirtScale, Mathf.FloorToInt(DirtHeightRange));

            stoneHeight -= dirtHeight;
            dirtHeight += stoneHeight;

            for (int y = Position.y - Generator.ChunkSize; y < Position.y + Generator.ChunkSize; y++)
            {
                Position blockPosition = new Position(columnPosition);

                blockPosition.y = y;

                if (y <= stoneHeight && GetCaveNoise(blockPosition, 0.01f, 1) >= 0.5f)
                {
                    Chunk.SetBlock(blockPosition, new Air());
                }
                else if (y <= stoneHeight)
                {
                    Chunk.SetBlock(blockPosition, new Stone());
                }
                else if (y <= dirtHeight)
                {
                    Chunk.SetBlock(blockPosition, new Earth());

                    if (Math.Abs(y - dirtHeight) < 0.5f && GetSimpleNoise(new Position(blockPosition.x, 0, blockPosition.z), treeFrequency, 100) < treeDensity)
                    {
                        CreateTree(blockPosition);
                    }
                }
            }
        }

        private void CreateTree(Position position)
        {
            int treeHeight = GetSimpleNoise(new Position(position.x, 0, position.z), 1, 4) + 10;

            for (int i = 0; i < treeHeight; i++)
            {
                Position treeblockPosition = new Position(position.x, position.y + i, position.z);

                Chunk.SetBlock(treeblockPosition, new Wood(), true);
            }

            Position treeTop = new Position(position);

            treeTop.y += treeHeight;

            int leafRadius = GetSimpleNoise(new Position(position.x, treeHeight, position.z), 1, 3) + treeHeight / 2;

            for (int x = treeTop.x - leafRadius; x < treeTop.x + leafRadius; x++)
            {
                for (int y = treeTop.y - leafRadius; y < treeTop.y + leafRadius; y++)
                {
                    for (int z = treeTop.z - leafRadius; z < treeTop.z + leafRadius; z++)
                    {
                        Position leafPosition = new Position(x, y, z);

                        if (Vector3.Distance(treeTop.ToVector3(), leafPosition.ToVector3()) < leafRadius - GetSimpleNoise(leafPosition, 1, 2))
                        {
                            Chunk.SetBlock(leafPosition, new Leaves());
                        }
                    }
                }
            }
        }
    }
}
