using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Messenger;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects;
using Assets.Code.WorldObjects.Dynamic;
using Assets.Code.WorldObjects.Dynamic.Plants;
using Assets.Code.WorldObjects.Dynamic.Statemachines;
using UnityEngine;
using Wheat = Assets.Code.WorldObjects.Dynamic.Plants.Wheat;

namespace Assets.Code.Items.Usables.Seeds
{
    class WheatSeed : BaseUsable
    {
        public override Mesh GetMesh()
        {
            MeshData meshData = new MeshData();

            meshData.AddVertex(new Vector3(-0.5f, -0.5f, -0.5f));
            meshData.AddVertex(new Vector3(-0.5f, +0.5f, -0.5f));
            meshData.AddVertex(new Vector3(+0.5f, +0.5f, -0.5f));
            meshData.AddVertex(new Vector3(+0.5f, -0.5f, -0.5f));

            meshData.AddQuadTriangles();

            meshData.Uv.AddRange(FaceUVs());

            Mesh mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = meshData.Vertices.ToArray();
            mesh.triangles = meshData.Triangles.ToArray();
            mesh.uv = meshData.Uv.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        public override bool Interact(Position position, Interactable interactable)
        {
            DynamicObjectComponent doc = interactable as DynamicObjectComponent;

            if (doc != null && doc.DynamicObject is PlowedEarth)
            {
                Wheat wheat = new Wheat();
                Position plantPosition = new Position(position) {y = position.y + 1};

                doc.Chunk.SetObject(plantPosition, wheat, true);

                return true;
            }

            return false;
        }

        public override bool DestroyOnUse()
        {
            return true;
        }

        public override bool AdjacentCast()
        {
            return false;
        }

        public Vector2[] FaceUVs()
        {
            Vector2[] uvs = new Vector2[4];

            WorldObject.Tile tilePos = new WorldObject.Tile();

            tilePos.x = 3;
            tilePos.y = 3;

            uvs[0] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y);
            uvs[1] = new Vector2(TileSize * tilePos.x + TileSize, TileSize * tilePos.y + TileSize);
            uvs[2] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y + TileSize);
            uvs[3] = new Vector2(TileSize * tilePos.x, TileSize * tilePos.y);

            return uvs;
        }
    }
}
