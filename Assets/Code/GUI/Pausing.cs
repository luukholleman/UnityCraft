using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.GUI
{
    class Pausing : MonoBehaviour
    {
        public bool Paused = false;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                Paused = !Paused;

            if (Paused)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }
}
