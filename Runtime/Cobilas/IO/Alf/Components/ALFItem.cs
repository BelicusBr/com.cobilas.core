using System;
using System.Text;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Components {
    public sealed class ALFItem : IDisposable, IEnumerable<ALFItem> {
        public ALFItem parent;
        public string name;
        public bool isRoot;
        public StringBuilder text;
        public ALFItem[] itens;

        public ALFItem()
        {
            text = new StringBuilder();
        }

        public void Add(ALFItem item) {
            item.parent = this;
            ArrayManipulation.Add(item, ref itens);
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

        IEnumerator IEnumerable.GetEnumerator()
            => new ArrayToIEnumerator<ALFItem>(itens);
    }
}
