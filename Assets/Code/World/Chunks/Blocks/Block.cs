﻿using System;
using Assets.Code.Items;
using Assets.Code.Items.Blocks;
using UnityEngine;

namespace Assets.Code.World.Chunks.Blocks
{
    [Serializable]
    public class Block
    {
        public enum Direction { North, East, South, West, Up, Down };

        const float TileSize = 0.0625f;

        public struct Tile { public int x; public int y;}

        public bool Changed = true;

        public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.UseRenderDataForCol = true;

            Position position = new Position(x, y, z);

            if (IsPossibleSolidBlock(chunk, position) && !chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down))
            {
                meshData = FaceDataUp(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunk, position) && !chunk.GetBlock(x, y - 1, z).IsSolid(Direction.Up))
            {
                meshData = FaceDataDown(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunk, position) && !chunk.GetBlock(x, y, z + 1).IsSolid(Direction.South))
            {
                meshData = FaceDataNorth(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunk, position) && !chunk.GetBlock(x, y, z - 1).IsSolid(Direction.North))
            {
                meshData = FaceDataSouth(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunk, position) && !chunk.GetBlock(x + 1, y, z).IsSolid(Direction.West))
            {
                meshData = FaceDataEast(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunk, position) && !chunk.GetBlock(x - 1, y, z).IsSolid(Direction.East))
            {
                meshData = FaceDataWest(x, y, z, meshData);
            }

            return meshData;

        }

        public virtual Item GetItem()
        {
            return new StoneBlock();
        }

        private bool IsPossibleSolidBlock(Chunk chunk, Position position)
        {
            return chunk.Blocks[position.x, position.y, position.z] != null || chunk.Blocks[position.x, position.y, position.z] is BlockAir;
        }

        public MeshData PropData(MeshData meshData)
        {
                meshData = FaceDataUp(0, 0, 0, meshData);
                meshData = FaceDataDown(0, 0, 0, meshData);
                meshData = FaceDataNorth(0, 0, 0, meshData);
                meshData = FaceDataSouth(0, 0, 0, meshData);
                meshData = FaceDataEast(0, 0, 0, meshData);
                meshData = FaceDataWest(0, 0, 0, meshData);

            return meshData;;
        }

        protected virtual MeshData FaceDataUp
            (int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.Up));

            return meshData;
        }
        protected virtual MeshData FaceDataDown
            (int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.Down));

            return meshData;
        }

        protected virtual MeshData FaceDataNorth
            (int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.North));

            return meshData;
        }

        protected virtual MeshData FaceDataEast
            (int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.East));

            return meshData;
        }

        protected virtual MeshData FaceDataSouth
            (int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.South));

            return meshData;
        }

        protected virtual MeshData FaceDataWest
            (int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.West));

            return meshData;
        }

        public virtual bool IsSolid(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return true;
                case Direction.East:
                    return true;
                case Direction.South:
                    return true;
                case Direction.West:
                    return true;
                case Direction.Up:
                    return true;
                case Direction.Down:
                    return true;
            }

            return false;
        }

        public virtual Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();
            tile.x = 1;
            tile.y = 15;

            return tile;
        }
        public virtual Vector2[] FaceUVs(Direction direction)
        {
            Vector2[] uvs = new Vector2[4];

            Tile tilePos = TexturePosition(direction);

            uvs[0] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y);
            uvs[1] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y + TileSize);
            uvs[2] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y + TileSize);
            uvs[3] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y);

            return uvs;
        }

    }
}