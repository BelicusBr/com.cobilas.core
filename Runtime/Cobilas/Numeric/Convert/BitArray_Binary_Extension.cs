using System.Collections;

namespace Cobilas.Numeric.Convert {
    public static class BitArray_Binary_Extension {
        public static byte[] GetBinaryArray(this BitArray array) {
            byte[] res = new byte[array.Length];
            for (int I = 0; I < array.Length; I++)
                res[I] = (byte)(array.Get(I) ? 1 : 0);
            return res;
        }

        public static void SetBinaryArray(this BitArray array, byte[] binaryArray) {
            for (int I = 0; I < binaryArray.Length; I++)
                array.Set(I, binaryArray[I] == 1);
        }

        public static decimal BitToDecimal(this BitArray array) {
            byte[] bits = GetBinaryArray(array);
            decimal res = 0;
            for (int I = 0; I < bits.Length; I++)
                res += bits[I] * Pow(2, I);
            return res;
        }

        internal static decimal Pow(decimal x, decimal y) {
            if (y <= 0) return 1;
            decimal I_Res = x;
            for (int I = 1; I < y; I++)
                I_Res *= x;
            return I_Res;
        }
    }
}
