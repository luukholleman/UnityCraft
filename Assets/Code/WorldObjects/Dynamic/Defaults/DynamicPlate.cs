using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Items;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Defaults
{
    abstract class DynamicPlate : DynamicObject
    {
        public override MeshData GetMeshData()
        {
            MeshData meshData = new MeshData();

            meshData.UseRenderDataForCol = true;

            meshData = FaceDataDown(0, 0, 0, meshData);

            return meshData;
        }

        protected virtual MeshData FaceDataDown(int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.49f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.49f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.49f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.49f, z - 0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs(Direction.Up));

            return meshData;
        }
        public Vector2[] FaceUVs(Direction direction)
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
