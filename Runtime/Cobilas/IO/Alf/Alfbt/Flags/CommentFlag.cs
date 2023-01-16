using System;
using System.Text;
using Cobilas.Collections;

namespace Cobilas.IO.Alf.Alfbt.Flags {
    /// <summary>Contem todos os comentarios do arquivo .alfbt</summary>
    public struct CommentFlag : ICloneable {
        private FlagBase[] flags;

        public int FlagCount => ArrayManipulation.ArrayLength(flags);
        public long l_FlagCount => ArrayManipulation.EmpytArray(flags) ? 0 : flags.LongLength;

        internal void Add(FlagBase flag)
            => ArrayManipulation.Add(flag, ref flags);

        /// <summary>Obtem o comentario na lista.</summary>
        public string GetComment(int index)
            => flags[index].Value;

        /// <summary>Obtem o comentario na lista.</summary>
        public string GetComment(long index)
            => flags[index].Value;

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < FlagCount; I++)
                builder.AppendFormat("{0}:{1}\n", flags[I].Name, flags[I].Value);
            return builder.ToString();
        }
        
        public object Clone() {
            CommentFlag res = new CommentFlag();
            res.flags = flags is null ? (FlagBase[])null : (FlagBase[])flags.Clone();
            return res;
        }
    }
}
