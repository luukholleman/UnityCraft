using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Tasker
{
    class Tasker : MonoBehaviour
    {
        public static Tasker Instance;

        [Range(1, 100)]
        public int ConcurrentTasks;

        private int _currentTasks = 0;

        private bool _busy = false;

        public object Lock = new object();

        public Queue<Task> Tasks = new Queue<Task>();

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            StartCoroutine("RunTasks");
        }

        public void Add(Task task)
        {
            lock (Lock)
            {
                Tasks.Enqueue(task);
            }
        }

        void OnGUI()
        {
            UnityEngine.GUI.Label(new Rect(10, 10, 50, 50), Tasks.Count.ToString());
        }

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

                        Task task;

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
