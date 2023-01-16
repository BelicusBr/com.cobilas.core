using System;

namespace Cobilas.Threading.Tasks {
    internal interface IAsyncTask : IDisposable {
        /// <summary> A <seealso cref="TaskPoolItem"/> atual. </summary>
        TaskPoolItem CurrentTaskPoolItem { get; }
        /// <summary>Função assíncrona da interface <seealso cref="IAsyncTask"/>.</summary>
        void AsyncAction();
        /// <summary> Define a <seealso cref="TaskPoolItem"/> atual. </summary>
        void SetCurrentTaskPoolItem(TaskPoolItem task);
    }
}
