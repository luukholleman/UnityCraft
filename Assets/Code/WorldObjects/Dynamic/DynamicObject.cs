using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.WorldObjects.Dynamic.Statemachines;

namespace Assets.Code.WorldObjects.Dynamic
{
    public abstract class DynamicObject : WorldObject
    {
        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        public abstract BaseStatemachine GetBehaviour();
    }
}
