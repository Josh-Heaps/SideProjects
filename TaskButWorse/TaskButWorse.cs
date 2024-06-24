namespace TaskButWorse
{
    public enum TaskStatus
    {
        NotCreated,
        InQueue,
        InProgress,
        Completed,
        Faulted,
    }

    public class WorseTask
    {
        public TaskStatus Status { get; private set; }

        private Thread? _executionThread;

        internal Exception? Exception = null;

        public WorseTask() { }

        public WorseAwaiter GetAwaiter()
        {
            return new WorseAwaiter(this);
        }

        public void Run(Action action)
        {
            Status = TaskStatus.InQueue;

            _executionThread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Exception = ex;
                }
            });

            _executionThread.Start();
            Status = TaskStatus.InProgress;
        }

        internal void UpdateTaskStatus()
        {
            if (_executionThread?.IsAlive ?? false)
                return;

            if (Exception is not null)
                Status = TaskStatus.Faulted;
            else
                Status = TaskStatus.Completed;
        }
    }
}
