using System;
using System.IO;
using System.Text;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Alfbt.Components;

namespace Cobilas.IO.Alf.Alfbt {
    public abstract class ALFBTWriter : IDisposable {

        public abstract ALFWriterSettings Settings { get; }

        public abstract void WriterHeaderFlag();
        [Obsolete("Use WriteElement(string, string)")]
        public abstract void WriterMarkingFlag(string name, string value);
        public abstract void WriterCommentFlag(string text);
        [Obsolete("Use WriteElement(string, string)")]
        public abstract void WriterTextFlag(string name, string value);
        public abstract void WriteLineBreak();
        public abstract void WriteLineBreak(int lines);
        public abstract bool Contains(string name);
        public abstract void WriteElement(string name, string text);
        public abstract void Dispose();
        public abstract void Close();
        public abstract void Flush();
        protected abstract string AddEscapeOnSpecialCharactersInText(string value);

        public static ALFBTWriter Create(TextWriter writer, ALFWriterSettings settings) {
            settings.Set(writer, writer.Encoding);
            return new ALFBTMemoryStreamWriter(settings);
        }

        public static ALFBTWriter Create(TextWriter writer)
            => Create(writer, ALFMemoryWriterSetting.DefaultSettings);

        public static ALFBTWriter Create(Stream stream, Encoding encoding, ALFWriterSettings settings) {
            settings.Set(stream, encoding);
            return new ALFBTMemoryStreamWriter(settings);
        }

        public static ALFBTWriter Create(Stream stream, ALFWriterSettings settings) {
            settings.Set(stream, Encoding.UTF8);
            return new ALFBTMemoryStreamWriter(settings);
        }

        public static ALFBTWriter Create(Stream stream, Encoding encoding)
            => Create(stream, encoding, ALFMemoryWriterSetting.DefaultSettings);

        public static ALFBTWriter Create(Stream stream)
            => Create(stream, ALFMemoryWriterSetting.DefaultSettings);

        protected static void WriterALFBTFlag(ALFItem root, StringBuilder builder, bool indent) {
            foreach (ALFItem item in root) {
                switch (item.name) {
                    case ALFBTUtility.n_Comment:
                        builder.AppendFormat("#>{0}<#{1}", item.text, indent ? "\r\n" : string.Empty);
                        break;
                    case ALFBTUtility.n_BreakLine:
                        if (indent)
                            builder.Append(item.text);
                        break;
                    default:
                        builder.AppendFormat("#! {0}:/*{1}*/{2}", item.name, item.text, indent ? "\r\n" : string.Empty);
                        break;
                }
            }
        }
    }
}
