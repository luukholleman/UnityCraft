using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.GenerationEngine
{
    interface IScheduleTask
    {
        void Execute();
    }
}
