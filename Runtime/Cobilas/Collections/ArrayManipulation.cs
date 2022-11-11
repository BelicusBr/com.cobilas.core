using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cobilas.Collections {
    public static class ArrayManipulation {
        public static event Action<object> ExceptionEvent;
        private static bool _autoClear;
        private static int autoClearLength;
        private static Queue<Exception> exceptions;

        public static Exception[] ExceptionsList => exceptions.ToArray();
        public static Exception FirstError => exceptions.Count == 0 ? (Exception)null : ExceptionsList[0];
        public static Exception LastError => exceptions.Count == 0 ? (Exception)null : ExceptionsList[exceptions.Count - 1];

        static ArrayManipulation() {
            exceptions = new Queue<Exception>();
            _autoClear = true;
            autoClearLength = 5;
            ExceptionEvent = null;
        }

        public static void AutoClearExceptionsList(bool autoClear, int length = 5) {
            _autoClear = autoClear;
            autoClearLength = length;
        }

        private static void AddException(Exception e) {
            if (ExceptionEvent != null) ExceptionEvent(e);
            if (_autoClear) {
                if (exceptions.Count < autoClearLength) exceptions.Enqueue(e);
                else {
                    int length = (exceptions.Count - autoClearLength) - 1;
                    length = length < 0 ? 0 : length;
                    for (int I = 0; I < length; I++)
                        _ = exceptions.Dequeue();
                    exceptions.Enqueue(e);
                }
            }
            else exceptions.Enqueue(e);
        }

        //Insert
        public static T[] Insert<T>(T[] itens, int index, T[] list) {
            try {
                if (list == null) list = new T[0];
                T[] newList = new T[list.Length + itens.Length];
                Array.Copy(list, 0, newList, 0, index);
                Array.Copy(itens, 0, newList, index, itens.Length);
                Array.Copy(list, index, newList, itens.Length + index, list.Length - index);
                return newList;
            } catch (Exception e) {
                AddException(e);
                return list;
            }
        }

        public static T[] Insert<T>(T item, int index, T[] list)
            => Insert<T>(new T[] { item }, index, list);

        public static T[] Insert<T>(IEnumerator<T> itens, int index, T[] list) {
            try {
                while (itens.MoveNext())
                    list = Insert<T>(itens.Current, index, list);
            } catch (Exception e) {
                AddException(e);
            }
            return list;
        }

        public static void Insert<T>(T[] itens, int index, ref T[] list)
            => list = Insert<T>(itens, index, list);

        public static void Insert<T>(T item, int index, ref T[] list)
            => list = Insert<T>(item, index, list);

        public static T[] AddNon_Existing<T>(T item, T[] list) {
            if (!Exists(item, list))
                return Add(item, list);
            return list;
        }

        public static void AddNon_Existing<T>(T item, ref T[] list)
            => list = AddNon_Existing<T>(item, list);

        public static T[] Add<T>(T[] itens, T[] list)
            => Insert<T>(itens, ArrayLength(list), list);

        public static T[] Add<T>(IEnumerator<T> itens, T[] list)
            => Insert<T>(itens, ArrayLength(list), list);

        public static void Add<T>(IEnumerator<T> itens, ref T[] list)
            => list = Add<T>(itens, list);

        public static T[] Add<T>(T item, T[] list)
            => Insert<T>(item, ArrayLength(list), list);

        public static void Add<T>(T[] itens, ref T[] list)
            => Insert<T>(itens, ArrayLength(list), ref list);

        public static void Add<T>(T item, ref T[] list)
            => Insert<T>(item, ArrayLength(list), ref list);

        public static T[] Remove<T>(int index, int length, T[] list) {
            try {
                T[] newList = new T[list.Length - length];
                Array.Copy(list, 0, newList, 0, index);
                Array.Copy(list, index + length, newList, index, list.Length - (index + length));
                return newList;
            } catch (Exception e) {
                AddException(e);
                return list;
            }
        }

        public static void Remove<T>(int index, int length, ref T[] list)
            => list = Remove<T>(index, length, list);

        public static T[] Remove<T>(int index, T[] list)
            => Remove<T>(index, 1, list);

        public static void Remove<T>(int index, ref T[] list)
            => list = Remove<T>(index, list);

        public static T[] Remove<T>(T item, T[] list)
            => Remove<T>(IndexOf(item, list), list);

        public static void Remove<T>(T item, ref T[] list)
            => list = Remove<T>(item, list);

        public static void ClearArray(Array array) {
            try { Array.Clear(array, 0, array.Length); }
            catch (Exception e) { AddException(e); }
        }

        public static void ClearArray<T>(ref T[] array) {
            try {
                Array.Clear(array, 0, array.Length);
                array = null;
            } catch (Exception e) { AddException(e); }
        }

        public static void ClearArraySafe(Array array) {
            if (!EmpytArray(array))
                ClearArray(array);
        }

        public static void ClearArraySafe<T>(ref T[] array) {
            if (!EmpytArray(array)) {
                ClearArray(array);
                array = null;
            }
        }

        public static void SeparateList<T>(T[] list, int separationIndex, out T[] part1, out T[] part2) {
            try {
                Array.Copy(list, 0, part1 = new T[separationIndex + 1], 0, separationIndex + 1);
                Array.Copy(list, separationIndex + 1, part2 = new T[list.Length - (separationIndex + 1)], 0, list.Length - (separationIndex + 1));
            } catch (Exception e) { 
                AddException(e);
                part1 = part2 = (T[])null;
            }
        }

        public static T[] TakeStretch<T>(int index, int length, T[] list) {
            try {
                T[] Res = new T[length];
                CopyTo(list, index, Res, 0, length);
                return Res;
            } catch (Exception e) {
                AddException(e);
                return (T[])null;
            }
        }

        public static ReadOnlyCollection<T> ReadOnly<T>(T[] list) {
            try {
                return Array.AsReadOnly<T>(list);
            } catch (Exception e) {
                AddException(e);
                return new ReadOnlyCollection<T>(new List<T>(0));
            }
        }

        public static int IndexOf(object item, Array array, int index, int length) {
            try {
                return Array.IndexOf(array, item, index, length);
            } catch (Exception e) {
                AddException(e);
                return -1;
            }
        }

        public static int IndexOf(object item, Array array, int index)
            => IndexOf(item, array, index, array.Length);

        public static int IndexOf(object item, Array array)
            => IndexOf(item, array, 0);

        public static bool Exists(object item, Array array) {
            for (int I = 0; I < ArrayLength(array); I++)
                if (array.GetValue(I) == item)
                    return true;
            return false;
        }

        public static void CopyTo(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length) {
            try {
                Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
            } catch (Exception e) {
                AddException(e);
            }
        }

        public static void CopyTo(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
            => CopyTo(sourceArray, (long)sourceIndex, destinationArray, (long)destinationIndex, (long)length);

        public static void CopyTo(Array sourceArray, Array destinationArray, long length)
            => CopyTo(sourceArray, 0, destinationArray, 0, length);

        public static void CopyTo(Array sourceArray, Array destinationArray, int length)
            => CopyTo(sourceArray, 0, destinationArray, 0, length);

        public static void CopyTo(Array sourceArray, Array destinationArray)
            => CopyTo(sourceArray, 0, destinationArray, 0, sourceArray.Length);

        public static void Reverse(Array array) {
            try {
                Array.Reverse(array, 0, array.Length);
            } catch (Exception e) {
                AddException(e);
            }
        }

        public static bool EmpytArray(ICollection array)
            => array == null ? true : array.Count == 0;

        public static int ArrayLength(ICollection array)
            => array == null ? 0 : array.Count;
    }
}
