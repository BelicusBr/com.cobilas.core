using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cobilas.Threading.Tasks {
    public class TaskPoolItem : ITaskPoolItem<TaskPoolItem> {
        private Task task;
        private bool isCompleted;
        private bool isCanceled;
        private bool isFaulted;
        private Exception exception = (Exception)null;
        private readonly InternalWait wait;

        public int TaskID => task.Id;
        public TaskStatus Status => task.Status;
        public bool IsCompleted => isCompleted;
        public bool IsCanceled => isCanceled;
        public bool IsFaulted => isFaulted;
        public Exception TaskException => exception;

        public TaskPoolItem()
            => wait = new InternalWait();

        /// <summary>Inicia a <seealso cref="Task"/>.</summary>
        /// <param name="action"><seealso cref="Action"/>(<seealso cref="InternalWait"/> arg)</param>
        public void Start(Action<object> action)
            => Start(action, new CancellationToken(false));

        /// <summary>Inicia a <seealso cref="Task"/>.</summary>
        /// <param name="action"><seealso cref="Action"/>(<seealso cref="InternalWait"/> arg)</param>
        public void Start(Action<object> action, CancellationToken token) {
            task = new Task(action, wait, token);
            task.Start();
        }

        /// <summary>Continua a <seealso cref="Task"/>.</summary>
        /// <param name="action"><seealso cref="Action"/>(<seealso cref="InternalWait"/> arg)</param>
        public void Continue(Action<object> action)
            => Continue(action, new CancellationToken(false));

        /// <summary>Continua a <seealso cref="Task"/>.</summary>
        /// <param name="action"><seealso cref="Action"/>(<seealso cref="InternalWait"/> arg)</param>
        public void Continue(Action<object> action, CancellationToken token)
            => task = task.ContinueWith((t)=> {
                t.Dispose();
                action(wait);
            }, token);

        public void Dispose() {
            task?.Dispose();
            exception = (Exception)null;
            wait?.Dispose();
        }

        TaskPoolItem ITaskPoolItem<TaskPoolItem>.GetTask() => this;

        void ITaskPoolItem<TaskPoolItem>.ConfirmCompleted(bool value)
            => isCompleted = value;

        void ITaskPoolItem<TaskPoolItem>.ConfirmCanceled(bool value)
            => isCanceled = value;

        void ITaskPoolItem<TaskPoolItem>.ConfirmFaulted(bool value)
            => isFaulted = value;

        void ITaskPoolItem<TaskPoolItem>.ConfirmException(Exception exception)
            => this.exception = exception;

        void ITaskPoolItem<TaskPoolItem>.Reset() {
            isCanceled = isCompleted = isFaulted = false;
            exception = (Exception)null;
        }
    }
}
