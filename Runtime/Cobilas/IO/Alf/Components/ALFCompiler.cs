using System;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Components {
    internal static class ALFCompiler {
        internal static readonly string version = "1.0";

        internal static void Read(ALFItem root, string[] lines) {
            bool OpenTextElement = false;
            for (int I = 0; I < ArrayManipulation.ArrayLength(lines); I++) {
                switch (GetALFNodeType(lines[I], I + 1, out Exception exception)) {
                    case ALFNodeType.OpenElement:
                        root.Add(root = new ALFItem());
                        root.name = GetElementName(lines[I]);
                        break;
                    case ALFNodeType.ClosedElement:
                        root = root.parent;
                        break;
                    case ALFNodeType.OpenTextElement:
                        OpenTextElement = true;
                        goto case ALFNodeType.OpenElement;
                    case ALFNodeType.ClosedTextElement:
                        OpenTextElement = false;
                        break;
                    case ALFNodeType.TextElement:
                        if (OpenTextElement) {
                            root.text.Append(GetElementText(lines[I], false));
                            root.text.Append("\r\n");
                        }
                        else throw ALFERROR.TextElementERROR(I + 1);
                        break;
                    case ALFNodeType.SingleLineElement:
                        root.Add(root = new ALFItem());
                        root.name = GetElementName(lines[I]);
                        root.text.Append(GetElementText(lines[I]));
                        root = root.parent;
                        break;
                    case ALFNodeType.SingleLineComment:
                        root.Add(root = new ALFItem());
                        root.name = "comment";
                        root.text.Append(lines[I].Remove(0, lines[I].IndexOf(' ') + 1));
                        root = root.parent;
                        break;
                    case ALFNodeType.UnknownElement:
                        if (exception == (Exception)null)
                            throw ALFERROR.UnidentifiedElement(I + 1, lines[I]);
                        throw exception;
                }
            }
        }

        internal static void Writer(ALFItem root, int tab, StringBuilder builder) {
            string txt = root.text.ToString();

            if (IsMultLine(txt) || !ArrayManipulation.EmpytArray(root.itens)) {
                builder.AppendFormat("{0}[{1}", GetTab(tab), root.name);
                if (!string.IsNullOrEmpty(txt)) {
                    builder.Append(":\r\n");
                    txt = txt.Replace("\n", string.Format("\n{0}:", GetTab(tab + 1)));

                    builder.AppendFormat("{0}:{1}\r\n", GetTab(tab + 1), txt);
                    builder.AppendFormat("{0}:>\r\n", GetTab(tab + 1));
                } else builder.Append(":>\r\n");

                for (int I = 0; I < ArrayManipulation.ArrayLength(root.itens); I++)
                    Writer(root.itens[I], tab + 1, builder);

                builder.AppendFormat("{0}<]\r\n", GetTab(tab + 1));
            } else {
                switch (root.name) {
                    case "breakline":
                        builder.AppendFormat("{0}{1}", GetTab(tab), txt);
                        break;
                    case "comment":
                        builder.AppendFormat("{0}[* {1}\r\n", GetTab(tab), txt);
                        break;
                    default:
                        builder.AppendFormat("{0}[{1}:{2}]\r\n", GetTab(tab), root.name, txt);
                        break;
                }
            }
        }

        private static bool IsMultLine(string txt)
            => !string.IsNullOrEmpty(txt) && txt.Contains("\n");

        private static string GetTab(int tabs)
            => string.Empty.PadRight(tabs, '\t');

        private static string GetElementName(string line) {
            line = line.Trim().Trim('[', ']');
            return line.Remove(line.IndexOf(':'));
        }

        private static string GetElementText(string line, bool isSingleLineElement) {
            if (isSingleLineElement)
                line = line.TrimStart().Trim('[', ']');
            return line.Remove(0, line.IndexOf(':') + 1);
        }

        private static string GetElementText(string line)
            => GetElementText(line, true);

        private static ALFNodeType GetALFNodeType(string line, int iline, out Exception exception) {
            string txt404;
            exception = (Exception)null;
            if (string.IsNullOrEmpty(line)) return ALFNodeType.EmptyElement;
            line = line.Trim();

            if (line.IndexOf("[*") >= 0) {
                if (!string.IsNullOrEmpty(line.Remove(line.IndexOf('[')))) {
                    exception = ALFERROR.CommentERROR(iline);
                    return ALFNodeType.UnknownElement;
                }

                return ALFNodeType.SingleLineComment;
            }

            if (line.IndexOf('[') >= 0 && line.LastIndexOf(']') >= 0) {
                if (!string.IsNullOrEmpty(txt404 = line.Remove(line.IndexOf('[')))) {
                    exception = ALFERROR.UnidentifiedElement(iline, txt404);
                    return ALFNodeType.UnknownElement;
                }

                if (!string.IsNullOrEmpty(txt404 = line.Remove(0, line.LastIndexOf(']') + 1))) {
                    exception = ALFERROR.UnidentifiedElement(iline, txt404);
                    return ALFNodeType.UnknownElement;
                }

                if (line.IndexOf(':') < 0) {
                    exception = ALFERROR.UnidentifiedElement(iline, line);
                    return ALFNodeType.UnknownElement;
                }
                return ALFNodeType.SingleLineElement;
            }

            if (line.IndexOf('[') >= 0 && line.LastIndexOf(":>") >= 0) {

                if (!string.IsNullOrEmpty(txt404 = line.Remove(line.IndexOf('[')))) {
                    exception = ALFERROR.UnidentifiedElement(iline, txt404);
                    return ALFNodeType.UnknownElement;
                }

                if ((txt404 = line.Remove(0, line.IndexOf(':'))) != ":>") {
                    exception = ALFERROR.UnidentifiedElement(iline, txt404);
                    return ALFNodeType.UnknownElement;
                }

                return ALFNodeType.OpenElement;
            }

            if (line.IndexOf('[') >= 0 && line.LastIndexOf(':') >= 0) {

                if (!string.IsNullOrEmpty(txt404 = line.Remove(line.IndexOf('[')))) {
                    exception = ALFERROR.UnidentifiedElement(iline, txt404);
                    return ALFNodeType.UnknownElement;
                }

                if ((txt404 = line.Remove(0, line.IndexOf(':'))) != ":") {
                    exception = ALFERROR.UnidentifiedElement(iline, txt404);
                    return ALFNodeType.UnknownElement;
                }

                return ALFNodeType.OpenTextElement;
            }

            if (line.IndexOf(":>") >= 0) {
                if (line != ":>") {
                    exception = ALFERROR.UnidentifiedElement(iline, line);
                    return ALFNodeType.UnknownElement;
                }
                return ALFNodeType.ClosedTextElement;
            }

            if (line.IndexOf(':') >= 0) {
                if (!string.IsNullOrEmpty(txt404 = line.Remove(line.IndexOf(':')))) {
                    exception = ALFERROR.PrintLineError("({0})text delimiter must not have prefix", iline, txt404);
                    return ALFNodeType.UnknownElement;
                }
                return ALFNodeType.TextElement;
            }

            if (line.IndexOf("<]") >= 0) {
                if (line != "<]") {
                    exception = ALFERROR.UnidentifiedElement(iline, line);
                    return ALFNodeType.UnknownElement;
                }
                return ALFNodeType.ClosedElement;
            }

            return ALFNodeType.UnknownElement;
        }
    }
}
