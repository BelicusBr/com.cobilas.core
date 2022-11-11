using Cobilas.Collections;
using System.Globalization;

namespace System {
    public static class String_CB_Extension {

        public static bool Contains(this string S, params char[] value) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(value); I++)
                if (S.Contains(value[I].ToString()))
                    return true;
            return false;
        }

        public static sbyte ToSByte(this string S)
            => sbyte.Parse(S);

        public static short ToShort(this string S)
            => short.Parse(S);

        public static int ToInt(this string S)
            => int.Parse(S);

        public static long ToLong(this string S)
            => long.Parse(S);

        public static byte ToByte(this string S)
            => byte.Parse(S);

        public static ushort ToUShort(this string S)
            => ushort.Parse(S);

        public static uint ToUInt(this string S)
            => uint.Parse(S);

        public static ulong ToULong(this string S)
            => ulong.Parse(S);

        public static float ToFloat(this string S)
            => float.Parse(S, CultureInfo.InvariantCulture);

        public static double ToDouble(this string S)
            => double.Parse(S, CultureInfo.InvariantCulture);

        public static decimal ToDecimal(this string S)
            => decimal.Parse(S, CultureInfo.InvariantCulture);
    }
}
