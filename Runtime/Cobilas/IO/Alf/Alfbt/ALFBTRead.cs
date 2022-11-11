using System;
using System.IO;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Alfbt.Compiler;
using Cobilas.IO.Alf.Alfbt.Components;

/*
 * bandeira de marcação   = marking flag
 * bandeira de texto      = text flag
 * bandeira de cabeçalho  = header flag
 * bandeira de comentario = comment flag
 */
namespace Cobilas.IO.Alf.Alfbt {
#pragma warning disable CS1591
    /// <summary>Responsavel pela leitura de um texto alfbt.</summary>
    public sealed class ALFBTRead : IDisposable {
        private bool disposedValue;
        private FlagBase[] flags;

        public int FlagCount => ArrayManipulation.ArrayLength(flags);

        internal ALFBTRead(string[] args) {
            flags = Compiler_ALFBT_1_0.Decompiler(args);
        }

        ~ALFBTRead()
            => Dispose();

        public HeaderFlag GetHeaderFlag()
            => new HeaderFlag(
                GetFlag(Compiler_ALFBT_1_0.n_Version, AlfbtFlags.HeaderFlag),
                GetFlag(Compiler_ALFBT_1_0.n_Type, AlfbtFlags.HeaderFlag),
                GetFlag(Compiler_ALFBT_1_0.n_Encoding, AlfbtFlags.HeaderFlag)
                );

        public MarkingFlag GetMarkingFlag(string name)
            => new MarkingFlag(GetFlag(name, AlfbtFlags.MarkingFlag));

        public TextFlag GetTextFlag(string name)
            => new TextFlag(GetFlag(name, AlfbtFlags.TextFlag));

        public MarkingFlag[] GetAllMarkingFlags() {
            MarkingFlag[] res = null;
            for (int I = 0; I < FlagCount; I++)
                if (flags[I].Flags == AlfbtFlags.MarkingFlag)
                    ArrayManipulation.Add(new MarkingFlag(flags[I]), ref res);
            return res;
        }

        public TextFlag[] GetAllTextFlags() {
            TextFlag[] res = null;
            for (int I = 0; I < FlagCount; I++)
                if (flags[I].Flags == AlfbtFlags.TextFlag)
                    ArrayManipulation.Add(new TextFlag(flags[I]), ref res);
            return res;
        }

        public CommentFlag GetCommentFlag() {
            CommentFlag comment = new CommentFlag();
            for (int I = 0; I < FlagCount; I++)
                if (flags[I].Flags == AlfbtFlags.CommentFlag)
                    comment.Add(flags[I]);
            return comment;
        }

        public bool TextFlagExists(string name)
            => FlagExists(name, AlfbtFlags.TextFlag);

        public bool MarkingFlagExists(string name)
            => FlagExists(name, AlfbtFlags.MarkingFlag);

        public bool HeaderFlagExists(string name)
            => FlagExists(name, AlfbtFlags.HeaderFlag);

        public bool FlagExists(string name, AlfbtFlags flags) {
            for (int I = 0; I < FlagCount; I++)
                if (this.flags[I].Name == name && this.flags[I].Flags == flags)
                    return true;
            return false;
        }

        public void Dispose() {
            if (disposedValue) return;
            disposedValue = true;
            if (FlagCount > 0)
                ArrayManipulation.ClearArray(ref flags);
        }

        private FlagBase GetFlag(string name, AlfbtFlags flags) {
            for (int I = 0; I < FlagCount; I++)
                if (this.flags[I].Name == name && this.flags[I].Flags == flags)
                    return this.flags[I];
            return (FlagBase)null;
        }

        public static ALFBTRead Create(Stream stream, Encoding encoding) {
            string[] args = null;
            using (StreamReader reader = new StreamReader(stream, encoding))
                while (!reader.EndOfStream)
                    ArrayManipulation.Add(reader.ReadLine(), ref args);
            return new ALFBTRead(args);
        }

        public static ALFBTRead Create(Stream stream) {
            string[] args = null;
            using (StreamReader reader = new StreamReader(stream))
                while (!reader.EndOfStream)
                    ArrayManipulation.Add(reader.ReadLine(), ref args);
            return new ALFBTRead(args);
        }

        public static ALFBTRead Create(TextReader reader) {
            string[] args = null;
            string temp;
            while ((temp = reader.ReadLine()) != null)
                ArrayManipulation.Add(temp, ref args);
            return new ALFBTRead(args);
        }

        public static ALFBTRead Create(string path, Encoding encoding)
            => Create(File.OpenRead(path), encoding);

        public static ALFBTRead Create(string path)
            => Create(File.OpenRead(path));
    }
#pragma warning restore CS1591
}
