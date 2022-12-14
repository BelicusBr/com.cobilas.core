using System;
using Cobilas.IO.Alf.Alfbt.Components;

namespace Cobilas.IO.Alf.Alfbt.Flags {
    /// <summary>O corpo base da bandeira.</summary>
    public class FlagBase : ICloneable {
        private string name;
        private string value;
        private AlfbtFlags flags;
#pragma warning disable CS1591
        public string Name => name;
        public string Value => value;
#pragma warning restore CS1591
        /// <summary>O tipo da bandeira.</summary>
        public AlfbtFlags Flags => flags;

#pragma warning disable CS1591
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
#pragma warning restore CS1591
    }
}
