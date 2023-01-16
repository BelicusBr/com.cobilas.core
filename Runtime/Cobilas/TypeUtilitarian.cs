using System;
using System.Reflection;
using Cobilas.Collections;

namespace Cobilas {
    public static class TypeUtilitarian {
        public static bool TypeExist(string fullName) {
            Assembly[] assemblies = GetAssemblies();
            for (int A = 0; A < ArrayManipulation.ArrayLength(assemblies); A++) {
                Type[] types = assemblies[A].GetTypes();
                for (int B = 0; B < ArrayManipulation.ArrayLength(types); B++)
                    if (types[B].Name == fullName)
                        return true;
            }
            return false;
        }

        public static Assembly[] GetAssemblies()
            => AppDomain.CurrentDomain.GetAssemblies();
    }
}
