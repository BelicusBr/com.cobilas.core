using System;

namespace Cobilas.Threading.Tasks {
    internal interface IAsyncTaskWait : IDisposable {
        TaskPoolItem CurrentTaskPoolItem { get; }
        /// <summary>Função assíncrona da interface <seealso cref="IAsyncTask"/>.</summary>
        void AsyncAction(InternalWait wait);
        /// <summary> Define a <seealso cref="TaskPoolItem"/> atual. </summary>
        void SetCurrentTaskPoolItem(TaskPoolItem task);
    }
}
