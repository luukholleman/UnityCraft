using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.Items.Blocks
{
    class Wood : BaseBlock
    {
        public override Mesh GetMesh()
        {
            WorldObjects.Static.Plants.Wood block = new WorldObjects.Static.Plants.Wood();

            MeshData meshData = block.GetMeshData();

            Mesh mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = meshData.Vertices.ToArray();
            mesh.triangles = meshData.Triangles.ToArray();
            mesh.uv = meshData.Uv.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        public override StaticObject GetBlock()
        {
            return new WorldObjects.Static.Plants.Wood();
        }

        public override bool DestroyOnUse()
        {
            return true;
        }
    }
}
