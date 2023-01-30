using System;
using System.Text;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Components {
    public sealed class ALFItem : IDisposable, IEnumerable<ALFItem>, ICloneable, IConvertible {
        public ALFItem parent;
        public string name;
        public bool isRoot;
        public StringBuilder text;
        public ALFItem[] itens;

        public int Count => ArrayManipulation.ArrayLength(itens);
        public static ALFItem Empty => new ALFItem(string.Empty);
        public static ALFItem DefaultComment => new ALFItem(ALFUtility.n_Comment);
        public static ALFItem DefaultRoot => new ALFItem("Root", string.Empty, true);

        public ALFItem this[int index] => itens[index];

        public ALFItem(string name, string text, bool isRoot) {
            this.name = name;
            this.isRoot = isRoot;
            this.text = new StringBuilder(text);
            this.itens = (ALFItem[])null;
            parent = (ALFItem)null;
        }

        public ALFItem(string name, string text) : this(name, text, false) { }

        public ALFItem(string name, bool isRoot) : this(name, string.Empty, isRoot) { }

        public ALFItem(string name) : this(name, string.Empty) { }

        public void Add(ALFItem item) {
            item.parent = this;
            ArrayManipulation.Add(item, ref itens);
        }

        public bool Contains(string name)
            => IndexOf(name) >= 0;

        public int IndexOf(string name) {
            for (int I = 0; I < Count; I++)
                if (itens[I].name == name)
                    return I;
            return -1;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            ToString(builder, 0);
            return builder.ToString();
        }

        private void ToString(StringBuilder builder, int tab) {
            builder.AppendFormat("{0}-> {1}{2}\n", string.Empty.PadRight(tab), isRoot ? "Root:" : string.Empty, name);
            string txt = text.ToString();
            if (!string.IsNullOrEmpty(txt)) {
                txt = txt.Replace("\n", string.Format("\n{0}", string.Empty.PadRight(tab + 1)));
                builder.AppendFormat("{0}-->my text:\n", string.Empty.PadRight(tab + 1));
                builder.AppendFormat("{0}{1}\n", string.Empty.PadRight(tab + 1), txt);
                builder.AppendFormat("{0}--<my text:\n", string.Empty.PadRight(tab + 1));
            }
            for (int I = 0; I < ArrayManipulation.ArrayLength(itens); I++)
                itens[I].ToString(builder, tab + 1);
            builder.AppendFormat("{0}-> {1}{2}\n", string.Empty.PadRight(tab), isRoot ? "Root:" : string.Empty, name);
        }

        public void Dispose() {
            this.isRoot = default;
            this.parent = (ALFItem)null;
            this.name = (string)null;
            this.text = (StringBuilder)null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(itens); I++)
                itens[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref itens);
        }

        public IEnumerator<ALFItem> GetEnumerator()
            => new ArrayToIEnumerator<ALFItem>(itens);

        public object Clone() {
            ALFItem item = new ALFItem(name == (string)null ? string.Empty : (string)name.Clone());
            item.text.Append(text.ToString());
            item.isRoot = isRoot;
            for (int I = 0; I < Count; I++)
                item.Add((ALFItem)itens[I].Clone());
            return item;
        }

        public TypeCode GetTypeCode()
            => TypeCode.String;

        IEnumerator IEnumerable.GetEnumerator()
            => new ArrayToIEnumerator<ALFItem>(itens);

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToBoolean(provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToChar(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToSByte(provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToByte(provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToInt16(provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToUInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToInt32(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToUInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToInt64(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToUInt64(provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToSingle(provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToDouble(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToDecimal(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToDateTime(provider);

        string IConvertible.ToString(IFormatProvider provider)
            => (text.ToString() as IConvertible).ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => (text.ToString() as IConvertible).ToType(conversionType, provider);
    }
}
