using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Items.Usables
{
    abstract class BaseUsable : Item
    {
        public override ItemType Type()
        {
            return ItemType.Usable;
        }

        public override int GetStackSize()
        {
            return 10;
        }
    }
}
