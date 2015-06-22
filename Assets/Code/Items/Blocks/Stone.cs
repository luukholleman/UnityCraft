﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using Assets.Code.WorldObjects.Static;
using UnityEngine;

namespace Assets.Code.Items.Blocks
{
    class Stone : Item
    {
        public override Mesh GetMesh()
        {
            MeshData meshData = new MeshData();
            
            WorldObjects.Static.Stone block = new WorldObjects.Static.Stone();

            meshData = block.PropData(meshData);

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
            return new WorldObjects.Static.Stone();
        }
    }
}