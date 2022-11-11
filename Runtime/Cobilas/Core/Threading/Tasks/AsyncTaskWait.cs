using System;
using System.Threading;

namespace Cobilas.Threading.Tasks {
    /// <summary>Classe base para uma tarefa assíncrona com espera.</summary>
    public abstract class AsyncTaskWait : IAsyncTaskWait {
        private bool disposedValue;
        protected CancellationTokenSource source;
        protected bool isCompleted;
        protected bool isCanceled;
        protected TaskPoolItem mainTask;

        public CancellationToken Token => source.Token;
        public TaskPoolItem CurrentTaskPoolItem => mainTask;
        public bool IsCompleted => isCompleted;
        public bool IsCanceled => isCanceled;

        protected AsyncTaskWait() {
            source = new CancellationTokenSource();
            isCompleted =
            isCanceled =
            disposedValue = false;
        }

        ~AsyncTaskWait()
            => Dispose(disposing: false);

        public virtual void Cancel() => source?.Cancel();

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    source?.Dispose();
                    mainTask = (TaskPoolItem)null;
                    Lixeira();
                }
                disposedValue = true;
            }
        }

        void IAsyncTaskWait.AsyncAction(InternalWait wait) {
            Internal_AsyncAction(wait);
            isCompleted = !(isCanceled = Token.IsCancellationRequested);
        }

        void IAsyncTaskWait.SetCurrentTaskPoolItem(TaskPoolItem task) => mainTask = task;

        /// <summary>Responsavel por dispensar recursos.</summary>
        protected abstract void Lixeira();
        /// <summary>Função assíncrona da classe <seealso cref="AsyncTask"/>.</summary>
        protected abstract void Internal_AsyncAction(InternalWait wait);
    }
}
