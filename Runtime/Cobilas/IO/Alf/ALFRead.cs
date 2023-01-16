using System;
using System.IO;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Components.Collections;

namespace Cobilas.IO.Alf {
    public sealed class ALFRead : IDisposable {
        private readonly ALFItem root;
        private bool disposedValue;

        public ALFItemReadOnly ReadOnly => new ALFItemReadOnly(root);

        private ALFRead() {
            root = new ALFItem();
            root.isRoot = true;
            root.name = "Root";
        }

        public void Dispose() {
            if (disposedValue)
                throw new ObjectDisposedException($"The object {typeof(ALFWriter)} has already been discarded");
            disposedValue = true;
            root?.Dispose();
        }

        public static ALFRead Create(TextReader reader) {
            ALFRead read = new ALFRead();
            string[] lines = (string[])null;
            string line;
            while ((line = reader.ReadLine()) != (string)null)
                ArrayManipulation.Add(line, ref lines);
            ALFCompiler.Read(read.root, lines);
            return read;
        }

        public static ALFRead Create(Stream stream, Encoding encoding) {
            using (StringReader reader = new StringReader(stream.GetString(encoding)))
                return Create(reader);
        }

        public static ALFRead Create(Stream stream)
            => Create(stream, Encoding.UTF8);
    }
}
