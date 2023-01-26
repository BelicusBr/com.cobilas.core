using System;
using System.Text;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf {
    internal class ALFMemoryStreamWriter : ALFWriter {
        private ALFItem root;
        private bool disposedValue;
        private bool headerStarted;
        private bool writingStarted;
        private readonly ALFWriterSettings memory;

        public override ALFWriterSettings Settings => memory;

        internal ALFMemoryStreamWriter(ALFWriterSettings memory) {
            this.memory = memory;
            this.root = ALFItem.DefaultRoot;
            disposedValue =
            headerStarted =
            writingStarted = false;
        }

        public override void Close() => memory.Close();

        public override void Flush() => memory.Flush();

        public override void Dispose() {
            if (disposedValue)
                throw ALFException.GetObjectDisposedException<ALFMemoryStreamWriter>();
            else if (!root.isRoot)
                throw ALFException.UnfinishedFlag(root.name);

            disposedValue = true;
            StringBuilder builder = new StringBuilder();
            using (root)
                WriteFlag(root, 0, builder, memory.Indent);
            memory.Writer(builder);
            memory.Dispose();
        }

        public override void StartElement(string name) {
            if (string.IsNullOrEmpty(name))
                throw ALFException.BlankName();
            if (!ThisNameIsValid(name))
                throw ALFException.InvalidName(name);
            writingStarted = true;
            root.Add(root = new ALFItem(name));
        }

        public override void EndElement() {
            if (root.isRoot)
                throw ALFException.DoNotLeaveTheRoot();
            this.root = this.root.parent;
        }

        public override void StartElementBreakLine(int breaks) {
            StartElement(n_BreakLine);
            for (int I = 0; I < breaks; I++)
                WriteText("\r\n");
            EndElement();
            writingStarted = headerStarted;
        }

        public override void StartElementBreakLine()
            => StartElementBreakLine(1);

        public override void StartElementComment(string text) {
            WriteElement(n_Comment, text);
            writingStarted = headerStarted;
        }

        public override void StartElementHeader() {
            if (writingStarted || headerStarted)
                throw ALFException.HeaderInSecond();
            headerStarted = true;
            StartElement("Header");
            WriteElement(n_Version, alf_Version);
            WriteElement(n_Type, alf_Type);
            WriteElement(n_Encoding, memory.Encoding.BodyName);
            EndElement();
        }

        public override void WriteElement(string name, string text) {
            StartElement(name);
            WriteText(text);
            EndElement();
        }

        public override void WriteText(bool value)
            => InternalWriteText(value);

        public override void WriteText(string value)
            => InternalWriteText(value);

        public override void WriteText(char value)
            => InternalWriteText(value);

        public override void WriteText(char[] value)
            => InternalWriteText(value);

        public override void WriteText(float value)
            => InternalWriteText(value);

        public override void WriteText(double value)
            => InternalWriteText(value);

        public override void WriteText(decimal value)
            => InternalWriteText(value);

        public override void WriteText(sbyte value)
            => InternalWriteText(value);

        public override void WriteText(short value)
            => InternalWriteText(value);

        public override void WriteText(int value)
            => InternalWriteText(value);

        public override void WriteText(long value)
            => InternalWriteText(value);

        public override void WriteText(byte value)
            => InternalWriteText(value);

        public override void WriteText(ushort value)
            => InternalWriteText(value);

        public override void WriteText(uint value)
            => InternalWriteText(value);

        public override void WriteText(ulong value)
            => InternalWriteText(value);

        public override void WriteText(DateTime value)
            => InternalWriteText(value);

        protected override void InternalWriteText(object value) {
            if (value is char[] chars) {
                InternalWriteText(chars);
                return;
            } else if (value is string stg) {
                if (memory.AddEscapeOnSpecialCharacters)
                    value = AddEscapeOnSpecialCharactersInText(stg);
                else if (!ThisTextIsValid(out char error, stg))
                    throw ALFException.InvalidText(error);
            } else if (value is char cr) {
                if (memory.AddEscapeOnSpecialCharacters)
                    value = AddEscapeOnSpecialCharactersInText(cr);
                else if (!ThisTextIsValid(out char error, cr))
                    throw ALFException.InvalidText(error);
            } else if (value is DateTime date) {
                value = AddEscapeOnSpecialCharactersInText(date.ToString());
            }
            root.text.Append(value);
        }

        protected override void InternalWriteText(char[] value) {
            if (memory.AddEscapeOnSpecialCharacters)
                value = AddEscapeOnSpecialCharactersInText(value).ToCharArray();
            else if (!ThisTextIsValid(out char error, value))
                throw ALFException.InvalidText(error);
            root.text.Append(value);
        }

        protected override string AddEscapeOnSpecialCharactersInText(string value)
            => value.Replace("\\", "\\\\").Replace(":", "\\:").Replace("[", "\\[")
                    .Replace("]", "\\]").Replace("<", "\\<").Replace(">", "\\>");

        protected string AddEscapeOnSpecialCharactersInText(params char[] value)
            => AddEscapeOnSpecialCharactersInText(new string(value));
    }
}
