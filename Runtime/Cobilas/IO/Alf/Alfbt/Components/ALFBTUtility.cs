using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf.Alfbt.Components {
    public static class ALFBTUtility {
        public const string alfbt_Type = ".alfbt";
        public const string alfbt_Version = "1.5";
        public const string n_Type = ALFUtility.n_Type;
        public const string n_Version = ALFUtility.n_Version;
        public const string n_Comment = ALFUtility.n_Comment;
        public const string n_Encoding = ALFUtility.n_Encoding;
        public const string n_BreakLine = ALFUtility.n_BreakLine;
        public static readonly char[] InvalidTextCharacters = { '\\', '*', '/' };
        public static readonly string[] EscapesString = { "\\\\", "\\*", "\\/" };

        /// <summary>
        /// <code>
        /// => <see cref="ALFUtility"/>.ThisNameIsValid(name, <see cref="ALFUtility"/>.ValidNameCharacter);
        /// </code>
        /// </summary>
        public static bool ThisNameIsValid(string name)
            => ALFUtility.ThisNameIsValid(name, ALFUtility.ValidNameCharacter);

        /// <summary>
        /// <code>
        /// => <see cref="ALFUtility"/>.TheTextIsValid(text, (chs) => {
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
            => ALFUtility.TheTextIsValid(text, (chs) => {
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
            => ALFUtility.GetTabs(depth, indent);

        public static bool IsWhiteSpace(char c)
            => ALFUtility.IsWhiteSpace(c);

        /// <summary>
        /// <code>
        /// => <see cref="ALFUtility"/>.ValidTextCharacter(c, (ch) => {
        ///     switch (ch) {
        ///         case '\\':
        ///         case '/':
        ///         case '*': return false;
        ///         default: return true;
        ///     }});
        /// </code>
        /// </summary>
        public static bool ValidTextCharacter(char c)
            => ALFUtility.ValidTextCharacter(c, (ch) => {
                switch (ch) {
                    case '\\':
                    case '/':
                    case '*': return false;
                    default: return true;
                }
            });
    }
}
