using System;
using System.IO;
using System.Text;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Alfbt.Components;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt {
    public abstract class ALFBTRead : ALFRead {
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

        public static new ALFBTRead Create(TextReader reader, ALFReadSettings settings) {
            settings.Set(reader, Encoding.Default);
            return new ALFBTMemoryStreamRead(settings);
        }

        public static new ALFBTRead Create(Stream stream, Encoding encoding, ALFReadSettings settings) {
            settings.Set(stream, encoding);
            return new ALFBTMemoryStreamRead(settings);
        }

        public static new ALFBTRead Create(Stream stream, ALFReadSettings settings) {
            settings.Set(stream, Encoding.UTF8);
            return new ALFBTMemoryStreamRead(settings);
        }

        public static new ALFBTRead Create(TextReader reader)
            => Create(reader, ALFMemoryReadSettings.DefaultSettings);

        public static new ALFBTRead Create(Stream stream, Encoding encoding)
            => Create(stream, encoding, ALFMemoryReadSettings.DefaultSettings);

        public static new ALFBTRead Create(Stream stream)
            => Create(stream, ALFMemoryReadSettings.DefaultSettings);

        protected static void GetALFBTFlags(ALFItem root, CharacterCursor cursor) {
            ALFBTVersion version = GetALFBTFlagVersion(cursor);
            cursor.Reset();
            switch (version) {
                case ALFBTVersion.alfbt_1_0:
                    GetALFBTFlags10(root, cursor);
                    break;
                case ALFBTVersion.alfbt_1_5:
                    GetALFBTFlags15(root, cursor);
                    break;
                case ALFBTVersion.UnknownVersion:
                    throw ALFBTException.UnknownVersion();
            }
        }

        #region ALFBT 1.0
        private static void GetALFBTFlags10(ALFItem root, CharacterCursor cursor) {
            ALFItem item;
            while (cursor.MoveToCharacter()) {
                CharacterCursor.LineEndColumn scursor = cursor.Cursor;
                if (cursor.CharIsEqualToIndex("#$ ")) {
                    cursor.MoveToCharacter(2L);
                    GetFlagName(item = ALFItem.Empty, "=", cursor);
                    if (cursor.CharIsEqualToIndex("="))
                        GetFlagText(item, "\n", (c) => false, HeaderValueIsValid, cursor);
                    if (AddFlag(root, item))
                        throw ALFBTException.FlagAlreadyExists(scursor, item.name);
                    continue;
                } else if (cursor.CharIsEqualToIndex("#! ")) {
                    cursor.MoveToCharacter(2L);
                    GetFlagName(item = ALFItem.Empty, "=", cursor);
                    if (cursor.CharIsEqualToIndex("="))
                        GetFlagText(item, "\n", (c) => false, FlagValueIsValid, cursor);
                    if (AddFlag(root, item))
                        throw ALFBTException.FlagAlreadyExists(scursor, item.name);
                    continue;
                } else if (cursor.CharIsEqualToIndex("#? ")) {
                    cursor.MoveToCharacter(2L);
                    GetFlagName(item = ALFItem.Empty, "=@(", cursor);
                    if (cursor.CharIsEqualToIndex("=@("))
                        GetFlagText(item, ")@", IsEscape10, IsExclude10, cursor);
                    if (AddFlag(root, item))
                        throw ALFBTException.FlagAlreadyExists(scursor, item.name);
                    continue;
                } else if (cursor.CharIsEqualToIndex("#* ")) {
                    cursor.MoveToCharacter(2L);
                    GetFlageComment(item = ALFItem.DefaultComment, "\n", cursor);
                    root.Add(item);
                    continue;
                }
            }
        }

        public static bool IsExclude10(CharacterCursor cursor)
            => cursor.CharIsEqualToIndex('\\', '@', '(', ')');

        private static bool IsEscape10(CharacterCursor cursor) {
            if (cursor.CharIsEqualToIndex("\\\\", "\\@", "\\(", "\\)")) {
                cursor.MoveToCharacter(1L);
                return true;
            }
            return false;
        }

        private static bool FlagValueIsValid(CharacterCursor cursor)
            => ValidCharacter(cursor.CurrentCharacter);

        private static bool HeaderValueIsValid(CharacterCursor cursor)
            => ValidCharacter(cursor.CurrentCharacter) || cursor.CurrentCharacter == '-';

        #endregion
        #region ALFBT 1.5
        private static void GetALFBTFlags15(ALFItem root, CharacterCursor cursor) {
            ALFItem item;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex("#! ")) {
                    CharacterCursor.LineEndColumn scursor = cursor.Cursor;
                    cursor.MoveToCharacter(2L);
                    GetFlagName(item = ALFItem.Empty, ":/*", cursor);
                    if (cursor.CharIsEqualToIndex(":/*")) {
                        cursor.MoveToCharacter(2L);
                        GetFlagText(item, "*/", IsEscape15, IsExclude15, cursor);
                    }
                    if (AddFlag(root, item))
                        throw ALFBTException.FlagAlreadyExists(scursor, item.name);
                    continue;
                } else if (cursor.CharIsEqualToIndex("#> ")) {
                    cursor.MoveToCharacter(2L);
                    GetFlageComment(item = ALFItem.DefaultComment, "<#", cursor);
                    root.Add(item);
                    continue;
                }

                if (!IsWhiteSpace(cursor.CurrentCharacter))
                    throw ALFException.SymbolNotIdentified(cursor.Cursor, cursor.CurrentCharacter);
            }
        }

        private static bool IsExclude15(CharacterCursor cursor)
            => cursor.CharIsEqualToIndex('\\', '/', '*');

        private static bool IsEscape15(CharacterCursor cursor) {
            if (cursor.CharIsEqualToIndex("\\\\", "\\/", "\\*")) {
                cursor.MoveToCharacter(1L);
                return true;
            }
            return false;
        }

        #endregion

        private static bool AddFlag(ALFItem root, ALFItem item) {
            if (root.Contains(item.name)) return false;
            root.Add(item);
            return true;
        }

        private static void GetFlageComment(ALFItem item, string escape, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scurso = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex(escape))
                    return;
                item.text.Append(cursor.CurrentCharacter);
            }
            throw ALFException.CommentNotFinalized(scurso);
        }

        private static void GetFlagName(ALFItem item, string separator, CharacterCursor cursor) {
            StringBuilder builder = new StringBuilder();
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex(separator)) {
                    item.name = builder.ToString();
                    return;
                }
                if (!ValidCharacter(cursor.CurrentCharacter))
                    throw ALFException.InvalidName(cursor.Cursor, builder.Append(cursor.CurrentCharacter).ToString());
                builder.Append(cursor.CurrentCharacter);
            }
        }

        private static void GetFlagText(ALFItem item, string escape,
            Func<CharacterCursor, bool> isEscape,
            Func<CharacterCursor, bool> isExclude, CharacterCursor cursor) {
            CharacterCursor.LineEndColumn scursor = cursor.Cursor;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex(escape)) {
                    cursor.MoveToCharacter(1L);
                    return;
                }
                if (isEscape(cursor)) {
                    item.text.Append(cursor.CurrentCharacter);
                    cursor.MoveToCharacter(1L);
                    item.text.Append(cursor.CurrentCharacter);
                    continue;
                }
                if (isExclude(cursor))
                    throw ALFException.InvalidText(cursor.Cursor, cursor.CurrentCharacter);
                item.text.Append(cursor.CurrentCharacter);
            }
            throw ALFException.UnfinishedTextBlock(scursor);
        }

        private static ALFBTVersion GetALFBTFlagVersion(CharacterCursor cursor) {
            bool determined = false;
            while (cursor.MoveToCharacter()) {
                if (cursor.CharIsEqualToIndex('\n'))
                    break;
                if (determined) {
                    if (cursor.CharIsEqualToIndex(":/*"))
                        return ALFBTVersion.alfbt_1_5;
                    else if (cursor.CharIsEqualToIndex("#$ "))
                        return ALFBTVersion.alfbt_1_0;
                    continue;
                }
                if (cursor.CharIsEqualToIndex("#$ ") ||
                    cursor.CharIsEqualToIndex("#? ") ||
                    cursor.CharIsEqualToIndex("#* "))
                    return ALFBTVersion.alfbt_1_0;

                if (cursor.CharIsEqualToIndex("#> "))
                    return ALFBTVersion.alfbt_1_5;

                if (cursor.CharIsEqualToIndex("#! "))
                    determined = true;
            }
            return ALFBTVersion.UnknownVersion;
        }
    }
}
