using System;
using System.Collections;
using Cobilas.Collections;
using System.Collections.Generic;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt.Language {
    public sealed class LanguageManager : IDisposable, IEnumerable<LanguageCollection> {
        private int count;
        private bool disposedValue;
        private LanguageCollection[] collections;

        public int Count => count;
        public int Capacity => ArrayManipulation.ArrayLength(collections);

        public LanguageCollection this[int index] => GetLanguageCollection(index);
        public LanguageCollection this[string lang_target] => GetLanguageCollection(IndexOf(lang_target));

        public LanguageManager(int Capacity) {
            collections = new LanguageCollection[Capacity];
            count = 0;
        }

        public LanguageManager() : this(0) { }

        ~LanguageManager()
            => Dispose(disposing: false);

        public void Dispose() {
            CheckIfItWasDiscarded();
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public int IndexOf(string lang_target) {
            CheckIfItWasDiscarded();
            for (int I = 0; I < Count; I++)
                if (collections[I].GetManifestText(nameof(lang_target)) == lang_target)
                    return I;
            return -1;
        }

        public bool Add(IItemReadOnly comunaManifest, IItemReadOnly language) {
            CheckIfItWasDiscarded();
            if (Contains(comunaManifest[comunaManifest.IndexOf("lang_target")].Name)) return false;
            Add(new LanguageCollection((IItemReadOnly)comunaManifest.Clone(), (IItemReadOnly)language.Clone()));
            return true;
        }

        public bool Contains(string lang_target)
            => IndexOf(lang_target) >= 0;

        public string GetLanguageText(int index, string path) {
            CheckIfItWasDiscarded();
            return collections[index].GetLanguageText(path);
        }

        public string GetLanguageText(string lang_target, string path) {
            CheckIfItWasDiscarded();
            int index = IndexOf(lang_target);
            if (index < 0) return (string)null;
            return collections[index].GetLanguageText(path);
        }

        public IEnumerator<LanguageCollection> GetEnumerator() {
            CheckIfItWasDiscarded();
            return new LanguageCollectionToIEnumerator(count, collections);
        }

        private LanguageCollection GetLanguageCollection(int index) {
            if (index >= count)
                throw new IndexOutOfRangeException();
            return collections[index];
        }

        private void Add(LanguageCollection collection) {
            if (count == Capacity)
                Array.Resize(ref collections, count + 1);
            collections[count] = collection;
            ++count;
        }

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    foreach (var item in collections)
                        item?.Dispose();
                    count = 0;
                    ArrayManipulation.ClearArraySafe(ref collections);
                }
                disposedValue = true;
            }
        }

        private void CheckIfItWasDiscarded() {
            if (!disposedValue) return;
            throw new ObjectDisposedException($"The object {GetType()} has already been discarded");
        }

        IEnumerator IEnumerable.GetEnumerator()
            => (this as IEnumerable<LanguageCollection>).GetEnumerator();
    }
}
