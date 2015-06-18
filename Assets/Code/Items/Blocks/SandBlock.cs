using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.World.Chunks.Blocks;
using UnityEngine;

namespace Assets.Code.Items.Blocks
{
    class SandBlock : Item
    {
        public override Mesh GetMesh()
        {
            MeshData meshData = new MeshData();

            BlockSand block = new BlockSand();

            meshData = block.PropData(meshData);

            Mesh mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = meshData.Vertices.ToArray();
            mesh.triangles = meshData.Triangles.ToArray();
            mesh.uv = meshData.Uv.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
