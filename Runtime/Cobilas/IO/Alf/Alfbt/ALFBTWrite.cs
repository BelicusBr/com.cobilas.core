using System;
using System.IO;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Alfbt.Compiler;
using Cobilas.IO.Alf.Alfbt.Components;

namespace Cobilas.IO.Alf.Alfbt {
    /// <summary>Responsavel por escrever um texto alfbt</summary>
    public sealed class ALFBTWrite : IDisposable {
        private bool append;
        private Stream stream;
        private FlagBase[] flags;
        private bool headerSecond;
        private Encoding encoding;
        private bool disposedValue;

#pragma warning disable CS1591
        public int FlagCount => ArrayManipulation.ArrayLength(flags);

        internal ALFBTWrite(Stream stream, bool append, Encoding encoding) {
            this.stream = stream;
            this.encoding = encoding;
            this.append = append;
        }

        ~ALFBTWrite()
            => Dispose();

        public void Write(StringBuilder builder)
            => Write(builder.ToString());

        public void Write(string text)
            => Write(new StringReader(text));

        public void Write(StringReader reader) {
            string[] args = null;
            string temp;
            while ((temp = reader.ReadLine()) != null)
                ArrayManipulation.Add(temp, ref args);
            ArrayManipulation.Add(Compiler_ALFBT_1_0.Decompiler(args), ref flags);
            headerSecond = true;
        }

        public void WriteVersionHeaderFlag() {
            if (headerSecond) throw ALFBTFormatException.GetHeaderSecond();
            AddFlag(Compiler_ALFBT_1_0.n_Version, Compiler_ALFBT_1_0.m_CompilerVersion, AlfbtFlags.HeaderFlag);
        }

        public void WriteTypeHeaderFlag() {
            if (headerSecond) throw ALFBTFormatException.GetHeaderSecond();
            AddFlag(Compiler_ALFBT_1_0.n_Type, Compiler_ALFBT_1_0.m_CompilerType, AlfbtFlags.HeaderFlag);
        }

        public void WriteEncodingHeaderFlag() {
            if (headerSecond) throw ALFBTFormatException.GetHeaderSecond();
            AddFlag(Compiler_ALFBT_1_0.n_Encoding, encoding.BodyName, AlfbtFlags.HeaderFlag);
        }

        public void WriteHeaderFlag() {
            WriteVersionHeaderFlag();
            WriteTypeHeaderFlag();
            WriteEncodingHeaderFlag();
        }

        public void WriteTextFlag(string name, string text) {
            headerSecond = true;
            AddFlag(name, text, AlfbtFlags.TextFlag);
        }

        public void WriteMarkingFlag(string name, string text) {
            headerSecond = true;
            AddFlag(name, text, AlfbtFlags.MarkingFlag);
        }

        public void WriteCommentFlag(string text)
            => AddFlag("cmt", text, AlfbtFlags.CommentFlag);

        public void WriteLineBreak()
            => AddFlag("lb", "\n", AlfbtFlags.LineBreak);
#pragma warning restore CS1591

        /// <summary>Fecha o <see cref="Stream"/> atual.</summary>
        public void CloseFlow()
            => stream.Dispose();

        /// <summary>Dispensa todos os recursos usados no <see cref="ALFBTWrite"/>.</summary>
        public void Dispose() {
            if (disposedValue) return;
            disposedValue = true;

            byte[] res = encoding.GetBytes(Compiler_ALFBT_1_0.Compiler(flags));
            if (!append) stream.SetLength(res.LongLength);
            stream.Write(res, 0, res.Length);

            if (FlagCount > 0)
                ArrayManipulation.ClearArray(ref flags);
            encoding = null;
            stream = null;
        }

        private FlagBase Contains(FlagBase flag) {
            for (int I = 0; I < FlagCount; I++)
                if (flag.Flags != AlfbtFlags.CommentFlag &&
                    flag.Flags != AlfbtFlags.LineBreak &&
                    flags[I].Name == flag.Name &&
                    flags[I].Flags == flag.Flags)
                    throw ALFBTFormatException.GetRepeatedFlag(flag.Name, flag.Flags);
            return flag;
        }

        private void AddFlag(string name, string value, AlfbtFlags flag)
            => ArrayManipulation.Add(Contains(new FlagBase(name, value, flag)), ref flags);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// </summary>
        public static ALFBTWrite Create(Stream stream, bool append, Encoding encoding)
            => new ALFBTWrite(stream, append, encoding);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// <para>Defaut <see cref="Encoding"/>.UTF8</para>
        /// </summary>
        public static ALFBTWrite Create(Stream stream, bool append)
            => Create(stream, append, Encoding.UTF8);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// <para>Defaut <see cref="Encoding"/>.UTF8</para>
        /// </summary>
        public static ALFBTWrite Create(Stream stream)
            => Create(stream, Encoding.UTF8);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// <para>Defaut append:true</para>
        /// </summary>
        public static ALFBTWrite Create(Stream stream, Encoding encoding)
            => Create(stream, true, encoding);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// </summary>
        public static ALFBTWrite Create(string path, bool append, Encoding encoding)
            => Create(File.OpenWrite(path), append, encoding);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// <para>Defaut append:false</para>
        /// </summary>
        public static ALFBTWrite Create(string path, Encoding encoding)
            => Create(path, false, encoding);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// <para>Defaut <see cref="Encoding"/>.UTF8</para>
        /// </summary>
        public static ALFBTWrite Create(string path, bool append)
            => Create(path, append, Encoding.UTF8);

        /// <summary>
        /// creates an <see cref="ALFBTWrite"/> instance
        /// <para>Defaut <see cref="Encoding"/>.UTF8</para>
        /// <para>Defaut append:false</para>
        /// </summary>
        public static ALFBTWrite Create(string path)
            => Create(path, false, Encoding.UTF8);
    }
}
