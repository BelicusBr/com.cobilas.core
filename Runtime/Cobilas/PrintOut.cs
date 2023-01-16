using System;
using System.Diagnostics;
using Cobilas.Collections;

namespace Cobilas {
    public static class PrintOut {

        /// <summary>Default separator CRLF</summary>
        public static string Separator = "\r\n";
        public static Action<object> Write = (o) => { Console.Write(o); };
        public static Action<object> WriteLine = (o) => { Write(o); Write(Separator); };

        public static void Print(params object[] values) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(values); I++)
                Write(values[I]);
        }

        public static void Print(object value)
            => Print(new object[] { value });

        public static void Print(IFormatProvider provider, string format, params object[] args)
            => Print(new object[] { string.Format(provider, format, args) });

        public static void Print(string format, params object[] args)
            => Print(new object[] { string.Format(format, args) });

        public static void PrintLine(object value)
            => WriteLine(value);

        public static void PrintLine(string format, params object[] args)
            => PrintLine(string.Format(format, args));

        public static void PrintLine(IFormatProvider provider, string format, params object[] args)
            => PrintLine(string.Format(provider, format, args));

        public static void TrackedPrint(object value)
        {
            PrintLine(MethodTrackingList(2));
            PrintLine(value);
        }

        public static void TrackedPrintLine(params object[] values)
        {
            PrintLine(MethodTrackingList(2));
            PrintLine(values);
        }

        public static void TrackedPrintLine(IFormatProvider provider, string format, params object[] args)
        {
            PrintLine(MethodTrackingList(2));
            PrintLine(provider, format, args);
        }

        public static void TrackedPrintLine(string format, params object[] args)
        {
            PrintLine(MethodTrackingList(2));
            PrintLine(format, args);
        }

        public static string MethodTrackingList(int startIndex)
        {
            StackFrame[] frames = TrackMethod();
            string Res = null;
            for (int I = startIndex; I < ArrayManipulation.ArrayLength(frames); I++)
                Res = string.Format("{0}{1}File name: {2} (C:{3} L:{4}) Method: {5}",
                    Res, (Res == null ? "" : Separator),
                    frames[I].GetFileName(), frames[I].GetFileLineNumber(),
                    frames[I].GetFileLineNumber(), frames[I].GetMethod());
            return Res;
        }

        public static StackFrame[] TrackMethod()
            => new StackTrace(true).GetFrames();

        public static void ResetSeparato()
            => Separator = "\r\n";

        public static void ResetWrite()
            => Write = (o) => Console.Write(o);

        public static void ResetWriteLine()
            => WriteLine = (o) => { Write(o); Write(Separator); };
    }
}
