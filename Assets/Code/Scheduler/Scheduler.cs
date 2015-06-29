using System.Collections.Generic;
using System.Linq;
using Assets.Code.GenerationEngine;
using Assets.Code.World.Chunks;
using UnityEngine;

namespace Assets.Code.Scheduler
{
    class Scheduler : MonoBehaviour
    {
        public static Scheduler Instance;

        void Start()
        {
            Instance = this;
        }

        public Queue<IScheduleTask> Tasks = new Queue<IScheduleTask>(); 

        public void Add(IScheduleTask task)
        {
            Tasks.Enqueue(task);
        }
        
        void Update()
        {
            if (!Tasks.Any())
                return;
                
            
            IScheduleTask task = Tasks.Dequeue();

            task.Execute();
        }
    }
}
