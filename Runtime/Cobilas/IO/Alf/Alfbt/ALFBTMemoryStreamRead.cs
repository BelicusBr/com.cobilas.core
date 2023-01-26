using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Alfbt.Flags;
using System.Collections.Generic;
using Cobilas.IO.Alf.Alfbt.Components;
using Cobilas.IO.Alf.Components.Collections;
using Cobilas.IO.Alf.Alfbt.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt {
    internal class ALFBTMemoryStreamRead : ALFBTRead {

        private bool disposedValue;
        private readonly ALFItem root;
        private readonly ALFReadSettings memory;

        public override int Count => root.Count;
        public override ALFReadSettings Settings => memory;
        public override IItemReadOnly ReadOnly => new ALFBTFlagReadOnly(root);
        public override IItemReadOnly this[int index] => new ALFBTFlagReadOnly(root[index]);

        public ALFBTMemoryStreamRead(ALFReadSettings memory) {
            this.memory = memory;
            root = ALFItem.DefaultRoot;
            Read();
        }

        public override void Dispose() {
            if (disposedValue)
                throw ALFException.GetObjectDisposedException<ALFBTMemoryStreamRead>();
            disposedValue = true;
            root.Dispose();
            memory.Dispose();
        }

        public override void Close() => memory.Close();

        public override void Flush() => memory.Flush();

        public override bool FlagExists(string name, AlfbtFlags flags) {
            switch (flags) {
                case AlfbtFlags.MarkingFlag:
                    return !GetFlag(name).ToString().Contains("\n") &&
                        name != ALFWriter.n_Version &&
                        name != ALFWriter.n_Type &&
                        name != ALFWriter.n_Encoding;
                case AlfbtFlags.TextFlag:
                    return GetFlag(name).ToString().Contains("\n") &&
                    name != ALFWriter.n_Version &&
                    name != ALFWriter.n_Type &&
                    name != ALFWriter.n_Encoding;
                case AlfbtFlags.HeaderFlag:
                    return GetFlag(name) != (ALFBTFlagReadOnly)null &&
                    (name == ALFWriter.n_Version ||
                    name == ALFWriter.n_Type ||
                    name == ALFWriter.n_Encoding);
                default: return false;
            }
        }

        public override bool FlagExists(string name) {
            foreach (ALFItem item in root)
                if (item.name == name)
                    return true;
            return false;
        }

        public override MarkingFlag[] GetAllMarkingFlags() {
            MarkingFlag[] res = null;
            foreach (var item in ReadOnly)
                if (!item.ToString().Contains("\n") && item.Name != ALFWriter.n_Version
                    && item.Name != ALFWriter.n_Encoding
                    && item.Name != ALFWriter.n_Type)
                    ArrayManipulation.Add(new MarkingFlag(item as ALFBTFlagReadOnly), ref res);
            return res;
        }

        public override TextFlag[] GetAllTextFlags() {
            TextFlag[] res = null;
            foreach (var item in ReadOnly)
                if (item.ToString().Contains("\n"))
                    ArrayManipulation.Add(new TextFlag(item as ALFBTFlagReadOnly), ref res);
            return res;
        }

        public override CommentFlag GetCommentFlag() {
            CommentFlag comment = new CommentFlag();
            foreach (var item in ReadOnly)
                if (item.Name == ALFWriter.n_Comment)
                    comment.Add(item as ALFBTFlagReadOnly);
            return comment;
        }

        public override IEnumerator<IItemReadOnly> GetEnumerator()
            => ((IEnumerable<IItemReadOnly>)ReadOnly).GetEnumerator();

        public override IItemReadOnly GetFlag(string name) {
            foreach (var item in ReadOnly)
                if (item.Name == name)
                    return item;
            return (ALFBTFlagReadOnly)null;
        }

        public override HeaderFlag GetHeaderFlag()
            => new HeaderFlag(
                GetFlag(ALFWriter.n_Version) as ALFBTFlagReadOnly,
                GetFlag(ALFWriter.n_Type) as ALFBTFlagReadOnly,
                GetFlag(ALFWriter.n_Encoding) as ALFBTFlagReadOnly
                );

        public override MarkingFlag GetMarkingFlag(string name)
            => new MarkingFlag(I_GetMarkingFlag(name) as ALFBTFlagReadOnly);

        public override TextFlag GetTextFlag(string name)
            => new TextFlag(I_GetTextFlag(name) as ALFBTFlagReadOnly);

        public override bool HeaderFlagExists(string name)
            => FlagExists(name, AlfbtFlags.HeaderFlag);

        public override bool MarkingFlagExists(string name)
            => FlagExists(name, AlfbtFlags.MarkingFlag);

        public override bool TextFlagExists(string name)
            => FlagExists(name, AlfbtFlags.TextFlag);

        protected override void Read() {
            using (CharacterCursor cursor = new CharacterCursor(memory.Read()))
                GetALFBTFlags(root, cursor);
            if (memory.RemoveEscapeOnSpecialCharacters)
                RemoveEscapeOnSpecialCharactersInALFItem(root);
        }

        private IItemReadOnly I_GetMarkingFlag(string name) {
            IItemReadOnly readOnly = GetFlag(name);
            if (readOnly != (ALFBTFlagReadOnly)null)
                if (!readOnly.ToString().Contains("\n"))
                    return readOnly;
            return (ALFBTFlagReadOnly)null;
        }

        private IItemReadOnly I_GetTextFlag(string name) {
            IItemReadOnly readOnly = GetFlag(name);
            if (readOnly != (ALFBTFlagReadOnly)null)
                if (readOnly.ToString().Contains("\n"))
                    return readOnly;
            return (ALFBTFlagReadOnly)null;
        }

        protected override void RemoveEscapeOnSpecialCharactersInALFItem(ALFItem root) {
            foreach (ALFItem item in root) {
                if (item.name == ALFWriter.n_Comment) continue;
                _ = item.text.Replace("\\\\", "\\").Replace("\\/", "/").Replace("\\*", "*");
            }
        }
    }
}
