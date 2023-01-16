using System;
using System.Text;

namespace Cobilas.IO.Alf.Alfbt.Flags {
    /// <summary>Representa a bandeira de cabeçalho.</summary>
    public struct HeaderFlag : ICloneable {
        private FlagBase[] flags;

        /// <summary>Tipo do programa.</summary>
        public string Type => GetValue(1);
        /// <summary>Versão do programa.</summary>
        public string Version => GetValue(0);
        /// <summary>Decodificador do programa.</summary>
        public string Encoding => GetValue(2);

        internal HeaderFlag(FlagBase version, FlagBase type, FlagBase encoding) {
            flags = new FlagBase[] {
                version,
                type,
                encoding
            };
        }

        private string GetValue(byte index)
            => flags[index] == null ? "vl_null" : flags[index].Value;

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            if (flags[0] != null)
                builder.AppendLine($"Version:{flags[0].Value}");
            if (flags[1] != null)
                builder.AppendLine($"Type:{flags[1].Value}");
            if (flags[2] != null)
                builder.AppendLine($"Encoding:{flags[2].Value}");
            return builder.ToString();
        }

        public object Clone() {
            HeaderFlag res = new HeaderFlag();
            res.flags = flags is null ? (FlagBase[])null : (FlagBase[])flags.Clone();
            return res;
        }
    }
}
