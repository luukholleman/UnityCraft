using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic
{
    class Chest : DynamicObject
    {
        public override MeshData GetMeshData(ChunkComponent chunkComponent, int x, int y, int z, MeshData meshData)
        {
            meshData.UseRenderDataForCol = true;

            Position position = new Position(x, y, z);

            if (IsPossibleSolidBlock(chunkComponent, position) && !chunkComponent.GetObject(x, y + 1, z).IsSolid(Direction.Down))
            {
                meshData = FaceDataUp(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunkComponent, position) && !chunkComponent.GetObject(x, y - 1, z).IsSolid(Direction.Up))
            {
                meshData = FaceDataDown(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunkComponent, position) && !chunkComponent.GetObject(x, y, z + 1).IsSolid(Direction.South))
            {
                meshData = FaceDataNorth(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunkComponent, position) && !chunkComponent.GetObject(x, y, z - 1).IsSolid(Direction.North))
            {
                meshData = FaceDataSouth(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunkComponent, position) && !chunkComponent.GetObject(x + 1, y, z).IsSolid(Direction.West))
            {
                meshData = FaceDataEast(x, y, z, meshData);
            }

            if (IsPossibleSolidBlock(chunkComponent, position) && !chunkComponent.GetObject(x - 1, y, z).IsSolid(Direction.East))
            {
                meshData = FaceDataWest(x, y, z, meshData);
            }

            return meshData;
        }
        private bool IsPossibleSolidBlock(ChunkComponent chunkComponent, Position position)
        {
            return chunkComponent.Blocks[position.x, position.y, position.z] != null || chunkComponent.Blocks[position.x, position.y, position.z] is Air;
        }

        public override MeshData PropData(MeshData meshData)
        {
            meshData.UseRenderDataForCol = true;

            meshData = FaceDataUp(0, 0, 0, meshData);
            meshData = FaceDataDown(0, 0, 0, meshData);
            meshData = FaceDataNorth(0, 0, 0, meshData);
            meshData = FaceDataSouth(0, 0, 0, meshData);
            meshData = FaceDataEast(0, 0, 0, meshData);
            meshData = FaceDataWest(0, 0, 0, meshData);

            return meshData; 
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile();

            switch (direction)
            {
                case Direction.Up:
                    tile.x = 9;
                    tile.y = 14;
                    break;
                case Direction.Down:
                    tile.x = 9;
                    tile.y = 14;
                    break;
                default:
                    tile.x = 10;
                    tile.y = 14;
                    break;
            }

            return tile;
        }

        public override Vector2[] FaceUVs(Direction direction)
        {
            Vector2[] uvs = new Vector2[4];

            Tile tilePos = TexturePosition(direction);

            uvs[0] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y);
            uvs[1] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y + TileSize);
            uvs[2] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y + TileSize);
            uvs[3] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y);

            return uvs;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        public override Item GetItem()
        {
            return null;
        }

        protected virtual MeshData FaceDataUp(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.Up));

            return meshData;
        }
        protected virtual MeshData FaceDataDown(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.Down));

            return meshData;
        }

        protected virtual MeshData FaceDataNorth(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.North));

            return meshData;
        }

        protected virtual MeshData FaceDataEast(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.East));

            return meshData;
        }

        protected virtual MeshData FaceDataSouth(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.South));

            return meshData;
        }

        protected virtual MeshData FaceDataWest(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.West));

            return meshData;
        }
    }
}
