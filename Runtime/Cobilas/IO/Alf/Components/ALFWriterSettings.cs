using System.Text;

namespace Cobilas.IO.Alf.Components {
    public abstract class ALFWriterSettings : ALFSettings {
        public abstract bool Indent { get; set; }
        public abstract bool AddEscapeOnSpecialCharacters { get; set; }

        public abstract void Writer(string text);
        public abstract void Writer(char[] buffer);
        public abstract void Writer(StringBuilder builder);
    }
}
