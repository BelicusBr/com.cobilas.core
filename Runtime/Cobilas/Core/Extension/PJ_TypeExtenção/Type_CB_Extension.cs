using Cobilas.Collections;

namespace System {
    public static class Type_CB_Extension {
        public static T GetAttribute<T>(this Type type) where T : Attribute
            => GetAttribute<T>(type, true);

        public static T GetAttribute<T>(this Type type, bool inherit) where T : Attribute {
            T[] types = GetAttributes<T>(type, inherit);
            return ArrayManipulation.EmpytArray(types) ? (T)null : types[0];
        }

        public static T[] GetAttributes<T>(this Type type) where T : Attribute
            => GetAttributes<T>(type, true);

        public static T[] GetAttributes<T>(this Type type, bool inherit) where T : Attribute
            => (T[])type.GetCustomAttributes(typeof(T), inherit);
    }
}
