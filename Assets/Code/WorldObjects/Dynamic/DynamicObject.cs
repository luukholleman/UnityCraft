using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.WorldObjects.Dynamic.Statemachines;

namespace Assets.Code.WorldObjects.Dynamic
{
    public abstract class DynamicObject : WorldObject
    {
        public BaseStatemachine Statemachine { get; protected set; }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        public override void Action()
        {
            if(Statemachine != null)
                Statemachine.Action();
        }

        public override void Interact()
        {
            if(Statemachine != null)
                Statemachine.Interact();
        }
    }
}
