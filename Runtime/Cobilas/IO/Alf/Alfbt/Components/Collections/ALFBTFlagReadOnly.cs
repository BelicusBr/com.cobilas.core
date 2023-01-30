using System;
using System.Collections;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using System.Collections.Generic;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt.Components.Collections {
    public sealed class ALFBTFlagReadOnly : FlagBase, IItemReadOnly {
        private readonly ALFItemReadOnly readOnly;

        public int Count => readOnly.Count;
        public string Text => ((IItemReadOnly)readOnly).Text;
        public bool IsRoot => ((IItemReadOnly)readOnly).IsRoot;
        public IItemReadOnly this[string name] => ((IItemReadOnly)readOnly)[name];

        IItemReadOnly IItemReadOnly.Parent => (IItemReadOnly)null;
        object IReadOnlyArray.this[int index] => new ALFBTFlagReadOnly(readOnly[index] as ALFItemReadOnly);
        IItemReadOnly IReadOnlyArray<IItemReadOnly>.this[int index] => new ALFBTFlagReadOnly(readOnly[index] as ALFItemReadOnly);

        public ALFBTFlagReadOnly(ALFItemReadOnly readOnly): base(readOnly.Name, readOnly.ToString(), AlfbtFlags.MarkingFlag)
        {
            this.readOnly = readOnly;
        }

        public ALFBTFlagReadOnly(ALFItem readOnly) : this(new ALFItemReadOnly(readOnly)) { }

        public new object Clone()
            => new ALFBTFlagReadOnly(readOnly == null ? null : (ALFItemReadOnly)readOnly.Clone());

        public void Dispose()
            => ((IDisposable)readOnly).Dispose();

        public int IndexOf(string name)
            => ((IItemReadOnly)readOnly).IndexOf(name);

        public bool Contains(string name)
            => ((IItemReadOnly)readOnly).Contains(name);

        public IEnumerator<IItemReadOnly> GetEnumerator()
            => new ALFBTFlagReadOnlyEnumerator(this);

        public TypeCode GetTypeCode()
            => ((IConvertible)readOnly).GetTypeCode();

        public override string ToString()
            => readOnly.ToString();

        IEnumerator IEnumerable.GetEnumerator()
            => new ALFBTFlagReadOnlyEnumerator(this);

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => ((IConvertible)readOnly).ToBoolean(provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => ((IConvertible)readOnly).ToByte(provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => ((IConvertible)readOnly).ToChar(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => ((IConvertible)readOnly).ToDateTime(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => ((IConvertible)readOnly).ToDecimal(provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => ((IConvertible)readOnly).ToDouble(provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => ((IConvertible)readOnly).ToInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => ((IConvertible)readOnly).ToInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => ((IConvertible)readOnly).ToInt64(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => ((IConvertible)readOnly).ToSByte(provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => ((IConvertible)readOnly).ToSingle(provider);

        string IConvertible.ToString(IFormatProvider provider)
            => ((IConvertible)readOnly).ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => ((IConvertible)readOnly).ToType(conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => ((IConvertible)readOnly).ToUInt16(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => ((IConvertible)readOnly).ToUInt32(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => ((IConvertible)readOnly).ToUInt64(provider);
    }
}
