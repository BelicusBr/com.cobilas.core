using System.Linq;

namespace Cobilas.IO.Alf.Alfbt.Extension {
    internal static class StringExtension {
        public static bool AlfbtIsWhiteSpace(this string str)
            => str.All(char.IsWhiteSpace);

        public static bool AlfbtValidText(this string str, out char errorChar) {
            char OutErrorChar = '\0';
            bool res = str.All((c) => {
                if (char.IsLetterOrDigit(c)) return true;
                switch (c) {
                    case '-':
                    case '.':
                    case '_':
                        return true;
                    default:
                        OutErrorChar = c;
                        return false;
                }
            });
            errorChar = OutErrorChar;
            return res;
        }

        public static bool AlfbtValidName(this string str, out char errorChar) {
            char OutErrorChar = '\0';
            bool res = str.All((c) => {
                if (char.IsLetterOrDigit(c)) return true;
                switch (c) {
                    case '\\':
                    case '/':
                    case '.':
                    case '_':
                        return true;
                    default:
                        OutErrorChar = c;
                        return false;
                }
            });
            errorChar = OutErrorChar;
            return res;
        }
    }
}
