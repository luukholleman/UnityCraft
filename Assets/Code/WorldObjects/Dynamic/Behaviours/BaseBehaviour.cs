using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.WorldObjects.Dynamic.Behaviours
{
    abstract class BaseBehaviour
    {
        protected DynamicObjectComponent DynamicObjectComponent;

        public void Setup(DynamicObjectComponent dynamicObject)
        {
            DynamicObjectComponent = dynamicObject;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void Interact();
        public abstract void Action();
        public abstract void OnGUI();
    }
}
