using Cobilas.Collections;

namespace System {
    public static class Object_CB_Extension {
        public static bool CompareType(this object O, Type type)
            => O.GetType() == type;

        public static bool CompareType(this object O, params Type[] types) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(types); I++)
                if (types[I] == O.GetType()) 
                    return true;
            return false;
        }

        public static bool CompareType<T>(this object O)
            => CompareType(O, typeof(T));
    }
}
