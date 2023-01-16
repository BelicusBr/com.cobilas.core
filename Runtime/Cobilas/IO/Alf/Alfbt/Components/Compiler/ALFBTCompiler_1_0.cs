using Cobilas.Collections;
using Cobilas.IO.Alf.Alfbt.Extension;

namespace Cobilas.IO.Alf.Alfbt.Components.Compiler {
    internal static class ALFBTCompiler_1_0 {

        public const string n_Type = "type";
        public const string n_Version = "version";
        public const string n_Comment = "comment";
        public const string n_Encoding = "encoding";
        public const string n_BreakLine = "breakline";

        internal static void Read(string[] inputs, ALFWriter writer) {
            bool headerSecond = false;
            for (int I = 0; I < ArrayManipulation.ArrayLength(inputs); I++) {
                string tagType = GetFalgType(inputs[I]);
                switch (tagType) {
                    case "#$":
                        if (headerSecond) throw ALFBTFormatException.GetHeaderSecond();
                        GetHeader(GetFlagContent(inputs[I]), I, writer);
                        break;
                    case "#!":
                        headerSecond = true;
                        GetTag(GetFlagContent(inputs[I]), I, writer);
                        break;
                    case "#?":
                        headerSecond = true;
                        I = GetTagText(inputs, I, writer);
                        break;
                    case "#*":
                        writer.StartElementComment(GetFlagContent(inputs[I]));
                        break;
                    default:
                        if (!string.IsNullOrEmpty(inputs[I]))
                            throw ALFBTFormatException.GetInvalidFormattingException(I + 1, inputs[I]);
                        break;
                }
            }
        }

        internal static bool DetectAlfbtVersion(string line, out ALFBTVersion version) {
            if (line.IndexOf("#*") >= 0 ||
               (line.IndexOf("#!") >= 0 && line.IndexOf("=") >= 0) ||
               (line.IndexOf("#$") >= 0 && line.IndexOf("=") >= 0) ||
               (line.IndexOf("#?") >= 0 && line.IndexOf("=@(") >= 0)) {
                version = ALFBTVersion.alfbt_1_0;
                return true;
            }
            version = ALFBTVersion.UnknownVersion;
            return false;
        }

        internal static string GetFlagContent(string str) {
            string strOut;
            SeparateString(str, ' ', out _, out strOut);
            return strOut;
        }

        internal static string GetFalgType(string str) {
            string strOut;
            SeparateString(str, ' ', out strOut, out _);
            return strOut;
        }

        internal static void GetFlagNameEndValue(string str, out string name, out string value)
            => SeparateString(str, '=', out name, out value);

        internal static void SeparateString(string str, char separator, out string str1, out string str2) {
            int indexSpace = str.IndexOf(separator);
            if (indexSpace == -1) {
                str1 = str;
                str2 = null;
                return;
            }
            str1 = str.Remove(indexSpace);
            str2 = str.Remove(0, indexSpace + 1);
        }

        private static void GetHeader(string line, int indexline, ALFWriter writer) {
            string name;
            string value;

            GetFlagNameEndValue(line, out name, out value);
            char charError;

            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(indexline + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);

            if (!(name == n_Version || name == n_Type || name == n_Encoding))
                throw ALFBTFormatException.GetHeaderFlagNameInvalid(name);

            if (string.IsNullOrEmpty(value))
                throw ALFBTFormatException.GetFlagValueException(indexline + 1);
            if (value.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);
            if (!value.AlfbtValidText(out charError))
                throw ALFBTFormatException.GetInvalidChar(value, charError, indexline + 1);

            writer.StartElement(name);
            writer.WriteText(value);
            writer.EndElement();
        }

        private static void GetTag(string line, int indexline, ALFWriter writer) {
            string name;
            string value;

            GetFlagNameEndValue(line, out name, out value);
            char charError;

            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(indexline + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);

            if (!name.AlfbtValidName(out charError))
                throw ALFBTFormatException.GetInvalidChar(name, charError, indexline + 1);

            if (string.IsNullOrEmpty(value))
                throw ALFBTFormatException.GetFlagValueException(indexline + 1);
            if (value.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);
            if (!value.AlfbtValidText(out charError))
                throw ALFBTFormatException.GetInvalidChar(value, charError, indexline + 1);

            writer.StartElement(name);
            writer.WriteText(value);
            writer.EndElement();
        }

        private static int GetTagText(string[] lines, int indexline, ALFWriter writer) {
            string name;
            string value;

            GetFlagNameEndValue(lines[indexline], out name, out value);
            name = GetFlagContent(name);

            char charError;
            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(indexline + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);
            if (!name.AlfbtValidName(out charError))
                throw ALFBTFormatException.GetInvalidChar(name, charError, indexline + 1);

            if (value.Trim() != "@(")
                throw ALFBTFormatException.GetException(
                    $"(Line: {indexline + 1})Text block opening symbol \"@(\" not found!",
                    $"(Linha: {indexline + 1})O símbolo de abertura de bloco de texto \"@(\" não foi encontrado!"
                    );

            writer.StartElement(name);

            for (int I = indexline + 1; I < ArrayManipulation.ArrayLength(lines); I++) {
                if (lines[I].Trim() == ")@") {
                    writer.EndElement();
                    return I;
                }
                writer.WriteText(lines[I]);
            }

            throw ALFBTFormatException.GetException(
                $"(Line: {indexline + 1})Text block closing symbol \")@\" not found!",
                $"(Linha: {indexline + 1})Símbolo de fechamento de bloco de texto \")@\" não encontrado!"
                );
        }
    }
}
