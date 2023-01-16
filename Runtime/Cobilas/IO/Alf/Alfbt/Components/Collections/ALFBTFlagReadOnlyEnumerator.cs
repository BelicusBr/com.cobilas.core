using System.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Alfbt.Components.Collections {
    public sealed class ALFBTFlagReadOnlyEnumerator : IEnumerator<ALFBTFlagReadOnly> {
        private ALFBTFlagReadOnly item;
        private int index;
        private object myObject;

        public ALFBTFlagReadOnly Current => (ALFBTFlagReadOnly)myObject;
        object IEnumerator.Current => myObject;

        public ALFBTFlagReadOnlyEnumerator(ALFBTFlagReadOnly item) {
            this.item = item;
            this.index = -1;
        }

        public void Dispose()
            => this.item = (ALFBTFlagReadOnly)null;

        public bool MoveNext()
        {
            if (++index >= item.Count) return false;
            this.myObject = item[index];
            return true;
        }

        public void Reset() => this.index = -1;
    }
}
