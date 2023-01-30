using Cobilas.IO.Alf.Components;
using System.Collections.Generic;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf {
    internal class ALFMemoryStreamRead : ALFRead {
        private bool disposedValue;
        private readonly ALFItem root;
        private readonly ALFReadSettings memory;

        public override int Count => root.Count;
        public override ALFReadSettings Settings => memory;
        public override IItemReadOnly ReadOnly => new ALFItemReadOnly(root);
        public override IItemReadOnly this[int index] => new ALFItemReadOnly(root.itens[index]);

        internal ALFMemoryStreamRead(ALFReadSettings memory) {
            this.memory = memory;
            this.root = ALFItem.DefaultRoot;
            Read();
        }

        public override void Dispose() {
            if (disposedValue)
                throw ALFException.GetALFException(1000, GetType());
            disposedValue = true;
            root.Dispose();
            memory.Dispose();
        }

        public override void Close() => memory.Close();

        public override void Flush() => memory.Flush();

        public override IEnumerator<IItemReadOnly> GetEnumerator()
            => ReadOnly.GetEnumerator();

        protected override void Read() {
            using (CharacterCursor cursor = new CharacterCursor(memory.Read()))
                GetAlfFlag(root, cursor);
            if (memory.RemoveEscapeOnSpecialCharacters)
                RemoveEscapeOnSpecialCharactersInALFItem(root);
        }

        protected override void RemoveEscapeOnSpecialCharactersInALFItem(ALFItem root) {
            foreach (ALFItem item in root) {
                if (item.name == ALFUtility.n_Comment) continue;
                _ = item.text.Replace("\\\\", "\\").Replace("\\:", ":").Replace("\\[", "[")
                    .Replace("\\]", "]").Replace("\\<", "<").Replace("\\>", ">");
                RemoveEscapeOnSpecialCharactersInALFItem(item);
            }
        }
    }
}
