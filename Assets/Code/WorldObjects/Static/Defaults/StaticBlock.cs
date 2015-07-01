using System.Collections.Generic;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.WorldObjects.Static.Defaults
{
    public abstract class StaticBlock : StaticObject
    {
        public override MeshData GetChunkMeshData(ChunkData chunk, Position position, MeshData meshData)
        {
            meshData.UseRenderDataForCol = true;

            int oriX = position.x;
            int oriY = position.y;
            int oriZ = position.z;

            Position otherPosition = new Position(position);

            otherPosition.y += 1;
            if (!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.Down))
            {
                meshData = FaceDataUp(oriX, oriY, oriZ, meshData);
            }
            otherPosition.y = oriY;

            otherPosition.y -= 1;
            if (!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.Up))
            {
                meshData = FaceDataDown(oriX, oriY, oriZ, meshData);
            }
            otherPosition.y = oriY;

            otherPosition.z += 1;
            if (!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.South))
            {
                meshData = FaceDataNorth(oriX, oriY, oriZ, meshData);
            }
            otherPosition.z = oriZ;

            otherPosition.z -= 1;
            if (!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.North))
            {
                meshData = FaceDataSouth(oriX, oriY, oriZ, meshData);
            }
            otherPosition.z = oriZ;

            otherPosition.x += 1;
            if (!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.West))
            {
                meshData = FaceDataEast(oriX, oriY, oriZ, meshData);
            }
            otherPosition.x = oriX;

            otherPosition.x -= 1;
            if (!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.East))
            {
                meshData = FaceDataWest(oriX, oriY, oriZ, meshData);
            }
            otherPosition.x = oriX;

            return meshData;
        }

        public override List<KeyValuePair<Vector3, Vector3>> GetChunkCollider(ChunkData chunk, Position position, List<KeyValuePair<Vector3, Vector3>> colliders)
        {
            int oriX = position.x;
            int oriY = position.y;
            int oriZ = position.z;

            Position otherPosition = new Position(position);

            bool borderBox = false;

            otherPosition.y += 1;
            if ((!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.Down)) && !borderBox)
            {
                borderBox = true;
            }
            otherPosition.y = oriY;

            otherPosition.y -= 1;
            if ((!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.Up)) && !borderBox)
            {
                borderBox = true;
            }
            otherPosition.y = oriY;

            otherPosition.z += 1;
            if ((!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.South)) && !borderBox)
            {
                borderBox = true;
            }
            otherPosition.z = oriZ;

            otherPosition.z -= 1;
            if ((!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.North)) && !borderBox)
            {
                borderBox = true;
            }
            otherPosition.z = oriZ;

            otherPosition.x += 1;
            if ((!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.West)) && !borderBox)
            {
                borderBox = true;
            }
            otherPosition.x = oriX;

            otherPosition.x -= 1;
            if ((!chunk.HasObjectAtPosition(otherPosition) || !chunk.GetObject(otherPosition).IsSolid(Direction.East)) && !borderBox)
            {
                borderBox = true;
            }
            otherPosition.x = oriX;

            if(borderBox)
                colliders.Add(new KeyValuePair<Vector3, Vector3>(position.ToVector3(), new Vector3(1, 1, 1)));

            return colliders;
        }
        public override MeshData GetMeshData()
        {
            MeshData meshData = new MeshData();

            meshData = FaceDataUp(0, 0, 0, meshData);
            meshData = FaceDataDown(0, 0, 0, meshData);
            meshData = FaceDataNorth(0, 0, 0, meshData);
            meshData = FaceDataSouth(0, 0, 0, meshData);
            meshData = FaceDataEast(0, 0, 0, meshData);
            meshData = FaceDataWest(0, 0, 0, meshData);

            return meshData;
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
        public override bool IsSolid(Direction direction)
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
    }
}
