using Cobilas.Collections;
using System.Collections.Generic;

namespace System {
    public static class Enum_CB_Extension {

        public static bool CompareFlag(this Enum E, Enum flag) {
            if (!E.CompareType(flag.GetType()))
                throw new InvalidCastException("The flag parameter is not the same type!");
            return E.ToString() == flag.ToString();
        }

        public static bool CompareFlag(this Enum E, Enum[] flags) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(flags); I++)
                if (CompareFlag(E, flags[I]))
                    return true;
            return false;
        }

        /// <summary>Verifica se o <seealso cref="Enum"/> contem uma marca em especifico.</summary>
        /// <param name="flag">Bandeira.</param>
        public static bool ContainsFlag(this Enum E, Enum flag) {
            if (!E.CompareType(flag.GetType()))
                throw new InvalidCastException("The flag parameter is not the same type!");
            KeyValuePair<string, int> p1 = GetEnumPair(E);
            KeyValuePair<string, int> p2 = GetEnumPair(flag);
            if (p1.Value == 0 && p2.Value == 0) return true;
            else if (p1.Value == 0 || p2.Value == 0) return false;
            return (p1.Value & p2.Value) == p2.Value;
        }

        /// <summary>Verifica se o <seealso cref="Enum"/> contem uma marca em especifico.</summary>
        /// <param name="flags">Bandeiras.</param>
        public static bool ContainsFlag(this Enum E, params Enum[] flags) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(flags); I++)
                if (ContainsFlag(E, flags[I]))
                    return true;
            return false;
        }

        public static KeyValuePair<string, int> GetEnumPair(this Enum E) {
            KeyValuePair<string, int>[] pairs = GetEnumPairs(E);
            if (E.ToString().Contains(',')) {
                string[] flags = E.ToString().Split(',');
                int res = 0;
                for (int A = 0; A < flags.Length; A++)
                    for (int B = 0; B < pairs.Length; B++)
                        if (pairs[B].Key == flags[A].Trim())
                            res |= pairs[B].Value;
                return new KeyValuePair<string, int>(E.ToString(), res);
            } else {
                for (int I = 0; I < ArrayManipulation.ArrayLength(pairs); I++)
                    if (pairs[I].Key == Enum.GetName(E.GetType(), E))
                        return pairs[I];
            }
            return default;
        }

        public static KeyValuePair<string, int>[] GetEnumPairs(this Enum E) {
            Array array = Enum.GetValues(E.GetType());
            KeyValuePair<string, int>[] Res = (KeyValuePair<string, int>[])null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(array); I++)
                ArrayManipulation.Add(new KeyValuePair<string, int>(
                    array.GetValue(I).ToString(),
                    (int)array.GetValue(I)
                    ), ref Res);
            return Res;
        }

        public static string GetName(this Enum E, object value)
            => Enum.GetName(E.GetType(), value);

        public static string GetName(this Enum E)
            => GetName(E, E);

        public static string[] GetNames(this Enum E)
            => Enum.GetNames(E.GetType());
    }
}
