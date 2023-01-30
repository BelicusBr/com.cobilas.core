using System;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Components {
    public sealed class CharacterCursor : IDisposable {
        private char[] characters;
        private long index;
        private long line;
        private long column;

        public long Line => line;
        public long Index => index;
        public long Column => column;
        public long Count => ArrayManipulation.ArrayLongLength(characters);
        public LineEndColumn Cursor => new LineEndColumn(line, column, index);
        public char CurrentCharacter => index < Count ? characters[index] : '\0';

        public char this[long index] => characters[index];

        public CharacterCursor(char[] characters) {
            this.characters = characters;
            index = -1L;
            line = 1L;
            column = 0L;
        }

        public CharacterCursor(string text) :
            this(text.ToCharArray()) { }

        public CharacterCursor(StringBuilder text) :
            this(text.ToString()) { }

        public CharacterCursor(byte[] bytes, Encoding encoding) :
            this(encoding.GetChars(bytes)) { }

        public bool MoveToCharacter(long index) {
            bool res = (this.index += index) < Count;
            ++column;
            if (CurrentCharacter == '\n') {
                ++line;
                column = 0L;
            }
            return res;
        }

        public void AddEscape(char escape) {
            char[] newCharacters = new char[Count + 1];
            for (long I = 0; I < characters.LongLength; I++) {
                if (I == index) {
                    newCharacters[I] = escape;
                    newCharacters[I + 1L] = characters[I];
                } else if (I > index)
                    newCharacters[I + 1L] = characters[I];
                else
                    newCharacters[I] = characters[I];
            }
            characters = null;
            characters = newCharacters;
        }

        public bool MoveToCharacter()
            => MoveToCharacter(1L);

        public void Reset() {
            line = 1;
            column = 1;
            index = -1L;
        }

        public string SliceText(long index, long count) {
            StringBuilder builder = new StringBuilder();
            for (long I = index; I < count + index && I < Count; I++)
                builder.Append(this[I]);
            return builder.ToString();
        }

        public bool CharIsEqualToIndex(char character)
            => index < Count && character == characters[index];

        public bool CharIsEqualToIndex(params char[] characters) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(characters) && index < Count; I++)
                if (characters[I] == this.characters[index])
                    return true;
            return false;
        }

        public bool CharIsEqualToIndex(string text) {
            if (string.IsNullOrEmpty(text)) return false;
            long textCount = (text == (string)null) ? 0L : text.Length;
            for (long I = index, C = 0L; C < textCount && I < Count; I++, C++)
                if (text[(int)C] != characters[I])
                    return false;
            return true;
        }

        public bool CharIsEqualToIndex(params string[] texts) {
            foreach (string item in texts)
                if (CharIsEqualToIndex(item))
                    return true;
            return false;
        }

        public void Dispose() {
            ArrayManipulation.ClearArraySafe(ref characters);
        }

        public struct LineEndColumn {
            private readonly long line;
            private readonly long index;
            private readonly long column;

            public long Line => line;
            public long Index => index;
            public long Column => column;

            public static LineEndColumn Default => new LineEndColumn(1, 1, 0);

            public LineEndColumn(long line, long column, long index) {
                this.line = line;
                this.column = column;
                this.index = index + 1;
            }

            public override string ToString() => string.Format("(L:{0} C:{1} I:{2})", line, column, index);
        }
    }
}
