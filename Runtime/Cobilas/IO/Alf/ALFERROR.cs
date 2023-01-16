namespace Cobilas.IO.Alf {
    internal static class ALFERROR {

        public static ALFException PrintError(string format, params object[] args)
            => new ALFException(string.Format(format, args));

        public static ALFException PrintError(string arg)
            => PrintError("{0}", arg);

        public static ALFException PrintLineError(string format, int line, params object[] args)
            => PrintError(string.Format("(L:{0}){1}", line, format), args);

        public static ALFException TextElementERROR(int line)
            => PrintError("(L:{0}){1}", line, "the text element has not been started!");

        public static ALFException CommentERROR(int line)
            => PrintError("(L:{0}){1}", line, "the comment must start on an empty line!");

        public static ALFException UnidentifiedElement(int line, string txt)
            => PrintError("(L:{0})element ({1}) not identified!", line, txt);
    }
}
