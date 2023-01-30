using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cobilas.Threading.Tasks {
    public static class TaskPool {
        private static event Func<TaskPoolItem> GetTask;
        private static readonly List<TaskPoolItem> tasks = new List<TaskPoolItem>();

        /// <summary>Numero de <seealso cref="Task"/> abertas.</summary>
        public static int PoolCount => tasks.Count;
        /// <summary>Numero de <seealso cref="Task"/> Concluidas.</summary>
        public static int CountTaskCompleted => I_CountTaskCompleted();

        /// <summary>
        /// Adiciona uma nova <seealso cref="Task"/> ou reutiliza um <seealso cref="Task"/> já aberta.
        /// </summary>
        public static void AddTask(Action<InternalWait> action)
            => AddTask(action, new CancellationToken(false));

        /// <summary>
        /// Adiciona uma nova <seealso cref="Task"/> ou reutiliza um <seealso cref="Task"/> já aberta.
        /// </summary>
        public static void AddTask(AsyncTaskWait task) {
            (task as IAsyncTaskWait).SetCurrentTaskPoolItem(
                I_AddTask((task as IAsyncTaskWait).AsyncAction, task.Token)
                );
        }

        /// <summary>
        /// Adiciona uma nova <seealso cref="Task"/> ou reutiliza um <seealso cref="Task"/> já aberta.
        /// </summary>
        public static void AddTask(AsyncTask task) {
            (task as IAsyncTask).SetCurrentTaskPoolItem(
                I_AddTask((task as IAsyncTask).AsyncAction, task.Token)
                );
        }

        /// <summary>
        /// Adiciona uma nova <seealso cref="Task"/> ou reutiliza um <seealso cref="Task"/> já aberta.
        /// </summary>
        public static void AddTask(Action action)
            => AddTask(action, new CancellationToken(false));

        /// <summary>
        /// Adiciona uma nova <seealso cref="Task"/> ou reutiliza um <seealso cref="Task"/> já aberta.
        /// </summary>
        public static void AddTask(Action action, CancellationToken token)
            => I_AddTask(action, token);

        /// <summary>
        /// Adiciona uma nova <seealso cref="Task"/> ou reutiliza um <seealso cref="Task"/> já aberta.
        /// </summary>
        public static void AddTask(Action<InternalWait> action, CancellationToken token)
            => I_AddTask(action, token);

        public static void Clear() {
            tasks.ForEach((t) => { t.Dispose(); });
            tasks.Clear();
        }

        private static TaskPoolItem I_AddTask(Action action, CancellationToken token)
            => I_AddTask((i) => { action(); }, token);

        // Adiciona uma nova Task ou reutiliza um Task já aberta.
        private static TaskPoolItem I_AddTask(Action<InternalWait> action, CancellationToken token) {
            TaskPoolItem temp = GetTaskCompleted();
            if (temp != null) {
                (temp as ITaskPoolItem<TaskPoolItem>).Reset();
                temp.Continue((t) => {
                    GetTask -= (temp as ITaskPoolItem<TaskPoolItem>).GetTask;
                    SafeActionExecution(action, (InternalWait)t, token, temp);
                    GetTask += (temp as ITaskPoolItem<TaskPoolItem>).GetTask;
                }, token);
            } else {
                temp = new TaskPoolItem();
                temp.Start((t) => {
                    SafeActionExecution(action, (InternalWait)t, token, temp);
                    GetTask += (temp as ITaskPoolItem<TaskPoolItem>).GetTask;
                }, token);
                tasks.Add(temp);
            }
            return temp;
        }

        // Numero de Task Concluidas.
        private static int I_CountTaskCompleted() {
            int Res = 0;
            tasks.ForEach((t) => { Res += t.IsCompleted ? 1 : 0; });
            return Res;
        }

        //Executa uma ação de forma segura.
        private static void SafeActionExecution(Action<InternalWait> action, InternalWait wait, CancellationToken token, ITaskPoolItem<TaskPoolItem> task) {
            try {
                action(wait);
                if (token.IsCancellationRequested)
                    task.ConfirmCanceled(true);
                else task.ConfirmCompleted(true);
            } catch (Exception e) {
                task.ConfirmFaulted(true);
                task.ConfirmException(e);
                Console.WriteLine(e);
            }
        }

        //Busca uma task concluida.
        private static TaskPoolItem GetTaskCompleted()
            => GetTask?.Invoke();
    }
}
