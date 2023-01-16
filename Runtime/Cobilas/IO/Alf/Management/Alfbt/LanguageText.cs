using System;
using System.Text;
using System.Globalization;

namespace Cobilas.IO.Alf.Management.Alfbt {
    /// <summary>Representa um container de texto.</summary>
    public struct LanguageText : IEquatable<LanguageText>, IDisposable {
        private string name;
        private string text;

        public string Name => name;
        public string Text => text;

        public LanguageText(string name, string text) {
            this.name = name;
            this.text = text;
        }

        /// <summary>Obtenha o texto já formatado.</summary>
        public string Format(IFormatProvider formatProvider, object[] args)
            => string.Format(text, args, formatProvider);

        /// <summary>Obtenha o texto já formatado.</summary>
        public string Format(object[] args)
            => Format(CultureInfo.InvariantCulture, args);

        public override string ToString()
            => new StringBuilder().AppendFormat("- {0}\n", name)
                .AppendFormat("\t{0}", text.Replace("\n", "\n\t")).ToString();

        public override bool Equals(object obj)
            => obj is LanguageText ltt && Equals(ltt);

        public override int GetHashCode()
            => base.GetHashCode() >>
                (string.IsNullOrEmpty(name) ? 0 : name.GetHashCode()) ^
                (string.IsNullOrEmpty(text) ? 0 : text.GetHashCode());

        public bool Equals(LanguageText other)
            => other.name == name && other.text == text;

        public void Dispose()
            => name = text = (string)null;

        public static bool operator ==(LanguageText A, LanguageText B)
            => A.Equals(B);

        public static bool operator !=(LanguageText A, LanguageText B)
            => !(A == B);
    }
}
