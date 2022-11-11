using System;

namespace Cobilas.IO.Alf.Alfbt.Flags {
    /// <summary>Representa uma bandeira de marcação.</summary>
    public struct MarkingFlag : ICloneable {
        private string name;
        private string value;

#pragma warning disable CS1591
        public string Name => name;
        public string Value => value;
#pragma warning restore CS1591

        internal MarkingFlag(string name, string value) {
            this.name = name;
            this.value = value;
        }

        internal MarkingFlag(FlagBase flag) :
            this(flag.Name, flag.Value) { }

#pragma warning disable CS1591
        public override string ToString() {
            return $"Name:{name}\n" +
                $"Value:{value}\n";
        }

        public object Clone()
            => new MarkingFlag(
                name is null ? string.Empty : (string)name.Clone(),
                value is null ? string.Empty : (string)value.Clone()
                );
#pragma warning restore CS1591
    }
}
