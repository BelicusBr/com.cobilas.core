using System;
using System.IO;
using System.Text;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf.Alfbt {
    public abstract class ALFBTWriter : ALFWriter {
        public const string alfbtVersion = "1.5";
        [Obsolete("Use StartElementHeader()")]
        public abstract void WriterHeaderFlag();
        [Obsolete("Use WriteElement(string, string)")]
        public abstract void WriterMarkingFlag(string name, string value);
        [Obsolete("Use StartElementComment(string)")]
        public abstract void WriterCommentFlag(string text);
        [Obsolete("Use WriteElement(string, string)")]
        public abstract void WriterTextFlag(string name, string value);
        [Obsolete("Use StartElementBreakLine(int) or StartElementBreakLine()")]
        public abstract void WriteLineBreak();
        public abstract bool Contains(string name);

        public static new ALFBTWriter Create(TextWriter writer, ALFWriterSettings settings) {
            settings.Set(writer, writer.Encoding);
            return new ALFBTMemoryStreamWriter(settings);
        }

        public static new ALFBTWriter Create(TextWriter writer)
            => Create(writer, ALFMemoryWriterSetting.DefaultSettings);

        public static new ALFBTWriter Create(Stream stream, Encoding encoding, ALFWriterSettings settings) {
            settings.Set(stream, encoding);
            return new ALFBTMemoryStreamWriter(settings);
        }

        public static new ALFBTWriter Create(Stream stream, ALFWriterSettings settings) {
            settings.Set(stream, Encoding.UTF8);
            return new ALFBTMemoryStreamWriter(settings);
        }

        public static new ALFBTWriter Create(Stream stream, Encoding encoding)
            => Create(stream, encoding, ALFMemoryWriterSetting.DefaultSettings);

        public static new ALFBTWriter Create(Stream stream)
            => Create(stream, ALFMemoryWriterSetting.DefaultSettings);

        protected static void WriterALFBTFlag(ALFItem root, StringBuilder builder, bool indent) {
            foreach (ALFItem item in root) {
                switch (item.name) {
                    case n_Comment:
                        builder.AppendFormat("#>{0}<#{1}", item.text, indent ? "\r\n" : string.Empty);
                        break;
                    case n_BreakLine:
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
