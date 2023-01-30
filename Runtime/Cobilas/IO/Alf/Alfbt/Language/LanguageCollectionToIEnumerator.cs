using System;
using System.Collections;
using System.Collections.Generic;

namespace Cobilas.IO.Alf.Alfbt.Language {
    public sealed class LanguageCollectionToIEnumerator : IEnumerator<LanguageCollection> {
        private int currentSize;
        private bool disposedValue;
        private readonly int sizeList;
        private LanguageCollection[] collections;

        object IEnumerator.Current => collections[currentSize];
        public LanguageCollection Current => collections[currentSize];

        public LanguageCollectionToIEnumerator(int sizeList, LanguageCollection[] collections) {
            this.sizeList = sizeList;
            this.collections = collections;
            Reset();
        }

        ~LanguageCollectionToIEnumerator()
            => Dispose(disposing: false);

        public bool MoveNext()
            => ++currentSize < sizeList;

        public void Reset() {
            currentSize = -1;
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void CheckIfItWasDiscarded() {
            if (!disposedValue) return;
            throw new ObjectDisposedException($"The object {GetType()} has already been discarded");
        }

        private void Dispose(bool disposing) {
            CheckIfItWasDiscarded();
            if (!disposedValue) {
                if (disposing) {
                    collections = (LanguageCollection[])null;
                    currentSize = sizeList;
                }
                disposedValue = true;
            }
        }
    }
}
