using System;
using System.Linq;

namespace Cobilas.IO.Alf.Components {
    public static class ALFUtility {
        public const string n_Type = "type";
        public const string alf_Type = ".alf";
        public const string alf_Version = "1.0";
        public const string n_Version = "version";
        public const string n_Comment = "comment";
        public const string n_Encoding = "encoding";
        public const string n_BreakLine = "breakline";
        public static readonly char[] InvalidTextCharacters = { '\\', ':', '[', ']', '<', '>' };
        public static readonly string[] EscapesString = { "\\\\", "\\:", "\\[", "\\]", "\\<", "\\>" };

        /// <summary>
        /// <code>
        /// => ThisNameIsValid(name, ValidNameCharacter);
        /// </code>
        /// </summary>
        public static bool ThisNameIsValid(string name)
            => ThisNameIsValid(name, ValidNameCharacter);

        public static bool ThisNameIsValid(string name, Func<char, bool> func)
            => name.All(func);

        public static bool TheTextIsValid(string text, Func<char[], bool> func)
            => func(text.ToCharArray());

        /// <summary>
        /// <code>
        /// => TheTextIsValid(text, (chs) => {
        ///      using (CharacterCursor cursor = new CharacterCursor(chs))
        ///          while (cursor.MoveToCharacter()) {
        ///             if (cursor.CharIsEqualToIndex(EscapesString)) {
        ///                 cursor.MoveToCharacter(1L);
        ///                 continue;
        ///             } else if (cursor.CharIsEqualToIndex(InvalidTextCharacters))
        ///                 return false;
        ///          }
        ///      return true;
        ///  });
        /// </code>
        /// </summary>
        public static bool TheTextIsValid(string text)
            => TheTextIsValid(text, (chs) => {
                using (CharacterCursor cursor = new CharacterCursor(chs))
                    while (cursor.MoveToCharacter()) {
                        if (cursor.CharIsEqualToIndex(EscapesString)) {
                            cursor.MoveToCharacter(1L);
                            continue;
                        } else if (cursor.CharIsEqualToIndex(InvalidTextCharacters))
                            return false;
                    }
                return true;
            });

        public static string GetTabs(int depth, bool indent)
            => indent ? string.Empty.PadRight(depth, '\t') : string.Empty;

        public static bool IsWhiteSpace(char c) {
            if (char.IsWhiteSpace(c))
                return true;
            return char.IsControl(c);
        }

        /// <summary>
        /// <code>
        /// => ValidNameCharacter(c, (ch) => {
        ///     switch (ch) {
        ///         case '\\': case '/':
        ///         case '_': case '.': 
        ///             return true;
        ///         default: return char.IsLetterOrDigit(ch);
        ///     }});
        /// </code>
        /// </summary>
        public static bool ValidNameCharacter(char c)
            => ValidNameCharacter(c, (ch) => {
                switch (ch) {
                    case '\\': case '/':
                    case '_': case '.': 
                        return true;
                    default: return char.IsLetterOrDigit(ch);
                }
            });

        public static bool ValidNameCharacter(char c, Func<char, bool> func)
            => func(c);

        public static bool ValidTextCharacter(char c, Func<char, bool> func)
            => func(c);

        /// <summary>
        /// <code>
        /// => ValidTextCharacter(c, (ch) => {
        ///     switch (ch) {
        ///         case ':': case '[':
        ///         case ']': case '&lt;':
        ///         case '&gt;': return false;
        ///         default: return true;
        ///     }});
        /// </code>
        /// </summary>
        public static bool ValidTextCharacter(char c)
            => ValidTextCharacter(c, (ch) => {
                switch (ch) {
                    case ':': case '[':
                    case ']': case '<':
                    case '>': return false;
                    default: return true;
                }
            });
    }
}
