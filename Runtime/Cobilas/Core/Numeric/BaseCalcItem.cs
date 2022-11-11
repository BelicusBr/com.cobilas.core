using Cobilas.Collections;

namespace Cobilas.Numeric {
    public abstract class BaseCalcItem {
        public abstract string Item { get; }

        public static int IndexOf(BaseCalcItem[] values, params string[] sings)
            => IndexOf(0, values, sings);

        public static int IndexOf(int StartIndex, BaseCalcItem[] values, params string[] sings) {
            for (int I = StartIndex; I < ArrayManipulation.ArrayLength(values); I++)
                foreach (string item in sings)
                    if ((string)values[I] == item)
                        return I;
            return -1;
        }

        public static int LastIndexOf(BaseCalcItem[] values, params string[] sings)
            => LastIndexOf(ArrayManipulation.ArrayLength(values) - 1, values, sings);

        public static int LastIndexOf(int StartIndex, BaseCalcItem[] values, params string[] sings) {
            for (int I = StartIndex; I >= 0; I--)
                foreach (string item in sings)
                    if ((string)values[I] == item)
                        return I;
            return -1;
        }

        public static explicit operator string(BaseCalcItem A)
            => (string)A.Item.Clone();
    }
}
