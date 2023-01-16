using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using Cobilas.IO.Alf.Alfbt.Extension;

namespace Cobilas.IO.Alf.Alfbt.Components.Compiler {
    internal static class ALFBTCompiler_1_5 {

        internal static void Read(string[] lines, ALFWriter writer) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(lines); I++) {
                string tagtype = ALFBTCompiler_1_0.GetFalgType(lines[I]);
                switch (tagtype) {
                    case "#!":
                        if (IsSingleLine(lines[I])) {
                            GetSingleTag(ALFBTCompiler_1_0.GetFlagContent(lines[I]), I, writer);
                            break;
                        }
                        I = GetMultLineTag(lines, I, writer);
                        break;
                    case "#>":
                        writer.StartElementComment(ALFBTCompiler_1_0.GetFlagContent(lines[I]));
                        break;
                    default:
                        if (!string.IsNullOrEmpty(lines[I]))
                            throw ALFBTFormatException.GetInvalidFormattingException(I + 1, lines[I]);
                        break;
                }
            }
        }

        internal static void Writer(ALFItem root, StringBuilder builder) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(root.itens); I++) {
                ALFItem temp = root.itens[I];
                string txt = temp.text.ToString();

                switch (temp.name) {
                    case ALFBTCompiler_1_0.n_Comment:
                        builder.AppendFormat("#> {0}\r\n", txt);
                        break;
                    case ALFBTCompiler_1_0.n_BreakLine:
                        builder.Append("\r\n");
                        break;
                    default:
                        builder.AppendFormat("#! {0}:/*", temp.name);
                        if (txt.Contains("\n")) {
                            builder.Append("\r\n");
                            builder.Append(txt);
                            builder.Append("\r\n*/\r\n");
                        } else {
                            builder.AppendFormat("{0}*/\r\n", txt);
                        }
                        break;
                }
            }
        }

        internal static bool DetectAlfbtVersion(string line, out ALFBTVersion version) {
            if (line.IndexOf("#>") >= 0 ||
                (line.IndexOf("#!") >= 0 && line.IndexOf(":/*") >= 0)) {
                version = ALFBTVersion.alfbt_1_5;
                return true;
            }
            version = ALFBTVersion.UnknownVersion;
            return false;
        }

        private static void GetSingleTag(string line, int indexline, ALFWriter writer) {
            string name;
            string value;
            string value2;
            GetFlagNameEndValue(line, out name, out value);

            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(indexline + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);

            if (value.IndexOf("/*") < 0)
                throw ALFBTFormatException.GetFlagValueException(indexline + 1);

            if (value.LastIndexOf("*/") < 0)
                throw ALFBTFormatException.GetFlagValueException(indexline + 1);

            value2 = value.Remove(value.IndexOf("/*"));
            if (!string.IsNullOrEmpty(value2))
                throw ALFBTFormatException.GetFlagValueException(indexline + 1);

            value2 = value.Remove(0, value.LastIndexOf("*/") + 2);
            if (!string.IsNullOrEmpty(value2))
                throw ALFBTFormatException.GetFlagValueException(indexline + 1);

            writer.StartElement(name);
            writer.WriteText(value.Trim('/').Trim('*').Trim());
            writer.EndElement();
        }

        private static int GetMultLineTag(string[] lines, int indexline, ALFWriter writer) {
            string line = ALFBTCompiler_1_0.GetFlagContent(lines[indexline]);
            string name;
            string value;
            GetFlagNameEndValue(line, out name, out value);

            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(indexline + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(indexline + 1, true);

            writer.StartElement(name);

            if (value.Trim() != "/*") throw ALFBTFormatException.GetFlagValueException(indexline + 1);
            for (int I = indexline + 1; I < ArrayManipulation.ArrayLength(lines); I++) {
                line = lines[I];
                if (line.Trim() == "*/") {
                    writer.EndElement();
                    return I;
                } else if (I != indexline + 1)
                    writer.WriteText("\r\n");
                writer.WriteText(line);
            }

            throw ALFBTFormatException.GetException(
                $"(Line: {indexline + 1})Text block closing symbol \"*/\" not found!",
                $"(Linha: {indexline + 1})Símbolo de fechamento de bloco de texto \"*/\" não encontrado!"
                );
        }

        private static bool IsSingleLine(string line)
            => line.IndexOf(":/*") >= 0 && line.LastIndexOf("*/") >= 0;

        private static void GetFlagNameEndValue(string stg, out string name, out string value)
            => ALFBTCompiler_1_0.SeparateString(stg, ':', out name, out value);
    }
}