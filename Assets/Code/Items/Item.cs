using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Blocks;
using UnityEngine;

namespace Assets.Code.Items
{
    public abstract class Item
    {
        public abstract Mesh GetMesh();

        public virtual Block GetBlock()
        {
            return null;
        }
    }
}
