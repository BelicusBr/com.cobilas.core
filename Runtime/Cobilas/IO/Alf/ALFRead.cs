using System;
using System.IO;
using System.Text;
using System.Collections;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using System.Collections.Generic;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf {
    public abstract class ALFRead : IDisposable, IReadOnlyArray<IItemReadOnly> {
        
        public abstract int Count { get; }
        public abstract IItemReadOnly ReadOnly { get; }
        public abstract ALFReadSettings Settings { get; }
        public abstract IItemReadOnly this[int index] { get; }
        object IReadOnlyArray.this[int index] => (this as IReadOnlyArray<IItemReadOnly>)[index];

        public abstract void Close();
        public abstract void Flush();
        public abstract void Dispose();
        public abstract IEnumerator<IItemReadOnly> GetEnumerator();

        protected abstract void Read();
        protected abstract void RemoveEscapeOnSpecialCharactersInALFItem(ALFItem root);

        IEnumerator IEnumerable.GetEnumerator() {
            return (this as IReadOnlyArray<IItemReadOnly>).GetEnumerator();
        }

        public static ALFRead Create(TextReader reader, ALFReadSettings settings) {
            settings.Set(reader, Encoding.Default);
            return new ALFMemoryStreamRead(settings);
        }

        public static ALFRead Create(TextReader reader)
            => Create(reader, ALFMemoryReadSettings.DefaultSettings);

        public static ALFRead Create(Stream stream, Encoding encoding, ALFReadSettings settings) {
            settings.Set(stream, encoding);
            return new ALFMemoryStreamRead(settings);
        }

        public static ALFRead Create(Stream stream, ALFReadSettings settings) {
            settings.Set(stream, Encoding.UTF8);
            return new ALFMemoryStreamRead(settings);
        }

        public static ALFRead Create(Stream stream, Encoding encoding)
            => Create(stream, encoding, ALFMemoryReadSettings.DefaultSettings);

        public static ALFRead Create(Stream stream)
            => Create(stream, ALFMemoryReadSettings.DefaultSettings);

        public static bool ValidCharacter(char carac) {
            switch (carac) {
                case '\\':
                case '/':
                case '_':
                case '.': return true;
                default: return char.IsLetterOrDigit(carac);
            }
        }

        public static bool IsWhiteSpace(char c) {
            if (char.IsWhiteSpace(c))
                return true;
            return char.IsControl(c);
        }

        protected static void GetAlfFlag(ALFItem root, CharacterCursor cursor)
            => GetAlfFlag(root, cursor, false);

        internal static bool ValidTextCharacter(char carac) {
            switch (carac) {
                case ':':
                case '[':
                case ']':
                case '<':
                case '>': return false;
                default: return true;
            }
        }

        private static void GetAlfFlag(ALFItem root, CharacterCursor cursor, bool isSubFlag) {
            ALFItem subflag;
            CharacterCursor.LineEndColumn num1;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("[*")) {
                    num1 = cursor.Cursor;
                    cursor.MoveToCharacter(1L);
                    if (!GetComment(root, cursor))
                        throw ALFException.CommentNotFinalized(num1);
                    cursor.MoveToCharacter(1L);
                    continue;
                } else if (cursor.CharIsEqualToIndex('[')) {
                    subflag = new ALFItem(string.Empty);
                    root.Add(subflag);
                    num1 = cursor.Cursor;

                    if (!GetFlagName(subflag, cursor))
                        throw ALFException.UnfinishedName(num1);
                    if (cursor.CharIsEqualToIndex(":>")) {
                        cursor.MoveToCharacter(1L);
                        GetAlfFlag(subflag, cursor, true);
                    } else if (cursor.CharIsEqualToIndex(':')) {
                        GetFlagText(subflag, cursor);
                        if (cursor.CharIsEqualToIndex(":>")) {
                            cursor.MoveToCharacter(1L);
                            GetAlfFlag(subflag, cursor, true);
                        }
                    }
                    continue;
                }

                if (cursor.CharIsEqualToIndex("<]") ||
                    cursor.CharIsEqualToIndex(']')) {
                    if (isSubFlag)
                        break;
                    continue;
                }

                if (!IsWhiteSpace(cursor.CurrentCharacter))
                    throw ALFException.SymbolNotIdentified(cursor.Cursor, cursor.CurrentCharacter);
            }
        }

        private static bool GetComment(ALFItem item, CharacterCursor cursor) {
            ALFItem comment = new ALFItem("comment");
            item.Add(comment);
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("*]"))
                    return true;
                comment.text.Append(cursor.CurrentCharacter);
            }

            return false;
        }

        private static bool GetFlagText(ALFItem item, CharacterCursor cursor) {
            bool breakLine = false;
            while (cursor.MoveToCharacter()) {
                if (cursor.CurrentCharacter == '\n') {
                    item.text.Append(cursor.CurrentCharacter);
                    breakLine = true;
                }

                if (breakLine) {
                    if (cursor.CharIsEqualToIndex(":>"))
                        break;
                    else if (cursor.CharIsEqualToIndex(':')) {
                        breakLine = false;
                        continue;
                    }
                    if (!IsWhiteSpace(cursor.CurrentCharacter) || !char.IsControl(cursor.CurrentCharacter))
                        throw ALFException.SymbolNotIdentifiedInTextFlag(cursor.Cursor, cursor.CurrentCharacter);
                    continue;
                }

                if (cursor.CharIsEqualToIndex("\\\\", "\\:", "\\[", "\\]", "\\<", "\\>")) {
                    item.text.Append(cursor.CurrentCharacter);
                    cursor.MoveToCharacter(1L);
                    item.text.Append(cursor.CurrentCharacter);
                    continue;
                } else if (cursor.CharIsEqualToIndex(":>") ||
                  cursor.CharIsEqualToIndex(']'))
                    return true;

                if (!ValidTextCharacter(cursor[cursor.Index]))
                    throw ALFException.SymbolNotIdentified(cursor.Cursor, cursor.CurrentCharacter);
                item.text.Append(cursor[cursor.Index]);
            }
            return false;
        }

        private static bool GetFlagName(ALFItem item, CharacterCursor cursor) {
            StringBuilder builder = new StringBuilder();
            CharacterCursor.LineEndColumn column = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex(":>") ||
                    cursor.CharIsEqualToIndex(':')) {
                    item.name = builder.ToString();
                    if (string.IsNullOrEmpty(item.name))
                        throw ALFException.BlankName(column);
                    return true;
                }
                if (!ValidCharacter(cursor[cursor.Index]))
                    throw ALFException.SymbolNotIdentifiedInTextFlag(cursor.Cursor, cursor.CurrentCharacter);
                builder.Append(cursor[cursor.Index]);
            }
            return false;
        }
    }
}
