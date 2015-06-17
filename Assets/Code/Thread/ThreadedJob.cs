namespace Assets.Code.Thread
{
    public abstract class ThreadedJob
    {
        private bool _isDone;
        private object _handle = new object();
        private System.Threading.Thread _thread;

        public bool IsDone
        {
            get
            {
                bool tmp;
                lock (_handle)
                {
                    tmp = _isDone;
                }
                return tmp;
            }
            set
            {
                lock (_handle)
                {
                    _isDone = value;
                }
            }
        }

        public virtual void Start()
        {
            if (_thread == null && !_isDone)
            {
                _thread = new System.Threading.Thread(Run);
                _thread.Start();
            }
        }
        public virtual void Abort()
        {
            _thread.Abort();
        }

        protected abstract void ThreadFunction();

        protected abstract void OnFinished();

        public virtual bool Update()
        {
            if (IsDone)
            {
                OnFinished();
                return true;
            }
            return false;
        }
        private void Run()
        {
            ThreadFunction();
            IsDone = true;
        }
    }
}
