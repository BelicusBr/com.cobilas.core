using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Alfbt.Extension;
using Cobilas.IO.Alf.Alfbt.Components;

namespace Cobilas.IO.Alf.Alfbt.Compiler {
    internal static class Compiler_ALFBT_1_0 {
        public const string n_Version = "version";
        public const string n_Type = "type";
        public const string n_Encoding = "encoding";
        private static int CommentFlagCount;
        private static int LineBreakCount;

        public static string m_CompilerVersion => "1.0";
        public static string m_CompilerType => ".alfbt";

        public static FlagBase[] Decompiler(string[] args) {
            CommentFlagCount = LineBreakCount = 0;
            FlagBase[] res = null;
            bool headerSecond = false;
            for (int I = 0; I < ArrayManipulation.ArrayLength(args); I++) {
                string str_flag = GetFalgType(args[I]);
                switch (str_flag) {
                    case "#*":
                        ArrayManipulation.Add(CreateCommentFlag(GetFlagContent(args[I])), ref res);
                        break;
                    case "#$":
                        if (headerSecond) throw ALFBTFormatException.GetHeaderSecond();
                        ArrayManipulation.Add(Contains(CreateMarkupOrHeaderFlag(GetFlagContent(args[I]), I, AlfbtFlags.HeaderFlag), res), ref res);
                        break;
                    case "#!":
                        headerSecond = true;
                        ArrayManipulation.Add(Contains(CreateMarkupOrHeaderFlag(GetFlagContent(args[I]), I, AlfbtFlags.MarkingFlag), res), ref res);
                        break;
                    case "#?":
                        headerSecond = true;
                        ArrayManipulation.Add(Contains(CreateTextFlag(GetFlagContent(args[I]), args, ref I), res), ref res);
                        break;
                    default:
                        if (!string.IsNullOrEmpty(args[I]))
                            throw ALFBTFormatException.GetInvalidFormattingException(I + 1);
                        else ArrayManipulation.Add(new FlagBase($"LineBreak_{LineBreakCount++}", "\n", AlfbtFlags.LineBreak), ref res);
                        break;
                }
            }
            return res;
        }

        public static string Compiler(FlagBase[] flags) {
            StringBuilder builder = new StringBuilder();
            bool headerSecond = false;
            for (int I = 0; I < ArrayManipulation.ArrayLength(flags); I++) {
                switch (flags[I].Flags) {
                    case AlfbtFlags.MarkingFlag:
                        headerSecond = true;
                        ValueIsValid(flags[I].Name, true);
                        ValueIsValid(flags[I].Value, false);
                        builder.AppendFormat("#! {0}={1}\n", flags[I].Name, flags[I].Value);
                        break;
                    case AlfbtFlags.TextFlag:
                        headerSecond = true;
                        ValueIsValid(flags[I].Name, true);
                        builder.AppendFormat("#? {0}=@(\n", flags[I].Name);
                        if (flags[I].Value.LastIndexOf('\n') == -1)
                            builder.AppendLine(flags[I].Value);
                        else builder.Append(flags[I].Value);
                        builder.AppendLine(")@");
                        break;
                    case AlfbtFlags.HeaderFlag:
                        if (headerSecond) throw ALFBTFormatException.GetHeaderSecond();
                        HeaderFlagIsValid(flags[I]);
                        builder.AppendFormat("#$ {0}={1}\n", flags[I].Name, flags[I].Value);
                        break;
                    case AlfbtFlags.CommentFlag:
                        builder.AppendFormat("#* {0}\n", flags[I].Value);
                        break;
                    case AlfbtFlags.LineBreak:
                        builder.AppendFormat("{0}", flags[I].Value);
                        break;
                }
            }
            return builder.ToString();
        }

        private static void GetFlagNameEndValue(string str, out string name, out string value)
            => SeparateString(str, '=', out name, out value);

        private static string GetFlagContent(string str) {
            string strOut;
            SeparateString(str, ' ', out _, out strOut);
            return strOut;
        }

