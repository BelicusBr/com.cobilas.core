namespace System {
    public static class Char_CB_Extension {
        public static sbyte ToSByte(this char c)
            => Convert.ToSByte(c);

        public static EscapeSequence InterpretEscapeSequence(this char c) {
            switch (c) {
                case '\'': return EscapeSequence.SingleQuote;
                case '\"': return EscapeSequence.DoubleQuote;
                case '\\': return EscapeSequence.BackSlash;
                case '\0': return EscapeSequence.Null;
                case '\a': return EscapeSequence.Alert;
                case '\b': return EscapeSequence.Backspace;
                case '\f': return EscapeSequence.FormFeed;
                case '\n': return EscapeSequence.NewLine;
                case '\r': return EscapeSequence.CarriageReturn;
                case '\t': return EscapeSequence.HorizontalTab;
                case '\v': return EscapeSequence.VerticalTab;
                default: return EscapeSequence.None;
            }
        }

        public static string EscapeSequenceToString(this char c) {
            switch (InterpretEscapeSequence(c)) {
                case EscapeSequence.SingleQuote: return @"\'";
                case EscapeSequence.DoubleQuote: return "\\\"";
                case EscapeSequence.BackSlash: return @"\\";
                case EscapeSequence.Null: return @"\0";
                case EscapeSequence.Alert: return @"\a";
                case EscapeSequence.Backspace: return @"\b";
                case EscapeSequence.FormFeed: return @"\f";
                case EscapeSequence.NewLine: return @"\n";
                case EscapeSequence.CarriageReturn: return @"\r";
                case EscapeSequence.HorizontalTab: return @"\t";
                case EscapeSequence.VerticalTab: return @"\v";
                default: return c.ToString();
            }
        }
    }
}
