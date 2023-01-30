using System.Text;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Alfbt.Components;

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
                    item.name != ALFBTUtility.n_BreakLine &&
                    item.name != ALFBTUtility.n_Comment)
                    return true;
            return false;
        }

        public override void Dispose() {
            if (disposedValue)
                throw ALFException.GetALFException(1000, GetType());
            disposedValue = true;

            StringBuilder builder = new StringBuilder();
            WriterALFBTFlag(root, builder, memory.Indent);
            memory.Writer(builder);

            root.Dispose();
            memory.Dispose();
        }

        public override void WriteElement(string name, string text) {
            if (string.IsNullOrEmpty(name))
                throw ALFException.GetALFException(1001);
            else if (Contains(name))
                throw ALFBTException.GetALFBTException(1102, name);
            else if (!ALFBTUtility.ThisNameIsValid(name))
                throw ALFException.GetALFException(1009, name);
            writingStarted = true;
            if (memory.AddEscapeOnSpecialCharacters)
                text = AddEscapeOnSpecialCharactersInText(text);
            else if (ALFBTUtility.TheTextIsValid(text))
                throw ALFBTException.GetALFBTException(1107, name);

            root.Add(new ALFItem(name, text));
        }

        public override void WriteLineBreak()
            => WriteLineBreak(1);

        public override void WriteLineBreak(int lines) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < lines; I++)
                builder.Append("\r\n");
            WriteElement(ALFBTUtility.n_BreakLine, builder.ToString());
            writingStarted = false;
        }

        public override void WriterCommentFlag(string text) {
            WriteElement(ALFBTUtility.n_Comment, text);
            writingStarted = false;
        }

        public override void WriterHeaderFlag() {
            if (writingStarted || headerStarted)
                throw ALFException.GetALFException(1004);
            headerStarted = true;
            WriteElement(ALFBTUtility.n_Version, ALFBTUtility.alfbt_Version);
            WriteElement(ALFBTUtility.n_Type, ALFBTUtility.alfbt_Type);
            WriteElement(ALFBTUtility.n_Encoding, memory.Encoding.BodyName);
        }

        public override void WriterMarkingFlag(string name, string value)
            => WriteElement(name, value);

        public override void WriterTextFlag(string name, string value)
            => WriteElement(name, value);

        protected override string AddEscapeOnSpecialCharactersInText(string value) {
            using (CharacterCursor cursor = new CharacterCursor(value.ToCharArray()))
                while (cursor.MoveToCharacter()) {
                    if (cursor.CharIsEqualToIndex(ALFUtility.EscapesString))
                        cursor.MoveToCharacter(1L);
                    else if (cursor.CharIsEqualToIndex(ALFUtility.InvalidTextCharacters)) {
                        cursor.AddEscape('\\');
                        cursor.MoveToCharacter(1L);
                    }
                }
            return value;
        }
    }
}
