using System.Runtime.CompilerServices;

namespace TaskButWorse
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
            while (_task.Status != TaskStatus.Completed && _task.Status != TaskStatus.Faulted) { _task.UpdateTaskStatus(); }

            continuation();
        }

        public void GetResult()
        {
            while (_task.Status != TaskStatus.Completed && _task.Status != TaskStatus.Faulted) { _task.UpdateTaskStatus(); }

            if (_task.Exception is not null)
                throw _task.Exception;
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
            while (_task.Status != TaskStatus.Completed && _task.Status != TaskStatus.Faulted) { _task.UpdateTaskStatus(); }

            continuation();
        }

        public T? GetResult()
        {
            while (_task.Status != TaskStatus.Completed && _task.Status != TaskStatus.Faulted) { _task.UpdateTaskStatus(); }

            if (_task.Exception is not null)
                throw _task.Exception;

            return _task._result;
        }
    }
}
