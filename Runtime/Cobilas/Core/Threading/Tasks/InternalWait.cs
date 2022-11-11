using System;
using System.Threading.Tasks;

namespace Cobilas.Threading.Tasks {
    public sealed class InternalWait : IDisposable {
        private Task task;
        public void Delay(TimeSpan span) {
            if (task == null)
                task = new Task(() => { });
            if (task.IsCompleted) {
                task = task.ContinueWith((t) => { t.Dispose(); });
                task.Wait(span);
            } else task.Wait(span);
        }

        public void Dispose()
            => task?.Dispose();
    }
}
