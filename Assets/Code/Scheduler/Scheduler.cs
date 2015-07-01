using System.Collections;
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

        [Range(1, 100)]
        public int ConcurrentTasks;

        private int _currentTasks = 0;

        private bool _busy = false;

        public object Lock = new object();

        public Queue<ScheduleTask> Tasks = new Queue<ScheduleTask>();

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            StartCoroutine("RunTasks");
        }

        public void Add(ScheduleTask task)
        {
            lock (Lock)
            {
                Tasks.Enqueue(task);
            }
        }

        //void OnGUI()
        //{
        //    UnityEngine.GUI.Label(new Rect(10, 10, 50, 50), Tasks.Count.ToString());
        //}

        public void TaskDone()
        {
            _currentTasks--;
        }

        IEnumerator RunTasks()
        {
            for (; ; )
            {
                lock (Lock)
                {
                    while (_currentTasks <= ConcurrentTasks && Tasks.Any())
                    {
                        _currentTasks++;

                        ScheduleTask task;

                        task = Tasks.Dequeue();

                        StartCoroutine(task.Execute(TaskDone));
                    }
                }

                yield return null;
                yield return null;
                yield return null;
                yield return null;
                yield return null;
            }
        }
    }
}
