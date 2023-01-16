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

        public static string TabPadRight(this string stg, int tabs)
            => string.Format("{0}{1}", stg, string.Empty.PadRight(tabs, '\t'));

        public static string TabPadLeft(this string stg, int tabs)
            => string.Format("{0}{1}", string.Empty.PadLeft(tabs, '\t'), stg);

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
