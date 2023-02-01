using System;

namespace Cobilas.IO.Alf.Management.Alfbt {
    public struct LanguageInfo : IEquatable<LanguageInfo>, ICloneable {
        private readonly string lang;
        private readonly string display_lang;

        public string Language => lang;
        public string Display_Name => display_lang;

        public LanguageInfo(string lang, string display_lang) {
            this.lang = lang;
            this.display_lang = display_lang;
        }

        public override int GetHashCode()
            => GetHashCodeSafe(lang) >> GetHashCodeSafe(display_lang) ^ base.GetHashCode();

        public override bool Equals(object obj)
            => obj is LanguageInfo Res && Equals(Res);

        public bool Equals(LanguageInfo other)
            => other.lang == lang && other.display_lang == display_lang;

        public override string ToString()
            => $"Language:{lang}\nDisplay name:{display_lang}\n";

        public object Clone()
            => new LanguageInfo(
                (string)CloneSafe(lang),
                (string)CloneSafe(display_lang)
                );

        private int GetHashCodeSafe(object obj)
            => obj == null ? 0 : obj.GetHashCode();

        private object CloneSafe(ICloneable cloneable)
            => cloneable == null ? (object)null : cloneable.Clone();

        public static bool operator ==(LanguageInfo A, LanguageInfo B)
            => A.Equals(B);

        public static bool operator !=(LanguageInfo A, LanguageInfo B)
            => !(A == B);
    }
}
