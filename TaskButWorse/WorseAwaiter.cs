using System.Runtime.CompilerServices;

namespace WorseTask
{
    public class WorseAwaiter : INotifyCompletion
    {
        public bool IsCompleted { get; private set; }
        private readonly WorseTask _task;

        internal WorseAwaiter(WorseTask task)
        {
            _task = task;
        }

        public void OnCompleted(Action continuation)
        {
            while (!_task.IsComplete) { _task.UpdateTaskStatus(); }

            continuation();
        }

        public void GetResult()
        {
            while (!_task.IsComplete) { _task.UpdateTaskStatus(); }

            if (_task.TaskException is not null)
                throw _task.TaskException;
        }
    }

    public class WorseAwaiter<T> : INotifyCompletion
    {
        public bool IsCompleted { get; private set; }
        private readonly WorseTask<T> _task;

        internal WorseAwaiter(WorseTask<T> task)
        {
            _task = task;
        }

        public void OnCompleted(Action continuation)
        {
            while (!_task.IsComplete) { _task.UpdateTaskStatus(); }

            continuation();
        }

        public T? GetResult()
        {
            while (!_task.IsComplete) { _task.UpdateTaskStatus(); }

            if (_task.TaskException is not null)
                throw _task.TaskException;

            return _task._result;
        }
    }
}
