using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Items.Tools
{
    abstract class BaseTool : Item
    {
        public override ItemType Type()
        {
            return ItemType.Tool;
        }

        public override int GetStackSize()
        {
            return 100;
        }

        public override bool AdjacentCast()
        {
            return false;
        }
    }
}
