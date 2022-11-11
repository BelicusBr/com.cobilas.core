using System;
using System.Linq;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Alfbt;
using Cobilas.IO.Alf.Alfbt.Flags;

//Language block >> Bloco de idioma
//Language text  >> Texto de idioma
namespace Cobilas.IO.Alf.Management.Alfbt {
#pragma warning disable CS1591
    public sealed class TranslationManagement : IDisposable {
        /// <summary>O separador atual por padrão é '.'</summary>
        public char CurrentSeparator;
        private TranslationCollection[] tlt_list;

        public const char DefaultSeparator = '.';
        public const string GenericGUITarget = "ggt";
        public const string MarkingFlagLanguage = "language";
        public const string MarkingFlagGUITarget = "gui_target";
        public const string MarkingFlagDisplayName = "display_name";
        public int LanguageCount => ArrayManipulation.ArrayLength(tlt_list);

        public TranslationManagement() {
            CurrentSeparator = DefaultSeparator;
        }

        /// <summary>Obtem uma lista de idiomas registrado no <see cref="TranslationManagement"/>.</summary>
        public LanguageInfo[] GetListOfLanguages() {
            LanguageInfo[] res = new LanguageInfo[LanguageCount];
            for (int I = 0; I < LanguageCount; I++)
                res[I] = new LanguageInfo(tlt_list[I].Language, tlt_list[I].DisplayName);
            return res;
        }

        public TranslationCollection GetTranslation(string language) => (TranslationCollection)null;

        public TextFlag GetTextFlag(string path) => new TextFlag();

        public MarkingFlag GetMarkingFlag(string path) => new MarkingFlag();

        /// <summary>Obtem um texto de idioma.</summary>
        /// <param name="path">Caminho alvo. ({language}.{block}.{name})</param>
        public LanguageText GetLanguageText(string path) {
            string lang = path.Remove(path.IndexOf('.'));
            string block = path.Remove(0, path.IndexOf('.') + 1);
            block = block.Remove(block.LastIndexOf('.'));
            string name = path.Remove(0, path.LastIndexOf('.') + 1);
            return tlt_list[I_IndexOff(lang)][block][name];
        }

        /// <summary>Carrega a tradução apartir de um arquivo .alfbt</summary>
        public bool LoadTranslation(ALFBTRead read)
            => LoadTranslation(read, out _);

        /// <summary>Carrega a tradução apartir de um arquivo .alfbt</summary>
        public bool LoadTranslation(ALFBTRead read, out Exception exception) {
            exception = (Exception)null;
            try {
                if (!read.MarkingFlagExists(MarkingFlagLanguage)) return false;
                string lang = read.GetMarkingFlag(MarkingFlagLanguage).Value;
                string displayName = read.MarkingFlagExists(MarkingFlagDisplayName) ? read.GetMarkingFlag(MarkingFlagDisplayName).Value : lang;
                string gui_target = read.MarkingFlagExists(MarkingFlagGUITarget) ? read.GetMarkingFlag(MarkingFlagGUITarget).Value : (string)null;

                int indexLang = I_IndexOff(lang);
                TranslationCollection temp = (TranslationCollection)null;
                if (indexLang < 0) 
                    ArrayManipulation.Add(temp = new TranslationCollection(lang, displayName), ref tlt_list);

                object[] flags = new object[0];

                ArrayManipulation.Add(FlagListToObjectArray(read.GetAllMarkingFlags()), ref flags);
                ArrayManipulation.Add(FlagListToObjectArray(read.GetAllTextFlags()), ref flags);

                if (!string.IsNullOrEmpty(gui_target)) {
                    if (!temp.Contanis(gui_target))
                        temp.AddBlock(gui_target);

                    foreach (object item in flags) {
                        if (item is MarkingFlag mkf)
                            if (mkf.Name != MarkingFlagLanguage &&
                                mkf.Name != MarkingFlagDisplayName && mkf.Name != MarkingFlagGUITarget) {
                                MarkingFlag m_temp = (MarkingFlag)mkf.Clone();
                                temp.AddText(gui_target, new LanguageText(m_temp.Name, m_temp.Value));
                            }
                        if (item is TextFlag ttf) {
                            TextFlag t_temp = (TextFlag)ttf.Clone();
                            temp.AddText(gui_target, new LanguageText(t_temp.Name, t_temp.Value));
                        }
                    }
                } else {
                    foreach (object item in flags) {
                        string nameTemp = (string)null;
                        if (item is MarkingFlag mkf)
                            if (mkf.Name != MarkingFlagLanguage && 
                                mkf.Name != MarkingFlagDisplayName && mkf.Name != MarkingFlagGUITarget) {
                                MarkingFlag m_temp = (MarkingFlag)mkf.Clone();
                                if (m_temp.Name.Contains(CurrentSeparator)) {
                                    gui_target = m_temp.Name.Remove(m_temp.Name.LastIndexOf('.'));
                                    nameTemp = m_temp.Name.Remove(0, m_temp.Name.LastIndexOf('.') + 1);
                                } else {
                                    nameTemp = m_temp.Name;
                                    gui_target = GenericGUITarget;
                                }

                                if (!temp.Contanis(gui_target))
                                    temp.AddBlock(gui_target);

                                temp.AddText(gui_target, new LanguageText(nameTemp, m_temp.Value));
                            }
                        if (item is TextFlag ttf) {
                            TextFlag t_temp = (TextFlag)ttf.Clone();
                            if (t_temp.Name.Contains(CurrentSeparator)) {
                                gui_target = t_temp.Name.Remove(t_temp.Name.LastIndexOf('.'));
                                nameTemp = t_temp.Name.Remove(0, t_temp.Name.LastIndexOf('.') + 1);
                            } else {
                                nameTemp = t_temp.Name;
                                gui_target = GenericGUITarget;
                            }

                            if (!temp.Contanis(gui_target))
                                temp.AddBlock(gui_target);

                            temp.AddText(gui_target, new LanguageText(nameTemp, t_temp.Value));
                        }
                    }
                }
            } catch (Exception e) {
                exception = e;
                return false;
            }
            return true;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("- TranslationDataManager\n");
            for (int I = 0; I < LanguageCount; I++)
                builder.AppendFormat("   {0}\n", tlt_list[I].ToString().Replace("\n", "\n   "));
            return builder.ToString();
        }

        public void Dispose() {
            for (int I = 0; I < LanguageCount; I++)
                tlt_list[I].Dispose();

            ArrayManipulation.ClearArraySafe(ref tlt_list);
        }

        private object[] FlagListToObjectArray(TextFlag[] flags) {
            object[] res = new object[ArrayManipulation.ArrayLength(flags)];
            for (int I = 0; I < res.Length; I++)
                res[I] = flags[I];
            return res;
        }

        private object[] FlagListToObjectArray(MarkingFlag[] flags) {
            object[] res = new object[ArrayManipulation.ArrayLength(flags)];
            for (int I = 0; I < res.Length; I++)
                res[I] = flags[I];
            return res;
        }

        private int I_IndexOff(string lang) {
            for (int I = 0; I < LanguageCount; I++)
                if (tlt_list[I].Language == lang)
                    return I;
            return -1;
        }
    }
}
