using System;
using System.Threading.Tasks;

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
        public bool IsComplete { get => Status == TaskStatus.Completed || Status == TaskStatus.Faulted; }

        private Thread? _executionThread;

        internal Exception? Exception = null;

        public WorseTask() { }

        public WorseAwaiter GetAwaiter()
        {
            return new WorseAwaiter(this);
        }

        public static WorseTask Run(Action action)
        {
            WorseTask task = new()
            {
                Status = TaskStatus.InQueue
            };

            task._executionThread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    task.Exception = ex;
                }
            });

            task._executionThread.Start();
            task.Status = TaskStatus.InProgress;

            return task;
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

        public static WorseTask WhenAny(params WorseTask[] tasks)
        {
            while (!tasks.Any(task => task.IsComplete))
            {
                foreach (var task in tasks)
                    task.UpdateTaskStatus();
            }

            return tasks.First(task => task.IsComplete);
        }
    }

    public class WorseTask<T>
    {
        public TaskStatus Status { get; private set; }

        public bool IsComplete { get => Status == TaskStatus.Completed || Status == TaskStatus.Faulted; }

        private Thread? _executionThread;

        internal T? _result;

        internal Exception? Exception = null;

        public WorseTask() { }

        public WorseAwaiter<T> GetAwaiter()
        {
            return new WorseAwaiter<T>(this);
        }

        public static WorseTask<T> Run(Func<T> action)
        {
            WorseTask<T> task = new()
            {
                Status = TaskStatus.InQueue
            };

            task._executionThread = new Thread(() =>
            {
                try
                {
                    task._result = action();
                }
                catch (Exception ex)
                {
                    task.Exception = ex;
                }
            });

            task._executionThread.Start();
            task.Status = TaskStatus.InProgress;

            return task;
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

        public static WorseTask<T> WhenAny(params WorseTask<T>[] tasks)
        {
            while (!tasks.Any(task => task.IsComplete))
            {
                foreach (var task in tasks)
                    task.UpdateTaskStatus();
            }

            return tasks.First(task => task.IsComplete);
        }
    }
}
