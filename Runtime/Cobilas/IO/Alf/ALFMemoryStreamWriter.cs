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
                throw ALFException.GetALFException(1000, GetType());
            else if (!root.isRoot)
                throw ALFException.GetALFException(1006, root.name);

            disposedValue = true;
            StringBuilder builder = new StringBuilder();
            using (root)
                WriteFlag(root, 0, builder, memory.Indent);
            memory.Writer(builder);
            memory.Dispose();
        }

        public override void StartElement(string name) {
            if (string.IsNullOrEmpty(name))
                throw ALFException.GetALFException(1001);
            if (!ALFUtility.ThisNameIsValid(name))
                throw ALFException.GetALFException(1009, name);
            writingStarted = true;
            root.Add(root = new ALFItem(name));
        }

        public override void EndElement() {
            if (root.isRoot)
                throw ALFException.GetALFException(1007);
            this.root = this.root.parent;
        }

        public override void StartElementBreakLine(int breaks) {
            StartElement(ALFUtility.n_BreakLine);
            for (int I = 0; I < breaks; I++)
                WriteText("\r\n");
            EndElement();
            writingStarted = headerStarted;
        }

        public override void StartElementBreakLine()
            => StartElementBreakLine(1);

        public override void StartElementComment(string text) {
            WriteElement(ALFUtility.n_Comment, text);
            writingStarted = headerStarted;
        }

        public override void StartElementHeader() {
            if (writingStarted || headerStarted)
                throw ALFException.GetALFException(1004);
            headerStarted = true;
            StartElement("Header");
            WriteElement(ALFUtility.n_Version, ALFUtility.alf_Version);
            WriteElement(ALFUtility.n_Type, ALFUtility.alf_Type);
            WriteElement(ALFUtility.n_Encoding, memory.Encoding.BodyName);
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
            if (value is char[] chars)
                value = new string(chars);

            if (memory.AddEscapeOnSpecialCharacters)
                value = AddEscapeOnSpecialCharactersInText(value.ToString());
            else if (!ALFUtility.TheTextIsValid(value.ToString()))
                throw ALFException.GetALFException(1011);

            root.text.Append(value);
        }

        //protected override void InternalWriteText(char[] value) {
        //    if (memory.AddEscapeOnSpecialCharacters)
        //        value = AddEscapeOnSpecialCharactersInText(value).ToCharArray();
        //    else if (!ThisTextIsValid(out char error, value))
        //        throw ALFException.InvalidText(error);
        //    root.text.Append(value);
        //}

        protected override string AddEscapeOnSpecialCharactersInText(string value) {
            using (CharacterCursor cursor = new CharacterCursor(value.ToCharArray()))
                while (cursor.MoveToCharacter()) {
                    if (cursor.CharIsEqualToIndex(ALFUtility.EscapesString)) {
                        cursor.MoveToCharacter(1L);
                    } else if (cursor.CharIsEqualToIndex(ALFUtility.InvalidTextCharacters)) {
                        cursor.AddEscape('\\');
                        cursor.MoveToCharacter(1L);
                    }
                }
            return value;
        }

        protected string AddEscapeOnSpecialCharactersInText(params char[] value)
            => AddEscapeOnSpecialCharactersInText(new string(value));
    }
}
