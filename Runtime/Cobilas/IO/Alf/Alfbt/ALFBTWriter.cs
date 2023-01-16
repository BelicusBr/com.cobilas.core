using System;
using System.IO;
using System.Text;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Alfbt.Components.Compiler;

namespace Cobilas.IO.Alf.Alfbt {
    public sealed class ALFBTWriter : IDisposable {
        private Stream stream;
        private Encoding encoding;
        private TextWriter textWriter;
        private bool disposed;
        private bool writingStarted;
        private bool headerStarted;
        private readonly ALFItem root;

        private ALFBTWriter() {
            disposed = false;
            root = new ALFItem();
            root.isRoot = true;
            root.name = "Root";
            stream = (Stream)null;
            encoding = (Encoding)null;
            textWriter = (TextWriter)null;
        }

        public void WriterHeaderFlag() {
            if (writingStarted || headerStarted)
                throw ALFBTFormatException.GetHeaderSecond();
            headerStarted = true;
            WriteFlag(ALFBTCompiler_1_0.n_Version, ALFBTCompiler.version);
            WriteFlag(ALFBTCompiler_1_0.n_Type, ".alfbt");
            WriteFlag(ALFBTCompiler_1_0.n_Encoding, IsStrem() ? encoding.BodyName : textWriter.Encoding.BodyName);
        }

        public void WriterMarkingFlag(string name, string value) {
            if (string.IsNullOrEmpty(value))
                throw ALFBTFormatException.GetEmptyValue(false);
            if (value.Contains('\n'))
                throw ALFBTFormatException.GetException(
                    "Markup flag value cannot contain line break!",
                    "O valor de bandeira de marcação não pode conter quebra de linha!"
                    );
            writingStarted = true;
            WriteFlag(name, value);
        }

        public void WriteSpacing()
            => WriteSpacing(0);

        public void WriteSpacing(int spacings) {
            for (int I = 0; I < spacings; I++) {
                if (!headerStarted)
                    writingStarted = false;
                WriteFlag(ALFBTCompiler_1_0.n_BreakLine, "\r\n");
            }
        }

        public void WriterCommentFlag(string text) {
            WriteFlag(ALFBTCompiler_1_0.n_Comment, text);
            if (!headerStarted)
                writingStarted = false;
        }

        public void WriterTextFlag(string name, string value) {
            writingStarted = true;
            WriteFlag(name, string.IsNullOrEmpty(value) ? "\r\n" : value);
        }

        [Obsolete("Use WriteSpacing()")]
        public void WriteLineBreak()
            => WriteSpacing();

        public void Dispose() {
            if (disposed)
                throw new ObjectDisposedException($"The object {typeof(ALFWriter)} has already been discarded");
            disposed = true;
            StringBuilder builder = new StringBuilder();
            ALFBTCompiler.Writer(root, builder);

            if (IsStrem()) {
                byte[] bytes = encoding.GetBytes(builder.ToString());
                stream.Write(bytes);
            } else {
                textWriter.Write(builder.ToString());
            }

            root.Dispose();
            stream = (Stream)null;
            encoding = (Encoding)null;
            textWriter = (TextWriter)null;
        }

        private void WriteFlag(string name, string value) {
            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetEmptyValue();
            ALFItem item = new ALFItem();
            item.name = name;
            item.text.Append(value);
            root.Add(item);
        }

        private bool IsStrem()
            => stream != (Stream)null;

        public static ALFBTWriter Create(TextWriter writer) {
            ALFBTWriter a_writer = new ALFBTWriter();
            a_writer.textWriter = writer;
            return a_writer;
        }

        public static ALFBTWriter Create(Stream stream, Encoding encoding) {
            ALFBTWriter a_writer = new ALFBTWriter();
            a_writer.stream = stream;
            a_writer.encoding = encoding;
            return a_writer;
        }

        public static ALFBTWriter Create(Stream stream)
            => Create(stream, Encoding.UTF8);
    }
}
