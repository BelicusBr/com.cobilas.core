using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.Collections {
    public class ArrayToIEnumerator<T> : IEnumerator<T> {
        protected T[] list;
        protected int index;
        protected T current;
        private bool disposedValue;

        public virtual T Current => current;
        object IEnumerator.Current => (object)current;

        protected ArrayToIEnumerator() {
#if NET5_0_OR_GREATER
            list = Array.Empty<T>();
#else
            list = new T[0];
#endif
            index = -1;
        }

        public ArrayToIEnumerator(T[] list) : this()
            => this.list = list;

        public virtual bool MoveNext() {
            if (++index >= ArrayManipulation.ArrayLength(list)) return false;
            else current = list[index];
            return true;
        }

        public virtual void Reset()
            => index = -1;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    list = (T[])null;

                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
