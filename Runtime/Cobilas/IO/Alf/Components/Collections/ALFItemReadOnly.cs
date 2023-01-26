using System;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Components.Collections {
    public sealed class ALFItemReadOnly : IItemReadOnly {
        private readonly ALFItem root;
        private ALFItemReadOnly parent;

        public int Count => root.Count;
        public string Name => root.name;
        public bool IsRoot => root.isRoot;
        public IItemReadOnly Parent => parent;
        public string Text => root.text.ToString();
        public IItemReadOnly this[int index] => new ALFItemReadOnly(root[index], this);
        object IReadOnlyArray.this[int index] => new ALFItemReadOnly(root[index], this);

        private ALFItemReadOnly(ALFItem root, ALFItemReadOnly parent) {
            this.root = root;
            this.parent = parent;
        }

        public ALFItemReadOnly(ALFItem root) : this(root, (ALFItemReadOnly)null) { }

        public object Clone()
            => new ALFItemReadOnly(root == null ? null : (ALFItem)root.Clone());

        public IEnumerator<IItemReadOnly> GetEnumerator()
            => new ItemReadOnlyEnumerator(this);

        public override string ToString()
            => root.text.ToString();

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

        /*
        public string Name => root.name;
        public bool IsRoot => root.isRoot;
        public ALFItemReadOnly Parent => parent;
        public int Count => ArrayManipulation.ArrayLength(root.itens);

        public ALFItemReadOnly this[string name] => this[IndexOf(name)];
        public ALFItemReadOnly this[int index] => GetALFItemReadOnly(root.itens[index]);

        object IReadOnlyArray.this[int index] => GetALFItemReadOnly(root.itens[index]);

        public ALFItemReadOnly(ALFItem root)
            => this.root = root;

        private ALFItemReadOnly GetALFItemReadOnly(ALFItem root) {
            ALFItemReadOnly readOnly = new ALFItemReadOnly(root);
            readOnly.parent = this;
            return readOnly;
        }

        public int IndexOf(string name) {
            for (int I = 0; I < Count; I++)
                if (root.itens[I].name == name)
                    return I;
            return -1;
        }

        public TypeCode GetTypeCode() 
            => TypeCode.String;

        public IEnumerator<ALFItemReadOnly> GetEnumerator()
            => new ALFItemReadOnlyEnumerator(this);

        public override string ToString()
            => root.text.ToString();

        public string ToString(IFormatProvider provider)
            => ToString().ToString(provider);

        public object Clone()
            => new ALFItemReadOnly(this.root == (ALFItem)null ? (ALFItem)null : (ALFItem)this.root.Clone());

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => (ToString() as IConvertible).ToBoolean(provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => (ToString() as IConvertible).ToChar(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => (ToString() as IConvertible).ToSByte(provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => (ToString() as IConvertible).ToByte(provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => (ToString() as IConvertible).ToByte(provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => (ToString() as IConvertible).ToUInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => (ToString() as IConvertible).ToInt32(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => (ToString() as IConvertible).ToUInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => (ToString() as IConvertible).ToInt64(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => (ToString() as IConvertible).ToUInt64(provider);

        public float ToSingle(IFormatProvider provider)
            => (ToString() as IConvertible).ToSingle(provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => (ToString() as IConvertible).ToDouble(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => (ToString() as IConvertible).ToDecimal(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) 
            => (ToString() as IConvertible).ToDateTime(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => (ToString() as IConvertible).ToType(conversionType, provider);

        IEnumerator IEnumerable.GetEnumerator()
            => new ALFItemReadOnlyEnumerator(this);
        */
    }
}
