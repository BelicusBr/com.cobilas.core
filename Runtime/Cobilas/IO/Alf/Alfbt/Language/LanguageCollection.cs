using System;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf.Alfbt.Language {
    public sealed class LanguageCollection : IDisposable {
        private bool disposedValue;
        private readonly IItemReadOnly language;
        private readonly IItemReadOnly comunaManifest;

        public LanguageCollection(IItemReadOnly comunaManifest, IItemReadOnly language) {
            this.language = language;
            this.comunaManifest = comunaManifest;
        }

        ~LanguageCollection()
            => Dispose(disposing: false);

        public string GetManifestText(string flagName) {
            int index = comunaManifest.IndexOf(flagName);
            if (index < 0) return (string)null;
            return Convert.ToString(comunaManifest[index]);
        }

        public string GetLanguageText(string path) {
            int index = language.IndexOf(path);
            if (index < 0) return (string)null;
            return Convert.ToString(language[index]);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    language.Dispose();
                    comunaManifest.Dispose();
                }
                disposedValue = true;
            }
        }
    }
}
