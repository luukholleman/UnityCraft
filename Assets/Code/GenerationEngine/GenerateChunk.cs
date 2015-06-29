using System;
using System.Diagnostics;
using Assets.Code.Thread;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Static;
using Assets.Code.WorldObjects.Static.Plants;
using Assets.CoherentNoise;
using Assets.CoherentNoise.Generation;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Code.GenerationEngine
{
    public class GenerateChunk : ThreadedJob
    {
        public ChunkData ChunkData;
        public Position Position;

        private const float StoneBaseHeight = -24;
        private const float StoneBaseScale = 0.0005f;
        private const float StoneBaseNoiseHeight = 4;

        private const float MountainScale = 0.001f;
        private const float MountainHeightRange = 60;

        private const float HillScale = 0.05f;
        private const float HillHeightRange = 3;

        private const float DirtBaseHeight = 2;
        private const float DirtScale = 0.01f;
        private const float DirtHeightRange = 1;

        private const float TreeFrequency = 0.2f;
        private const int TreeDensity = 3;

        private const float FlowerFrequency = 0.4f;
        private const int FlowerDensity = 3;

        private const float GrassFrequency = 0.2f;
        private const int GrassDensity = 5;

        private static readonly CoherentNoise.Generator MainLandNoise = new GradientNoise2D(123456789);
        private static readonly CoherentNoise.Generator CaveNoise = new GradientNoise(123456789);

        public MeshData MeshData = new MeshData();

        public GenerateChunk(Position position)
        {
            Position = position;
            ChunkData = new ChunkData(Position);
        }

        protected override void ThreadFunction()
        {
            try
            {
                System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;

                Generate();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

        }

        protected override void OnFinished()
        {

        }

        public void Generate()
        {
            for (int x = Position.x - World.WorldSettings.ChunkSize / 2; x < Position.x + World.WorldSettings.ChunkSize + World.WorldSettings.ChunkSize / 2; x++)
            {
                for (int z = Position.z - World.WorldSettings.ChunkSize / 2; z < Position.z + World.WorldSettings.ChunkSize + World.WorldSettings.ChunkSize / 2; z++)
                {
                    GenerateColumnInChunk(x, z);
                }
            }

            for (int x = Position.x - 1; x < Position.x + World.WorldSettings.ChunkSize + 1; x++)
            {
                for (int y = Position.y - 1; y < Position.y + World.WorldSettings.ChunkSize + 1; y++)
                {
                    for (int z = Position.z - 1; z < Position.z + World.WorldSettings.ChunkSize + 1; z++)
                    {
                        Position pos = new Position(x, y, z);
                        ChunkData.SetObject(pos, new Air());
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

            float dirtHeight = stoneHeight + Mathf.FloorToInt(DirtBaseHeight);
            dirtHeight += Get2DNoise(columnPosition + new Position(0, 100, 0), DirtScale, Mathf.FloorToInt(DirtHeightRange));
            
            for (int y = Position.y - World.WorldSettings.ChunkSize - 1; y < Position.y + World.WorldSettings.ChunkSize + 1; y++)
            {
                Position blockPosition = new Position(columnPosition) {y = y};

                if (y <= stoneHeight && GetCaveNoise(blockPosition, 0.01f, 1) >= 0.5f)
                {
                    ChunkData.SetObject(blockPosition, new Air());
                }
                else if (y <= stoneHeight)
                {
                    ChunkData.SetObject(blockPosition, new Stone());
                }
                else if (y <= dirtHeight)
                {
                    ChunkData.SetObject(blockPosition, new Earth());

                    if (Math.Abs(y - dirtHeight) < 0.5f)
                    {
                        blockPosition.y++;

                        if (GetSimpleNoise(new Position(blockPosition.x, 0, blockPosition.z), TreeFrequency, 100) < TreeDensity)
                        {
                            CreateTree(blockPosition);
                        }
                        //else if (GetSimpleNoise(new Position(blockPosition.x, 0, blockPosition.z), FlowerFrequency, 100) < FlowerDensity)
                        //{
                        //    ChunkData.SetObject(blockPosition, new Flower());
                        //}
                        //else if (GetSimpleNoise(new Position(blockPosition.x, 0, blockPosition.z), GrassFrequency, 100) < GrassDensity)
                        //{
                        //    ChunkData.SetObject(blockPosition, new Grass());
                        //}
                    }
                }
            }
        }

        private void CreateTree(Position position)
        {
            int treeHeight = GetSimpleNoise(new Position(position.x, 0, position.z), 1, 3) + 5;

            for (int i = 0; i < treeHeight; i++)
            {
                Position treeblockPosition = new Position(position.x, position.y + i, position.z);

                ChunkData.SetObject(treeblockPosition, new Wood(), true);
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
                            ChunkData.SetObject(leafPosition, new Leaves());
                        }
                    }
                }
            }
        }
    }
}
