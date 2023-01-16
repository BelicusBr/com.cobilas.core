using System;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Management.Alfbt {
    /// <summary>Representa um bloco de texto.</summary>
    public struct LanguageBlock : IDisposable {
        private string lang_gui;
        private LanguageText[] texts;

        public string BlockName => lang_gui;
        public int LanguageTextCount => ArrayManipulation.ArrayLength(texts);

        public LanguageText this[int index] => texts[index];

        public LanguageText this[string name] => texts[IndexOff(name)];

        public LanguageBlock(string blockName) {
            this.lang_gui = blockName;
            this.texts = (LanguageText[])null;
        }

        /// <summary>Adiciona um texto</summary>
        /// <param name="name">Nome do container de texto</param>
        public void Add(string name, string text)
            => Add(new LanguageText(name, text));

        /// <summary>Adiciona um texto</summary>
        public void Add(LanguageText languageText)
            => ArrayManipulation.Add(languageText, ref texts);

        public int IndexOff(string name) {
            for (int I = 0; I < LanguageTextCount; I++)
                if (texts[I].Name == name)
                    return I;
            return -1;
        }

        public bool Contains(string name)
            => IndexOff(name) >= 0;

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("- {0}\n", lang_gui);
            for (int I = 0; I < LanguageTextCount; I++)
                builder.AppendFormat("   {0}\n", texts[I].Name);
            return builder.ToString();
        }

        public void Dispose() {
            lang_gui = (string)null;
            for (int I = 0; I < LanguageTextCount; I++)
                texts[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref texts);
        }
    }
}
