using System;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Components.Collections {
    public sealed class ALFItemReadOnly : IReadOnlyArray<ALFItemReadOnly>, IConvertible {
        private readonly ALFItem item;
        private ALFItemReadOnly parent;

        public string Name => item.name;
        public bool IsRoot => item.isRoot;
        public ALFItemReadOnly Parent => parent;
        public int Count => ArrayManipulation.ArrayLength(item.itens);

        public ALFItemReadOnly this[string name] => this[IndexOf(name)];
        public ALFItemReadOnly this[int index] => GetALFItemReadOnly(item.itens[index]);

        object IReadOnlyArray.this[int index] => GetALFItemReadOnly(item.itens[index]);

        public ALFItemReadOnly(ALFItem item)
            => this.item = item;

        private ALFItemReadOnly GetALFItemReadOnly(ALFItem item) {
            ALFItemReadOnly readOnly = new ALFItemReadOnly(item);
            readOnly.parent = this;
            return readOnly;
        }

        public int IndexOf(string name) {
            for (int I = 0; I < Count; I++)
                if (item.itens[I].name == name)
                    return I;
            return -1;
        }

        public TypeCode GetTypeCode() 
            => TypeCode.String;

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

        public IEnumerator<ALFItemReadOnly> GetEnumerator()
            => new ALFItemReadOnlyEnumerator(this);

        public override string ToString()
            => item.text.ToString();

        public string ToString(IFormatProvider provider) 
            => ToString().ToString(provider);
    }
}
