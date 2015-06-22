using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.WorldObjects.Dynamic.Behaviours
{
    class ChestBehaviour : BaseBehaviour
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

        }

        public override void OnGUI()
        {
            if(_open)
                UnityEngine.GUI.Label(new Rect(10, 10, 200, 200), "Chest open");
        }
    }
}
