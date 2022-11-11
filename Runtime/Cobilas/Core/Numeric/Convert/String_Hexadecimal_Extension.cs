using System.Linq;
using Cobilas.Collections;

namespace Cobilas.Numeric.Convert {
    public static class String_Hexadecimal_Extension {
        public static bool IsHexadecimal(this string str)
            => str.All((c) => {
                switch (c) {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'X': return true;
                    default: return false;
                }
            });

        public static decimal HexToDecimal(this string str) {
            char[] carac = str.ToLower().Replace("x", "").Remove(0, 1).ToUpper().ToCharArray();
            ArrayManipulation.Reverse(carac);
            decimal res = 0;
            for (int I = 0; I < carac.Length; I++)
                res += HexToByte(carac[I]) * BitArray_Binary_Extension.Pow(16, I);
            return res;
        }

        private static byte HexToByte(char c) {
            switch (c) {
                case 'A': return 10;
                case 'B': return 11;
                case 'C': return 12;
                case 'D': return 13;
                case 'E': return 14;
                case 'F': return 15;
                default: return byte.Parse(c.ToString());
            }
        }
    }
}