        private static string GetFalgType(string str) {
            string strOut;
            SeparateString(str, ' ', out strOut, out _);
            return strOut;
        }

        private static void SeparateString(string str, char separator, out string str1, out string str2) {
            int indexSpace = str.IndexOf(separator);
            if (indexSpace == -1) {
                str1 = str;
                str2 = null;
                return;
            }
            str1 = str.Remove(indexSpace);
            str2 = str.Remove(0, indexSpace + 1);
        }

        private static FlagBase Contains(FlagBase flag, FlagBase[] flags) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(flags); I++)
                if (flags[I].Name == flag.Name && flags[I].Flags == flag.Flags)
                    throw ALFBTFormatException.GetRepeatedFlag(flag.Name, flag.Flags);
            return flag;
        }

        private static void HeaderFlagIsValid(FlagBase flag) {
            if (!(flag.Name == n_Version || flag.Name == n_Type || flag.Name == n_Encoding))
                throw ALFBTFormatException.GetHeaderFlagNameInvalid(flag.Name);
            ValueIsValid(flag.Value, false);
        }

        private static void ValueIsValid(string value, bool isName) {
            if (string.IsNullOrEmpty(value)) throw ALFBTFormatException.GetEmptyValue(isName);
            else if (value.Contains(" ")) throw ALFBTFormatException.GetWhiteSpaceException(value);
        }

        private static FlagBase CreateTextFlag(string str_arg, string[] args, ref int index) {
            int startLine = index;

            StringBuilder builder = new StringBuilder();
            string name;
            string value;
            GetFlagNameEndValue(str_arg, out name, out value);

            char charError;
            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(startLine + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(startLine + 1, true);
            if (!name.AlfbtValidName(out charError))
                throw ALFBTFormatException.GetInvalidChar(name, charError, startLine + 1);
            if (value.TrimEnd() != "@(")
                throw ALFBTFormatException.GetException(
                    $"(Line: {startLine + 1})Text block opening symbol \"@(\" not found!",
                    $"(Linha: {startLine + 1})O símbolo de abertura de bloco de texto \"@(\" não foi encontrado!"
                    );

            for (int I = index + 1; I < ArrayManipulation.ArrayLength(args); I++) {
                if (args[I].TrimEnd() == ")@") {
                    index = I;
                    return new FlagBase(name, builder.ToString(), AlfbtFlags.TextFlag);
                }
                builder.AppendLine(args[I]);
            }

            throw ALFBTFormatException.GetException(
                $"(Line: {startLine + 1})Text block closing symbol \")@\" not found!",
                $"(Linha: {startLine + 1})Símbolo de fechamento de bloco de texto \")@\" não encontrado!"
                );
        }

        private static FlagBase CreateMarkupOrHeaderFlag(string str_arg, int index, AlfbtFlags flag) {
            string name;
            string value;
            GetFlagNameEndValue(str_arg, out name, out value);

            char charError;
            if (string.IsNullOrEmpty(name))
                throw ALFBTFormatException.GetFlagNameException(index + 1);
            if (name.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(index + 1, true);
            switch (flag) {
                case AlfbtFlags.HeaderFlag:
                    if (!(name == n_Version || name == n_Type || name == n_Encoding))
                        throw ALFBTFormatException.GetHeaderFlagNameInvalid(name);
                    break;
                default:
                    if (!name.AlfbtValidName(out charError))
                        throw ALFBTFormatException.GetInvalidChar(name, charError, index + 1);
                    break;
            }

            if (string.IsNullOrEmpty(value))
                throw ALFBTFormatException.GetFlagValueException(index + 1);
            if (value.AlfbtIsWhiteSpace()) throw ALFBTFormatException.GetWhiteSpaceException(index + 1, true);
            if (!value.AlfbtValidText(out charError))
                throw ALFBTFormatException.GetInvalidChar(value, charError, index + 1);

            return new FlagBase(name, value, flag);
        }

        private static FlagBase CreateCommentFlag(string str_arg)
            => new FlagBase($"Comment_{CommentFlagCount++}", str_arg, AlfbtFlags.CommentFlag);
    }
}
