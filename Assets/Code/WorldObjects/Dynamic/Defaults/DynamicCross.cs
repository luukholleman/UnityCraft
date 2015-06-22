using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Defaults
{
    abstract class DynamicCross : DynamicObject
    {
        public override MeshData GetMeshData()
        {
            MeshData meshData = new MeshData();

            meshData.UseRenderDataForCol = true;

            meshData = FaceDataNorth(0, 0, 0, meshData);
            meshData = FaceDataSouth(0, 0, 0, meshData);
            meshData = FaceDataEast(0, 0, 0, meshData);
            meshData = FaceDataWest(0, 0, 0, meshData);

            return meshData;
        }
        
        protected virtual MeshData FaceDataNorth(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z)); // bottom left
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z)); // top left
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z)); // top right
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z)); // bottom right

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.North));

            return meshData;
        }

        protected virtual MeshData FaceDataEast(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x, y - 0.5f, z + 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.East));

            return meshData;
        }

        protected virtual MeshData FaceDataSouth(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.South));

            return meshData;
        }

        protected virtual MeshData FaceDataWest(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x, y - 0.5f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.West));

            return meshData;
        }
        public Vector2[] FaceUVs(Direction direction)
        {
            if (direction == Direction.Up || direction == Direction.Down)
                return new Vector2[0];

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
