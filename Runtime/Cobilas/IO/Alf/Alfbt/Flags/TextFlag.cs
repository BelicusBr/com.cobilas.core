using System;
using System.Text;
using System.Globalization;

namespace Cobilas.IO.Alf.Alfbt.Flags {
    /// <summary>Representa uma bandeira de texto.</summary>
    public struct TextFlag : ICloneable {
        private readonly string name;
        private readonly string value;

        public string Name => name;
        public string Value => value;

        internal TextFlag(string name, string value) {
            this.name = name;
            this.value = value;
        }

        internal TextFlag(FlagBase flag) :
            this(flag.Name, flag.Value) { }

        /// <summary>Obtenha o texto de forma formatada.</summary>
        public string Format(IFormatProvider provider, params object[] args)
            => I_Format(provider, args);

        /// <summary>Obtenha o texto de forma formatada.</summary>
        public string Format(params object[] args)
            => I_Format(CultureInfo.CurrentCulture, args);

        /// <summary>Obtenha o texto de forma formatada.</summary>
        public string Format(object arg1, object arg2, object arg3, object arg4)
            => I_Format(arg1, arg2, arg3, arg4);

        /// <summary>Obtenha o texto de forma formatada.</summary>
        public string Format(object arg1, object arg2, object arg3)
            => I_Format(arg1, arg2, arg3);

        /// <summary>Obtenha o texto de forma formatada.</summary>
        public string Format(object arg1, object arg2)
            => I_Format(arg1, arg2);

        /// <summary>Obtenha o texto de forma formatada.</summary>
        public string Format(object arg1)
            => I_Format(arg1);

        private string I_Format(IFormatProvider provider, params object[] args)
            => string.Format(provider, value, args);

        private string I_Format(params object[] args)
            => I_Format(CultureInfo.CurrentCulture, args);
        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Name:{name} {{");
            builder.AppendLine(value);
            builder.AppendLine("}");
            return builder.ToString();
        }

        public object Clone()
            => new TextFlag(
                name is null ? string.Empty : (string)name.Clone(),
                value is null ? string.Empty : (string)value.Clone()
                );
    }
}
