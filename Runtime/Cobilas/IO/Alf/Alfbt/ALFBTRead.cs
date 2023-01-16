using System;
using System.IO;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Alfbt.Flags;
using Cobilas.IO.Alf.Alfbt.Components;
using Cobilas.IO.Alf.Alfbt.Components.Compiler;
using Cobilas.IO.Alf.Alfbt.Components.Collections;
using System.Collections.Generic;
using System.Collections;

namespace Cobilas.IO.Alf.Alfbt {
    /// <summary>Responsavel pela leitura de um texto alfbt.</summary>
    public sealed class ALFBTRead : IDisposable, IReadOnlyArray<ALFBTFlagReadOnly> {

        private readonly ALFRead myRead;

        public int Count => ((IReadOnlyArray)ReadOnly).Count;
        public ALFBTFlagReadOnly ReadOnly => new ALFBTFlagReadOnly(myRead.ReadOnly);

        object IReadOnlyArray.this[int index] => ((IReadOnlyArray)ReadOnly)[index];
        public ALFBTFlagReadOnly this[int index] => ((IReadOnlyArray<ALFBTFlagReadOnly>)ReadOnly)[index];

        private ALFBTRead(ALFRead read) {
            this.myRead = read;
        }

        [Obsolete("Use ALFBTFlagReadOnly:GetFlag(string)")]
        public HeaderFlag GetHeaderFlag()
            => new HeaderFlag(
                GetFlag(ALFBTCompiler_1_0.n_Version),
                GetFlag(ALFBTCompiler_1_0.n_Type),
                GetFlag(ALFBTCompiler_1_0.n_Encoding)
                );

        [Obsolete("Use ALFBTFlagReadOnly:GetFlag(string)")]
        public MarkingFlag GetMarkingFlag(string name)
            => new MarkingFlag(I_GetMarkingFlag(name));

        [Obsolete("Use ALFBTFlagReadOnly:GetFlag(string)")]
        public TextFlag GetTextFlag(string name)
            => new TextFlag(I_GetTextFlag(name));

        [Obsolete("Use ALFBTFlagReadOnly:GetFlag(string)")]
        public MarkingFlag[] GetAllMarkingFlags() {
            MarkingFlag[] res = null;
            foreach (var item in ReadOnly)
                if (!item.ToString().Contains("\n"))
                    ArrayManipulation.Add(new MarkingFlag(item), ref res);
            return res;
        }

        [Obsolete("Use ALFBTFlagReadOnly:GetFlag(string)")]
        public TextFlag[] GetAllTextFlags() {
            TextFlag[] res = null;
            foreach (var item in ReadOnly)
                if (item.ToString().Contains("\n"))
                    ArrayManipulation.Add(new TextFlag(item), ref res);
            return res;
        }

        public CommentFlag GetCommentFlag() {
            CommentFlag comment = new CommentFlag();
            foreach (var item in ReadOnly)
                if (item.Name == "comment")
                    comment.Add(item);
            return comment;
        }

        [Obsolete("Use bool:FlagExists(string)")]
        public bool TextFlagExists(string name)
            => FlagExists(name, AlfbtFlags.TextFlag);

        [Obsolete("Use bool:FlagExists(string)")]
        public bool MarkingFlagExists(string name)
            => FlagExists(name, AlfbtFlags.MarkingFlag);

        [Obsolete("Use bool:FlagExists(string)")]
        public bool HeaderFlagExists(string name)
            => FlagExists(name, AlfbtFlags.HeaderFlag);

        [Obsolete("Use bool:FlagExists(string)")]
        public bool FlagExists(string name, AlfbtFlags flags) {
            bool res = false;
            switch (flags) {
                case AlfbtFlags.MarkingFlag:
                    res = !GetFlag(name).ToString().Contains("\n") &&
                        name != ALFBTCompiler_1_0.n_Version &&
                        name != ALFBTCompiler_1_0.n_Type &&
                        name != ALFBTCompiler_1_0.n_Encoding;
                    return res;
                case AlfbtFlags.TextFlag:
                    res = GetFlag(name).ToString().Contains("\n") &&
                    name != ALFBTCompiler_1_0.n_Version &&
                    name != ALFBTCompiler_1_0.n_Type &&
                    name != ALFBTCompiler_1_0.n_Encoding;
                    return res;
                case AlfbtFlags.HeaderFlag:
                    res = GetFlag(name) != (ALFBTFlagReadOnly)null &&
                    (name == ALFBTCompiler_1_0.n_Version ||
                    name == ALFBTCompiler_1_0.n_Type ||
                    name == ALFBTCompiler_1_0.n_Encoding);
                    return res;
                default: return res;
            }
        }

        public bool FlagExists(string name) {
            foreach (var item in ReadOnly)
                if (item.Name == name)
                    return true;
            return false;
        }

        public void Dispose() {
            myRead?.Dispose();
        }

        public ALFBTFlagReadOnly GetFlag(string name) {
            foreach (var item in ReadOnly)
                if (item.Name == name)
                    return item;
            return (ALFBTFlagReadOnly)null;
        }

        public IEnumerator<ALFBTFlagReadOnly> GetEnumerator()
            => ((IEnumerable<ALFBTFlagReadOnly>)ReadOnly).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)ReadOnly).GetEnumerator();

        private ALFBTFlagReadOnly I_GetMarkingFlag(string name) {
            ALFBTFlagReadOnly readOnly = GetFlag(name);
            if (readOnly != (ALFBTFlagReadOnly)null)
                if (!readOnly.ToString().Contains("\n"))
                    return readOnly;
            return (ALFBTFlagReadOnly)null;
        }

        private ALFBTFlagReadOnly I_GetTextFlag(string name) {
            ALFBTFlagReadOnly readOnly = GetFlag(name);
            if (readOnly != (ALFBTFlagReadOnly)null)
                if (readOnly.ToString().Contains("\n"))
                    return readOnly;
            return (ALFBTFlagReadOnly)null;
        }

        public static ALFBTRead Create(TextReader reader) {
            string[] lines = (string[])null;
            string line;
            while ((line = reader.ReadLine()) != (string)null)
                ArrayManipulation.Add(line, ref lines);

            using (StringWriter stringWriter = new StringWriter()) {
                using (ALFWriter writer = ALFWriter.Create(stringWriter))
                    ALFBTCompiler.Read(lines, writer);
                return new ALFBTRead(ALFRead.Create(new StringReader(stringWriter.ToString())));
            }
        }

        public static ALFBTRead Create(Stream stream, Encoding encoding)
            => Create(new StringReader(stream.GetString(encoding)));

        public static ALFBTRead Create(Stream stream)
            => Create(stream, Encoding.UTF8);
    }
}
