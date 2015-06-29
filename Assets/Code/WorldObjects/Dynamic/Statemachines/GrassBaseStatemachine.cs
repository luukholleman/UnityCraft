﻿using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Statemachines
{
    class GrassBaseStatemachine : BaseStatemachine
    {
        public override void Start()
        {
        }

        public override void Update()
        {

        }

        public override void Interact()
        {

        }

        public override void Action()
        {
            DynamicObjectComponent.Chunk.RemoveObject(new Position(DynamicObjectComponent.transform.position));

            GameObject.Destroy(DynamicObjectComponent.gameObject);
        }

        public override void OnGUI()
        {
            
        }
    }
}
