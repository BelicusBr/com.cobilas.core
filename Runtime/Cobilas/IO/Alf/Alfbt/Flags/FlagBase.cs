using System;
using Cobilas.IO.Alf.Alfbt.Components;

namespace Cobilas.IO.Alf.Alfbt.Flags {
    /// <summary>O corpo base da bandeira.</summary>
    public class FlagBase : ICloneable {
        private readonly string name;
        private readonly string value;
        private readonly AlfbtFlags flags;

        public string Name => name;
        public string Value => value;
        /// <summary>O tipo da bandeira.</summary>
        public AlfbtFlags Flags => flags;

        public FlagBase(string name, string value, AlfbtFlags flags) {
            this.name = name;
            this.value = value;
            this.flags = flags;
        }

        public object Clone()
            => new FlagBase(
                name is null ? string.Empty : (string)name.Clone(),
                value is null ? string.Empty : (string)value.Clone(),
                flags
                );
    }
}
