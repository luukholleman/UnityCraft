using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Statemachines
{
    class ChestStatemachine : BaseStatemachine
    {
        private bool _open;

        public override void Start()
        {
            
        }

        public override void Update()
        {

        }

        public override void Interact()
        {
            _open = !_open;
        }

        public override void Action()
        {
            GameObject.Destroy(GO);
        }

        public override void OnGUI()
        {
            if(_open)
                UnityEngine.GUI.Label(new Rect(10, 10, 200, 200), "Chest open");
        }
    }
}
