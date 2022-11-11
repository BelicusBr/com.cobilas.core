using System.Collections;
using System.Collections.Generic;

namespace Cobilas.Collections {
    public class ArrayToIEnumerator<T> : IEnumerator<T> {
        protected T[] list;
        protected int index;
        protected T current;

        public virtual T Current => current;
        object IEnumerator.Current => (object)current;

        protected ArrayToIEnumerator() {
            list = new T[0];
            index = -1;
        }

        public ArrayToIEnumerator(T[] list) : this()
            => this.list = list;

        public virtual void Dispose()
            => list = (T[])null;

        public virtual bool MoveNext() {
            if (++index >= ArrayManipulation.ArrayLength(list)) return false;
            else current = list[index];
            return true;
        }

        public virtual void Reset()
            => index = -1;
    }
}
