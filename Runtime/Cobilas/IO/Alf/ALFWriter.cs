using System;
using System.IO;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf {
    public sealed class ALFWriter : IDisposable {
        
        private Stream stream;
        private Encoding encoding;
        private TextWriter textWriter;
        private bool disposed;
        private bool writingStarted;
        private bool headerStarted;
        private readonly ALFItem root;
        private ALFItem temp;

        private ALFWriter() {
            disposed = false;
            temp = root = new ALFItem();
            temp.isRoot = true;
            temp.name = "Root";
            stream = (Stream)null;
            encoding = (Encoding)null;
            textWriter = (TextWriter)null;
        }

        public void StartElement(string name) {
            if (string.IsNullOrEmpty(name))
                throw ALFERROR.PrintError("the markup cannot be given a blank name.");

            if (name.Contains("\n") || name.Contains(":"))
                throw ALFERROR.PrintError("name cannot contain ('\\n', ':')\"{0}\"", name);

            writingStarted = true;
            ALFItem itemtemp = new ALFItem();
            itemtemp.name = name;
            itemtemp.parent = temp;
            temp.Add(itemtemp);
            temp = itemtemp;
        }

        public void StartElementHeader() {
            if (writingStarted || headerStarted)
                throw new ALFException("The header must start first.");
            headerStarted = true;
            StartElement("header");
                StartElement("version");
                    WriteText(ALFCompiler.version);
                EndElement();
                StartElement("type");
                    WriteText(".alf");
                EndElement();
            if (encoding != (Encoding)null) {
                StartElement("encoding");
                    WriteText(encoding.BodyName);
                EndElement();
            }
            if (textWriter != (TextWriter)null) {
                StartElement("encoding");
                    WriteText(textWriter.Encoding.BodyName);
                EndElement();
            }
            EndElement();
        }

        public void StartElementComment(string format, params object[] args) {
            StartElement("comment");
            WriteText(string.Format(format, args));
            EndElement();
            if (!headerStarted)
                writingStarted = false;
        }
        public void StartElementComment(string text) => StartElementComment("{0}", text);
        public void StartElementBreakLine(int breaks) {
            StartElement("breakline");
            for (int I = 0; I < breaks; I++)
                WriteText("\r\n");
            EndElement();
            if (!headerStarted)
                writingStarted = false;
        }
        public void StartElementBreakLine() => StartElementBreakLine(1);

        public void WriteText(bool value) => I_WriteText(value);
        public void WriteText(string value) => I_WriteText(value);
        public void WriteText(char value) => I_WriteText(value);
        public void WriteText(char[] value) => I_WriteText(value);
        public void WriteText(float value) => I_WriteText(value);
        public void WriteText(double value) => I_WriteText(value);
        public void WriteText(decimal value) => I_WriteText(value);
        public void WriteText(sbyte value) => I_WriteText(value);
        public void WriteText(short value) => I_WriteText(value);
        public void WriteText(int value) => I_WriteText(value);
        public void WriteText(long value)=> I_WriteText(value);
        public void WriteText(byte value) => I_WriteText(value);
        public void WriteText(ushort value) => I_WriteText(value);
        public void WriteText(uint value) => I_WriteText(value);
        public void WriteText(ulong value) => I_WriteText(value);
        public void WriteText(DateTime value) => I_WriteText(value);

        public void EndElement() {
            if (temp.isRoot)
                throw new ALFException("No element has been started!");
            ALFItem mtemp = temp.parent;
            temp = mtemp;
        }

        public void Dispose() {
            if (disposed)
                throw new ObjectDisposedException($"The object {typeof(ALFWriter)} has already been discarded");
            disposed = true;

            int count = ArrayManipulation.ArrayLength(root.itens);
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < count; I++)
                ALFCompiler.Writer(root.itens[I], 0, builder);

            if (IsStrem()) {
                byte[] chars = encoding.GetBytes(builder.ToString());
                stream.Write(chars, 0, chars.Length);
            } else {
                textWriter.Write(builder.ToString());
            }

            stream = (Stream)null;
            encoding = (Encoding)null;
            textWriter = (TextWriter)null;
        }

        private void I_WriteText(object value) {
            if (temp.isRoot)
                throw new ALFException("No element has been started!");
            temp.text.Append(value);
        }

        private void I_WriteText(char[] value) {
            if (temp.isRoot)
                throw new ALFException("No element has been started!");
            temp.text.Append(value);
        }

        private bool IsStrem()
            => stream != (Stream)null;

        public static ALFWriter Create(TextWriter text) {
            ALFWriter writer = new ALFWriter();
            writer.textWriter = text;
            return writer;
        }

        public static ALFWriter Create(Stream stream, Encoding encoding) {
            ALFWriter res = new ALFWriter();
            res.stream = stream;
            res.encoding = encoding;
            return res;
        }

        public static ALFWriter Create(Stream stream)
            => Create(stream, Encoding.UTF8);
    }
}
