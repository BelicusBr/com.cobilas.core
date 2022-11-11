using System;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Management.Alfbt {
#pragma warning disable CS1591
#pragma warning disable CS1573
    /// <summary>Contem todos os textos do idioma alvo.</summary>
    public sealed class TranslationCollection : IDisposable {
        private string lang;
        private string displayName;
        private LanguageBlock[] blocks;

        public string Language => lang;
        public string DisplayName => displayName;
        public int BlocksCount => ArrayManipulation.ArrayLength(blocks);

        public LanguageBlock this[int index] => blocks[index];

        public LanguageBlock this[string blockName] => blocks[IndexOff(blockName)];

        public TranslationCollection(string lang, string displayName) {
            this.lang = lang;
            this.displayName = displayName;
        }

        public TranslationCollection(string lang) : this(lang, lang) { }

        /// <summary>Adiciona um novo bloco de texto.</summary>
        public void AddBlock(string blockName)
            => ArrayManipulation.Add(new LanguageBlock(blockName), ref blocks);

        /// <summary>Adiciona um novo texto.</summary>
        /// <param name="blockName">O nome do bloco de texto alvo.</param>
        public void AddText(string blockName, LanguageText text)
            => blocks[IndexOff(blockName)].Add(text);

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder = builder.AppendFormat("- TranslationCollection\n-- lang:{0}\n-- displayName:{1}\n", lang, displayName);
            for (int I = 0; I < BlocksCount; I++)
                builder = builder.AppendFormat("   {0}\n", blocks[I].ToString().TrimEnd('\n').Replace("\n", "\n   "));
            return builder.ToString();
        }

        public bool Contanis(string block)
            => IndexOff(block) >= 0;

        public int IndexOff(string block) {
            for (int I = 0; I < BlocksCount; I++)
                if (blocks[I].BlockName == block)
                    return I;
            return -1;
        }

        public void Dispose() {
            lang = displayName = (string)null;
            for (int I = 0; I < BlocksCount; I++)
                blocks[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref blocks);
        }
    }
}
