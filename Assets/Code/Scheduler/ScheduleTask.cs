using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    abstract class ScheduleTask
    {
        public Action Callback;

        public abstract IEnumerator Execute(Action taskdone);
    }
}
