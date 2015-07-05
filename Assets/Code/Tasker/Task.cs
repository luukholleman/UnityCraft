using System;
using System.Collections;

namespace Assets.Code.Tasker
{
    abstract class Task
    {
        public Action Callback;

        public abstract IEnumerator Execute(Action taskdone);
    }
}
