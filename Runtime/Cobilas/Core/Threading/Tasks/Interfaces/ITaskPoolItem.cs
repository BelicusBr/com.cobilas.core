using System;

namespace Cobilas.Threading.Tasks {
    internal interface ITaskPoolItem<T> : IDisposable {
        T GetTask();
        void ConfirmCompleted(bool value);
        void ConfirmCanceled(bool value);
        void ConfirmFaulted(bool value);
        void ConfirmException(System.Exception exception);
        void Reset();
    }
}
