using System;
using System.Diagnostics;

namespace Cobilas {
    [Obsolete("Use PrintOut class")]
    public static class CobilasConsole {

        /// <summary>Default separator CRLF</summary>
        public static Action<object> Write { get => PrintOut.Write; set => PrintOut.Write = value; }
        public static string Separator { get => PrintOut.Separator; set => PrintOut.Separator = value; }
        public static Action<object> WriteLine { get => PrintOut.WriteLine; set => PrintOut.WriteLine = value; }

        public static void Print(params object[] values) => PrintOut.Print(values);
        public static void Print(object value) => PrintOut.Print(value);
        public static void Print(IFormatProvider provider, string format, params object[] args) => PrintOut.Print(provider, format, args);
        public static void Print(string format, params object[] args) => PrintOut.Print(format, args);

        public static void PrintLine(object value) => PrintOut.Print(value);
        public static void PrintLine(string format, params object[] args) => PrintOut.Print(format, args);
        public static void PrintLine(IFormatProvider provider, string format, params object[] args) => PrintOut.Print(provider, format, args);

        public static void TrackedPrint(object value) => PrintOut.TrackedPrint(value);
        public static void TrackedPrintLine(params object[] values) => PrintOut.TrackedPrintLine(values);
        public static void TrackedPrintLine(string format, params object[] args) => PrintOut.TrackedPrintLine(format, args);
        public static void TrackedPrintLine(IFormatProvider provider, string format, params object[] args) => PrintOut.TrackedPrintLine(provider, format, args);

        public static StackFrame[] TrackMethod() => PrintOut.TrackMethod();
        public static string MethodTrackingList(int startIndex) => PrintOut.MethodTrackingList(startIndex);

        public static void ResetSeparato() => PrintOut.ResetSeparato();
        public static void ResetWrite() => PrintOut.ResetWrite();
        public static void ResetWriteLine() => PrintOut.ResetWriteLine();
    }
}
