using System;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Components.Collections {
    public sealed class ALFItemReadOnly : IItemReadOnly {
        private bool disposedValue;
        private readonly ALFItem root;
        private readonly ALFItemReadOnly parent;

        public int Count => root.Count;
        public string Name => root.name;
        public bool IsRoot => root.isRoot;
        public IItemReadOnly Parent => parent;
        public string Text => root.text.ToString();
        public IItemReadOnly this[string name] => this[IndexOf(name)];
        public IItemReadOnly this[int index] => new ALFItemReadOnly(root[index], this);
        object IReadOnlyArray.this[int index] => new ALFItemReadOnly(root[index], this);

        private ALFItemReadOnly(ALFItem root, ALFItemReadOnly parent) {
            this.root = root;
            this.parent = parent;
        }

        public ALFItemReadOnly(ALFItem root) : this(root, (ALFItemReadOnly)null) { }

        ~ALFItemReadOnly()
            => Dispose(disposing: false);

        public object Clone()
            => new ALFItemReadOnly(root == null ? null : (ALFItem)root.Clone());

        public bool Contains(string name)
            => IndexOf(name) >= 0;

        public int IndexOf(string name) {
            for (int I = 0; I < Count; I++)
                if (name == root[I].name)
                    return I;
            return -1;
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<IItemReadOnly> GetEnumerator()
            => new ItemReadOnlyEnumerator(this);

        public override string ToString()
            => root.ToString();

        public TypeCode GetTypeCode()
            => TypeCode.String;

        IEnumerator IEnumerable.GetEnumerator()
            => new ItemReadOnlyEnumerator(this);

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => (root as IConvertible).ToBoolean(provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => (root as IConvertible).ToChar(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => (root as IConvertible).ToSByte(provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => (root as IConvertible).ToByte(provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => (root as IConvertible).ToInt16(provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => (root as IConvertible).ToUInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => (root as IConvertible).ToInt32(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => (root as IConvertible).ToUInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => (root as IConvertible).ToInt64(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => (root as IConvertible).ToUInt64(provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => (root as IConvertible).ToSingle(provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => (root as IConvertible).ToDouble(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => (root as IConvertible).ToDecimal(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => (root as IConvertible).ToDateTime(provider);

        string IConvertible.ToString(IFormatProvider provider)
            => (root as IConvertible).ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => (root as IConvertible).ToType(conversionType, provider);

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    root.Dispose();

                disposedValue = true;
            }
        }
    }
}
