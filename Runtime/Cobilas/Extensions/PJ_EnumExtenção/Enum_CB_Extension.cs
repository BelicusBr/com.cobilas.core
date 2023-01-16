using Cobilas.Collections;

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

        public static string GetName(this Enum E, object value)
            => Enum.GetName(E.GetType(), value);

        public static string GetName(this Enum E)
            => GetName(E, E);

        public static string[] GetNames(this Enum E)
            => Enum.GetNames(E.GetType());
    }
}
