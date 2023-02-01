using System;
using System.IO;
using System.Text;
using System.Collections;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using System.Collections.Generic;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Alfbt.Components;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt {
    public abstract class ALFBTRead : IDisposable, IReadOnlyArray<IItemReadOnly> {

        public abstract int Count { get; }
        public abstract IItemReadOnly ReadOnly { get; }
        public abstract ALFReadSettings Settings { get; }
        public abstract IItemReadOnly this[int index] { get; }
        object IReadOnlyArray.this[int index] => (this as IReadOnlyArray<IItemReadOnly>)[index];

        [Obsolete("Use IItemReadOnly:GetFlag(string)")]
        public abstract HeaderFlag GetHeaderFlag();
        [Obsolete("Use IItemReadOnly:GetFlag(string)")]
        public abstract MarkingFlag GetMarkingFlag(string name);
        [Obsolete("Use IItemReadOnly:GetFlag(string)")]
        public abstract TextFlag GetTextFlag(string name);
        [Obsolete("Use IItemReadOnly:GetFlag(string)")]
        public abstract MarkingFlag[] GetAllMarkingFlags();
        [Obsolete("Use IItemReadOnly:GetFlag(string)")]
        public abstract TextFlag[] GetAllTextFlags();
        public abstract CommentFlag GetCommentFlag();
        [Obsolete("Use bool:FlagExists(string)")]
        public abstract bool TextFlagExists(string name);
        [Obsolete("Use bool:FlagExists(string)")]
        public abstract bool MarkingFlagExists(string name);
        [Obsolete("Use bool:FlagExists(string)")]
        public abstract bool HeaderFlagExists(string name);
        [Obsolete("Use bool:FlagExists(string)")]
        public abstract bool FlagExists(string name, AlfbtFlags flags);
        public abstract bool FlagExists(string name);
        public abstract IItemReadOnly GetFlag(string name);

        public abstract void Close();
        public abstract void Flush();
        public abstract void Dispose();
        public abstract IEnumerator<IItemReadOnly> GetEnumerator();

        protected abstract void Read();
        protected abstract void RemoveEscapeOnSpecialCharactersInALFItem(ALFItem root);

        IEnumerator IEnumerable.GetEnumerator()
            => (this as IReadOnlyArray<IItemReadOnly>).GetEnumerator();

        public static ALFBTRead Create(TextReader reader, ALFReadSettings settings) {
            settings.Set(reader, Encoding.Default);
            return new ALFBTMemoryStreamRead(settings);
        }

        public static ALFBTRead Create(Stream stream, Encoding encoding, ALFReadSettings settings) {
            settings.Set(stream, encoding);
            return new ALFBTMemoryStreamRead(settings);
        }

        public static ALFBTRead Create(Stream stream, ALFReadSettings settings) {
            settings.Set(stream, Encoding.UTF8);
            return new ALFBTMemoryStreamRead(settings);
        }

        public static ALFBTRead Create(TextReader reader)
            => Create(reader, ALFMemoryReadSettings.DefaultSettings);

        public static ALFBTRead Create(Stream stream, Encoding encoding)
            => Create(stream, encoding, ALFMemoryReadSettings.DefaultSettings);

        public static ALFBTRead Create(Stream stream)
            => Create(stream, ALFMemoryReadSettings.DefaultSettings);

        protected static void GetALFBTFlags(ALFItem root, CharacterCursor cursor) {
            ALFBTVersion version = GetALFBTFlagVersion(cursor);
            cursor.Reset();
            switch (version) {
                case ALFBTVersion.alfbt_1_0:
                    StringBuilder builder = new StringBuilder();
                    GetALFBTFlags10(builder, cursor);
                    using (CharacterCursor cursor1 = new CharacterCursor(builder))
                        GetALFBTFlags15(root, cursor1);
                    break;
                case ALFBTVersion.alfbt_1_5:
                    GetALFBTFlags15(root, cursor);
                    break;
                case ALFBTVersion.UnknownVersion:
                    throw ALFBTException.GetALFBTException(1100);
            }
        }

        #region ALFBT 1.0
        private static void GetALFBTFlags10(StringBuilder builder, CharacterCursor cursor) {
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("#$ ")) {
                    cursor.MoveToCharacter(2L);
                    
                    builder.Append("#! hd/");
                    GetFlagName10(builder, cursor);
                    builder.Append(":/*");
                    
                    if (cursor.CharIsEqualToIndex("="))
                        GetHeaderText10(builder, cursor);
                    builder.Append("*/\r\n");
                    continue;
                } else if (cursor.CharIsEqualToIndex("#! ")) {
                    cursor.MoveToCharacter(2L);
                    
                    builder.Append("#! ");
                    GetFlagName10(builder, cursor);
                    builder.Append(":/*");

                    if (cursor.CharIsEqualToIndex("="))
                        GetTagFlagText10(builder, cursor);
                    builder.Append("*/\r\n");
                    continue;
                } else if (cursor.CharIsEqualToIndex("#? ")) {
                    cursor.MoveToCharacter(2L);

                    builder.Append("#! txt/");
                    GetFlagName10(builder, cursor);
                    builder.Append(":/*");
                    
                    if (cursor.CharIsEqualToIndex("=@(")) {
                        cursor.MoveToCharacter(2L);
                        GetTextFlagText10(builder, cursor);
                    }
                    builder.Append("*/\r\n");
                    continue;
                } else if (cursor.CharIsEqualToIndex("#*")) {
                    builder.Append("#>");
                    cursor.MoveToCharacter(1L);
                    GetFlageComment10(builder, cursor);
                    builder.Append("<#\r\n");
                    continue;
                }

                if (!ALFBTUtility.IsWhiteSpace(cursor.CurrentCharacter))
                    throw ALFException.GetALFException(1005, cursor.Cursor, cursor.CurrentCharacter);
            }
        }

        private static void GetTextFlagText10(StringBuilder builder, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scursor = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex(")@")) {
                    cursor.MoveToCharacter(1L);
                    return;
                }
                if (cursor.CharIsEqualToIndex("\\)", "\\\\", "\\@")) {
                    builder.Append(cursor.CurrentCharacter);
                    cursor.MoveToCharacter(1L);
                    builder.Append(cursor.CurrentCharacter);
                    continue;
                } else if (cursor.CharIsEqualToIndex('\\', ')', '@'))
                    throw ALFBTException.GetALFBTException(1103, cursor.Cursor, cursor.CurrentCharacter);
                builder.Append(cursor.CurrentCharacter);
            }
            throw ALFException.GetALFException(1014, scursor);
        }

        private static void GetHeaderText10(StringBuilder builder, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scursor = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("\r\n")) {
                    cursor.MoveToCharacter(1L);
                    return;
                } else if (cursor.CharIsEqualToIndex('\n'))
                    return;
                if (!TagOrHeaderFlagTextIsValid(cursor, '_', '.', '-'))
                    throw ALFBTException.GetALFBTException(1104, cursor.Cursor, cursor.CurrentCharacter);
                builder.Append(cursor.CurrentCharacter);
            }
            throw ALFException.GetALFException(1014, scursor);
        }

        private static void GetTagFlagText10(StringBuilder builder, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scursor = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("\r\n")) {
                    cursor.MoveToCharacter(1L);
                    return;
                } else if (cursor.CharIsEqualToIndex('\n'))
                    return;
                if (!TagOrHeaderFlagTextIsValid(cursor, '\\', '/', '_', '.'))
                    throw ALFBTException.GetALFBTException(1105, cursor.Cursor, cursor.CurrentCharacter);
                builder.Append(cursor.CurrentCharacter);
            }
            throw ALFException.GetALFException(1014, scursor);
        }

        private static void GetFlagName10(StringBuilder builder, CharacterCursor cursor) {
            StringBuilder sbuilder = new StringBuilder();
            CharacterCursor.LineEndColumn column = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex('=')) {
                    if (string.IsNullOrEmpty(sbuilder.ToString()))
                        throw ALFException.GetALFException(1002, column);
                    builder.Append(sbuilder);
                    return;
                } else if (!ALFUtility.ValidNameCharacter(cursor.CurrentCharacter))
                    throw ALFException.GetALFException(1010, cursor.Cursor, sbuilder.Append(cursor.CurrentCharacter).ToString());
                sbuilder.Append(cursor.CurrentCharacter);
            }
        }

        private static void GetFlageComment10(StringBuilder builder, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scurso = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("\r\n")) {
                    cursor.MoveToCharacter(1L);
                    return;
                } else if (cursor.CharIsEqualToIndex('\n'))
                    return;
                builder.Append(cursor.CurrentCharacter);
            }
            throw ALFException.GetALFException(1003, scurso);
        }

        private static bool TagOrHeaderFlagTextIsValid(CharacterCursor cursor, params char[] ch)
            => cursor.CharIsEqualToIndex(ch) || char.IsLetterOrDigit(cursor.CurrentCharacter) ||
                        char.IsControl(cursor.CurrentCharacter);
        #endregion
        #region ALFBT 1.5
        private static void GetALFBTFlags15(ALFItem root, CharacterCursor cursor) {
            ALFItem item;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("#! ")) {
                    CharacterCursor.LineEndColumn scursor = cursor.Cursor;
                    cursor.MoveToCharacter(2L);
                    GetFlagName(item = ALFItem.Empty, cursor);
                    if (cursor.CharIsEqualToIndex(":/*")) {
                        cursor.MoveToCharacter(2L);
                        GetFlagText(item, cursor);
                    }
                    if (!AddFlag(root, item))
                        throw ALFBTException.GetALFBTException(1101, scursor, item.name);
                    continue;
                } else if (cursor.CharIsEqualToIndex("#>")) {
                    cursor.MoveToCharacter(1L);
                    GetFlageComment(item = ALFItem.DefaultComment, cursor);
                    root.Add(item);
                    continue;
                }

                if (!ALFBTUtility.IsWhiteSpace(cursor.CurrentCharacter))
                    throw ALFException.GetALFException(1005, cursor.Cursor, cursor.CurrentCharacter);
            }
        }

        private static bool AddFlag(ALFItem root, ALFItem item) {
            if (root.Contains(item.name)) return false;
            root.Add(item);
            return true;
        }

        private static void GetFlageComment(ALFItem item, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scurso = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("<#")) {
                    cursor.MoveToCharacter(1L);
                    return;
                }
                item.text.Append(cursor.CurrentCharacter);
            }
            throw ALFException.GetALFException(1003, scurso);
        }

        private static void GetFlagName(ALFItem item, CharacterCursor cursor) {
            StringBuilder builder = new StringBuilder();
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex(":/*")) {
                    item.name = builder.ToString();
                    return;
                }
                if (!ALFUtility.ValidNameCharacter(cursor.CurrentCharacter))
                    throw ALFException.GetALFException(1010, cursor.Cursor, builder.Append(cursor.CurrentCharacter).ToString());
                builder.Append(cursor.CurrentCharacter);
            }
        }

        private static void GetFlagText(ALFItem item, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scursor = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("*/")) {
                    cursor.MoveToCharacter(1L);
                    return;
                }
                if (cursor.CharIsEqualToIndex("\\\\", "\\/", "\\*")) {
                    item.text.Append(cursor.CurrentCharacter);
                    cursor.MoveToCharacter(1L);
                    item.text.Append(cursor.CurrentCharacter);
                    continue;
                }
                if (cursor.CharIsEqualToIndex('\\', '/', '*'))
                    throw ALFBTException.GetALFBTException(1106, cursor.Cursor, cursor.CurrentCharacter);
                item.text.Append(cursor.CurrentCharacter);
            }
            throw ALFException.GetALFException(1014, scursor);
        }

        private static ALFBTVersion GetALFBTFlagVersion(CharacterCursor cursor) {
            bool determined = false;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex('\n'))
                    break;
                if (determined) {
                    if (cursor.CharIsEqualToIndex(":/*"))
                        return ALFBTVersion.alfbt_1_5;
                    else if (cursor.CharIsEqualToIndex('='))
                        return ALFBTVersion.alfbt_1_0;
                    continue;
                }
                if (cursor.CharIsEqualToIndex("#$") ||
                    cursor.CharIsEqualToIndex("#?") ||
                    cursor.CharIsEqualToIndex("#*"))
                    return ALFBTVersion.alfbt_1_0;

                if (cursor.CharIsEqualToIndex("#>"))
                    return ALFBTVersion.alfbt_1_5;

                if (cursor.CharIsEqualToIndex("#!"))
                    determined = true;
            }
            return ALFBTVersion.UnknownVersion;
        }
        #endregion
    }
}
