using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Blocks;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Items.Plants
{
    class Wood : Item
    {
        public override Mesh GetMesh()
        {
            MeshData meshData = new MeshData();

            Code.Blocks.Plants.Wood block = new Code.Blocks.Plants.Wood();

            meshData = block.PropData(meshData);

            Mesh mesh = new Mesh();

            mesh.Clear();
            mesh.vertices = meshData.Vertices.ToArray();
            mesh.triangles = meshData.Triangles.ToArray();
            mesh.uv = meshData.Uv.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        public override Block GetBlock()
        {
            return new Code.Blocks.Plants.Wood();
        }
    }
}
