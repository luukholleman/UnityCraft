using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Statemachines
{
    public abstract class BaseStatemachine
    {
        protected DynamicObjectComponent DynamicObjectComponent;

        protected GameObject GO;

        public void Setup(DynamicObjectComponent dynamicObject)
        {
            DynamicObjectComponent = dynamicObject;

            GO = DynamicObjectComponent.gameObject;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void Interact();
        public abstract void Action();
        public abstract void OnGUI();
    }
}
