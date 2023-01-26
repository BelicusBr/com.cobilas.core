using System;
using System.Text;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf.Alfbt {
    internal class ALFBTMemoryStreamWriter : ALFBTWriter {

        private bool disposedValue;
        private bool headerStarted;
        private bool writingStarted;
        private readonly ALFItem root;
        private readonly ALFWriterSettings memory;

        public override ALFWriterSettings Settings => memory;

        internal ALFBTMemoryStreamWriter(ALFWriterSettings memory) {
            this.root = ALFItem.DefaultRoot;
            this.memory = memory;
        }

        public override void Close() => memory.Close();

        public override void Flush() => memory.Flush();

        public override bool Contains(string name) {
            foreach (ALFItem item in root)
                if (item.name == name &&
                    item.name != n_BreakLine &&
                    item.name != n_Comment)
                    return true;
            return false;
        }

        public override void Dispose() {
            if (disposedValue)
                throw ALFException.GetObjectDisposedException<ALFBTMemoryStreamWriter>();
            disposedValue = true;

            StringBuilder builder = new StringBuilder();
            WriterALFBTFlag(root, builder, memory.Indent);
            memory.Writer(builder);

            root.Dispose();
            memory.Dispose();
        }

        public override void StartElementBreakLine(int breaks) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < breaks; I++)
                builder.Append("\r\n");
            WriteElement(n_BreakLine, builder.ToString());
            writingStarted = false;
        }

        public override void StartElementBreakLine()
            => StartElementBreakLine(1);

        public override void StartElementComment(string text) {
            WriteElement(n_Comment, text);
            writingStarted = false;
        }

        public override void StartElementHeader() {
            if (writingStarted || headerStarted)
                throw ALFException.HeaderInSecond();
            headerStarted = true;
            WriteElement(n_Version, alfbtVersion);
            WriteElement(n_Type, ".alfbt");
            WriteElement(n_Encoding, memory.Encoding.BodyName);
        }

        public override void WriteElement(string name, string text) {
            if (string.IsNullOrEmpty(name))
                throw ALFException.BlankName();
            else if (Contains(name))
                throw ALFBTException.FlagAlreadyExists(name);
            else if (!ThisNameIsValid(name))
                throw ALFException.InvalidName(name);
            writingStarted = true;
            if (memory.AddEscapeOnSpecialCharacters)
                text = AddEscapeOnSpecialCharactersInText(text);
            root.Add(new ALFItem(name, text));
        }

        public override void WriteLineBreak()
            => StartElementBreakLine();

        public override void WriterCommentFlag(string text)
            => StartElementComment(text);

        public override void WriterHeaderFlag()
            => StartElementHeader();

        public override void WriterMarkingFlag(string name, string value)
            => WriteElement(name, value);

        public override void WriterTextFlag(string name, string value)
            => WriteElement(name, value);

        [Obsolete("Use WriteElement(string, string)")]
        public override void EndElement() { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void StartElement(string name) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(bool value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(string value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(char value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(char[] value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(float value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(double value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(decimal value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(sbyte value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(short value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(int value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(long value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(byte value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(ushort value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(uint value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(ulong value) { }
        [Obsolete("Use WriteElement(string, string)")]
        public override void WriteText(DateTime value) { }

        protected override string AddEscapeOnSpecialCharactersInText(string value)
            => value.Replace("\\", "\\\\").Replace("/", "\\/").Replace("*", "\\*");

        protected override void InternalWriteText(object value) { }
        protected override void InternalWriteText(char[] value) { }
    }
}
