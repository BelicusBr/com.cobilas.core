using System.Collections;
using System.Collections.Generic;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt.Components.Collections {
    public sealed class ALFBTFlagReadOnlyEnumerator : IEnumerator<IItemReadOnly> {
        private IItemReadOnly item;
        private int index;
        private object myObject;

        public IItemReadOnly Current => (IItemReadOnly)myObject;
        object IEnumerator.Current => myObject;

        public ALFBTFlagReadOnlyEnumerator(IItemReadOnly item) {
            this.item = item;
            this.index = -1;
        }

        public void Dispose()
            => this.item = (IItemReadOnly)null;

        public bool MoveNext()
        {
            if (++index >= item.Count) return false;
            this.myObject = item[index];
            return true;
        }

        public void Reset() => this.index = -1;
    }
}
