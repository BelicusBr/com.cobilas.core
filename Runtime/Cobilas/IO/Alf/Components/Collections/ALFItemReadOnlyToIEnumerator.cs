using System.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Components.Collections {
    public sealed class ALFItemReadOnlyEnumerator : IEnumerator<ALFItemReadOnly> {
        private ALFItemReadOnly item;
        private int index;
        private object myObject;

        public ALFItemReadOnly Current => (ALFItemReadOnly)myObject;
        object IEnumerator.Current => myObject;

        public ALFItemReadOnlyEnumerator(ALFItemReadOnly item) {
            this.item = item;
            this.index = -1;
        }

        public void Dispose()
            => this.item = (ALFItemReadOnly)null;

        public bool MoveNext()
        {
            if (++index >= item.Count) return false;
            this.myObject = item[index];
            return true;
        }

        public void Reset() => this.index = -1;
    }
}
