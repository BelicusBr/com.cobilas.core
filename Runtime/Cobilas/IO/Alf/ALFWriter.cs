using System;
using System.IO;
using System.Linq;
using System.Text;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf {
    public abstract class ALFWriter : IDisposable {
        public abstract ALFWriterSettings Settings { get; }

        public abstract void StartElement(string name);
        public abstract void EndElement();
        public abstract void WriteElement(string name, string text);
        public abstract void WriteText(bool value)    ;
        public abstract void WriteText(string value)  ;
        public abstract void WriteText(char value)    ;
        public abstract void WriteText(char[] value)  ;
        public abstract void WriteText(float value)   ;
        public abstract void WriteText(double value)  ;
        public abstract void WriteText(decimal value) ;
        public abstract void WriteText(sbyte value)   ;
        public abstract void WriteText(short value)   ;
        public abstract void WriteText(int value)     ;
        public abstract void WriteText(long value)    ;
        public abstract void WriteText(byte value)    ;
        public abstract void WriteText(ushort value)  ;
        public abstract void WriteText(uint value)    ;
        public abstract void WriteText(ulong value)   ;
        public abstract void WriteText(DateTime value);
        public abstract void StartElementComment(string text);
        public abstract void StartElementBreakLine(int breaks);
        public abstract void StartElementBreakLine();
        public abstract void StartElementHeader();
        public abstract void Close();
        public abstract void Flush();
        public abstract void Dispose();
        protected abstract void InternalWriteText(object value);
        protected abstract string AddEscapeOnSpecialCharactersInText(string value);

        public static ALFWriter Create(TextWriter writer, ALFWriterSettings settings) {
            settings.Set(writer, writer.Encoding);
            return new ALFMemoryStreamWriter(settings);
        }

        public static ALFWriter Create(TextWriter writer)
            => Create(writer, ALFMemoryWriterSetting.DefaultSettings);

        public static ALFWriter Create(Stream stream, Encoding encoding, ALFWriterSettings settings) {
            settings.Set(stream, encoding);
            return new ALFMemoryStreamWriter(settings);
        }

        public static ALFWriter Create(Stream stream, ALFWriterSettings settings) {
            settings.Set(stream, Encoding.UTF8);
            return new ALFMemoryStreamWriter(settings);
        }

        public static ALFWriter Create(Stream stream, Encoding encoding)
            => Create(stream, encoding, ALFMemoryWriterSetting.DefaultSettings);

        public static ALFWriter Create(Stream stream)
            => Create(stream, ALFMemoryWriterSetting.DefaultSettings);

        protected static void WriteFlag(ALFItem root, int depth, StringBuilder builder, bool indent) {
            foreach (ALFItem item in root)
                switch (item.name) {
                    case ALFUtility.n_Comment:
                        builder.AppendFormat("{0}[*{1}", ALFUtility.GetTabs(depth, indent), item.text.ToString());
                        builder.AppendFormat("{0}*]{1}", ALFUtility.GetTabs(depth, indent), indent ? "\r\n" : string.Empty);
                        break;
                    case ALFUtility.n_BreakLine:
                        if (!indent) break;
                        builder.Append(item.text.ToString());
                        break;
                    default:
                        builder.AppendFormat("{0}[{1}", ALFUtility.GetTabs(depth, indent), item.name);
                        WriteFlagText(item, depth + 1, builder, indent);
                        if (item.Count != 0) {
                            builder.AppendFormat(":>{0}", indent ? "\r\n" : string.Empty);
                            WriteFlag(item, depth + 1, builder, indent);
                            builder.AppendFormat("{0}<]{1}", ALFUtility.GetTabs(depth, indent), indent ? "\r\n" : string.Empty);
                        } else {
                            if (item.text.ToString().Contains('\n')) {
                                builder.AppendFormat(":>", indent ? "\r\n" : string.Empty);
                                builder.AppendFormat("{0}<]{1}", ALFUtility.GetTabs(depth, indent), indent ? "\r\n" : string.Empty);
                            }
                            else builder.AppendFormat("]{0}", indent ? "\r\n" : string.Empty);
                        }
                        break;
                }
        }

        internal static bool ThisTextIsValid(out char error, params char[] text) {
            error = char.MinValue;
            using (CharacterCursor cursor = new CharacterCursor(text))
                while (cursor.MoveToCharacter()) {
                    if (cursor.CharIsEqualToIndex(ALFUtility.EscapesString))
                        cursor.MoveToCharacter(1L);
                    else if (cursor.CharIsEqualToIndex(ALFUtility.InvalidTextCharacters)) {
                        error = cursor.CurrentCharacter;
                        return false;
                    }
                }
            return true;
        }

        internal static bool ThisTextIsValid(out char error, string text)
            => ThisTextIsValid(out error, text.ToCharArray());

        private static void WriteFlagText(ALFItem root, int depth, StringBuilder builder, bool indent) {
            string txt = root.text.ToString();
            if (!string.IsNullOrEmpty(txt)) {
                txt = txt.Replace("\n", string.Format("\n{0}:", ALFUtility.GetTabs(depth, indent)));
                builder.AppendFormat(":{0}", txt);
            }
        }
    }
}
