using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.WorldObjects.Dynamic.Defaults;

namespace Assets.Code.WorldObjects.Dynamic.Plants
{
    abstract class BasePlant : DynamicCross
    {
        public int GrowLevel { get; set; }
    }
}
