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
            if (_task.Exception is not null)
                throw _task.Exception;
        }
    }
}
