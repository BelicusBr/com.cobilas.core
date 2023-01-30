using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cobilas.Collections {
    /// <summary>Classe de manipulação de matriz.</summary>
    public static class ArrayManipulation {
        /// <summary>Insira um item no idice de um <see cref="Array"/>.</summary>
        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="index">Indice alvo.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] Insert<T>(T[] itens, int index, T[] list) {
            if (list == null) list = new T[0];
            T[] newList = new T[list.Length + itens.Length];
            Array.Copy(list, 0, newList, 0, index);
            Array.Copy(itens, 0, newList, index, itens.Length);
            Array.Copy(list, index, newList, itens.Length + index, list.Length - index);
            return newList;
        }

        /// <summary>Insira um item no idice de um <see cref="Array"/>.</summary>
        /// <param name="item">O iten a ser inserido no <see cref="Array"/>.</param>
        /// <param name="index">Indice alvo.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] Insert<T>(T item, int index, T[] list)
            => Insert<T>(new T[] { item }, index, list);

        /// <summary>Insira um item no idice de um <see cref="Array"/>.</summary>
        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="index">Indice alvo.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] Insert<T>(IEnumerator<T> itens, int index, T[] list) {
            while (itens.MoveNext())
                list = Insert<T>(itens.Current, index, list);
            return list;
        }

        /// <summary>Insira um item no idice de um <see cref="Array"/>.</summary>
        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="index">Indice alvo.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static void Insert<T>(T[] itens, int index, ref T[] list)
            => list = Insert<T>(itens, index, list);

        /// <summary>Insira um item no idice de um <see cref="Array"/>.</summary>
        /// <param name="item">O iten a ser inserido no <see cref="Array"/>.</param>
        /// <param name="index">Indice alvo.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static void Insert<T>(T item, int index, ref T[] list)
            => list = Insert<T>(item, index, list);

        /// <summary>Adicione itens que não estão no <see cref="Array"/>.</summary>
        /// <param name="item">O iten a ser inserido no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] AddNon_Existing<T>(T item, T[] list) {
            if (!Exists(item, list))
                return Add(item, list);
            return list;
        }

        /// <summary>Adicione itens que não estão no <see cref="Array"/>.</summary>
        /// <param name="item">O iten a ser inserido no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static void AddNon_Existing<T>(T item, ref T[] list)
            => list = AddNon_Existing<T>(item, list);

        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] Add<T>(T[] itens, T[] list)
            => Insert<T>(itens, ArrayLength(list), list);

        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] Add<T>(IEnumerator<T> itens, T[] list)
            => Insert<T>(itens, ArrayLength(list), list);

        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static void Add<T>(IEnumerator<T> itens, ref T[] list)
            => list = Add<T>(itens, list);

        /// <param name="item">O iten a ser inserido no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static T[] Add<T>(T item, T[] list)
            => Insert<T>(item, ArrayLength(list), list);

        /// <param name="itens">Os itens seren inseridos no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static void Add<T>(T[] itens, ref T[] list)
            => Insert<T>(itens, ArrayLength(list), ref list);

        /// <param name="item">O iten a ser inserido no <see cref="Array"/>.</param>
        /// <param name="list"><see cref="Array"/> alvo.</param>
        public static void Add<T>(T item, ref T[] list)
            => Insert<T>(item, ArrayLength(list), ref list);

        public static T[] Remove<T>(int index, int length, T[] list) {
            T[] newList = new T[list.Length - length];
            Array.Copy(list, 0, newList, 0, index);
            Array.Copy(list, index + length, newList, index, list.Length - (index + length));
            return newList;
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

        /// <summary>Limpa uma lista.</summary>
        public static void ClearArray(Array array)
            => Array.Clear(array, 0, array.Length);

        /// <summary>Limpa uma lista.</summary>
        public static void ClearArray<T>(ref T[] array) {
            Array.Clear(array, 0, array.Length);
            array = null;
        }

        /// <summary>Limpa uma lista.</summary>
        public static void ClearArraySafe(Array array) {
            if (!EmpytArray(array))
                ClearArray(array);
        }

        /// <summary>Limpa uma lista.</summary>
        public static void ClearArraySafe<T>(ref T[] array) {
            if (!EmpytArray(array)) {
                ClearArray(array);
                array = null;
            }
        }

        public static void SeparateList<T>(T[] list, int separationIndex, out T[] part1, out T[] part2) {
            Array.Copy(list, 0, part1 = new T[separationIndex + 1], 0, separationIndex + 1);
            Array.Copy(list, separationIndex + 1, part2 = new T[list.Length - (separationIndex + 1)], 0, list.Length - (separationIndex + 1));
        }

        public static T[] TakeStretch<T>(int index, int length, T[] list) {
            T[] Res = new T[length];
            CopyTo(list, index, Res, 0, length);
            return Res;
        }

        /// <summary>Cria uma lista somente de leitura.</summary>
        /// <param name="list">A lista que será somente leitura.</param>
        public static ReadOnlyCollection<T> ReadOnly<T>(T[] list)
            => Array.AsReadOnly<T>(list);

        /// <summary>Cria uma lista somente de leitura.</summary>
        /// <param name="list">A lista que será somente leitura.</param>
        public static ReadOnlyCollection<T> ReadOnlySafe<T>(T[] list)
            => EmpytArray(list) ? new ReadOnlyCollection<T>(new List<T>()) : ReadOnly<T>(list);

        public static int IndexOf(object item, Array array, int index, int length)
            => Array.IndexOf(array, item, index, length);

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

        public static void CopyTo(Array sourceArray, long sourceIndex, Array destinationArray, long destinationIndex, long length)
            => Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);

        public static void CopyTo(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
            => CopyTo(sourceArray, (long)sourceIndex, destinationArray, (long)destinationIndex, (long)length);

        public static void CopyTo(Array sourceArray, Array destinationArray, long length)
            => CopyTo(sourceArray, 0, destinationArray, 0, length);

        public static void CopyTo(Array sourceArray, Array destinationArray, int length)
            => CopyTo(sourceArray, 0, destinationArray, 0, length);

        public static void CopyTo(Array sourceArray, Array destinationArray)
            => CopyTo(sourceArray, 0, destinationArray, 0, sourceArray.Length);

        /// <summary>Inverte uma lista.</summary>
        public static void Reverse(Array array)
            => Array.Reverse(array, 0, array.Length);

        /// <summary>Indica se a lista está vazia.</summary>
        public static bool EmpytArray(ICollection array)
            => array == null || array.Count == 0;

        /// <summary>Indica o comprimento da lista.</summary>
        public static int ArrayLength(ICollection array)
            => array == null ? 0 : array.Count;

        public static long ArrayLongLength(Array array)
            => array == null ? 0L : array.LongLength;
    }
}
