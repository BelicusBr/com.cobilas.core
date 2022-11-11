using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Cobilas.IO.Alf.Alfbt.Components {
#pragma warning disable CS1591
    [Serializable]
    public class ALFBTFormatException : Exception {
        public ALFBTFormatException() { }
        public ALFBTFormatException(string message) : base(message) { }
        public ALFBTFormatException(string message, Exception inner) : base(message, inner) { }
        protected ALFBTFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#pragma warning restore CS1591

        internal static ALFBTFormatException GetException(string msm_en_US, string other) {
            switch (CultureInfo.CurrentCulture.Name) {
                case "en-US": return new ALFBTFormatException(msm_en_US);
                default: return new ALFBTFormatException(other);
            }
        }

        internal static ALFBTFormatException GetRepeatedFlag(string name, AlfbtFlags type)
            => GetException(
                $"Flag {name}(type:{type}) already exists!",
                $"Bandeira {name}(tipo:{type}) já existe!"
                );

        internal static ALFBTFormatException GetHeaderSecond()
            => GetException(
                "Header must be above the text and markup flags!",
                "Cabeçalho deve estar acima das bandeiras de texto e marcação!"
                );

        internal static ALFBTFormatException GetInvalidFormattingException(int line)
            => GetException(
                $"(Line: {line})Flag formatting invalid!",
                $"(Linha: {line})Formatação de bandeira invalida!"
                );

        internal static ALFBTFormatException GetHeaderFlagNameInvalid(string name)
            => GetException(
                $"(Name:{name})Header flag name invalid! Use (version, tipe, Encoding)",
                $"(Nome:{name})Nome bandeira de cabeçalho invalido! Usar (version, type, Encoding)"
                );

        internal static ALFBTFormatException GetEmptyValue(bool name = true)
            => GetException(
                $"({(name ? "Name" : "Value")})Value cannot be empty!",
                $"({(name ? "Nome" : "Valor")})O valor não pode ser vazio!"
                );

        internal static ALFBTFormatException GetWhiteSpaceException(int line, bool isName)
            => GetException(
                $"(Line: {line})The {(isName ? "name" : "value")} cannot contain blanks!",
                $"(Linha: {line})O {(isName ? "nome" : "valor")} não pode conter espaços em braco!"
                );

        internal static ALFBTFormatException GetWhiteSpaceException(string value)
            => GetException(
                $"(Value:{value})The value cannot contain blanks!",
                $"(Valor:{value})O valor não pode conter espaços em branco!"
                );

        internal static ALFBTFormatException GetFlagNameException(int line)
            => GetException(
                    $"(Line: {line})The flag must contain a name!",
                    $"(Linha: {line})A bandeira deve conter um nome!"
                );

        internal static ALFBTFormatException GetInvalidChar(string value, char error, int line)
            => GetException(
                $"(Line: {line})\"{value}\" value has invalid \"{error}\" character! Valid characters('_', '.', '\\', '/').",
                $"(Linha: {line})O valor \"{value}\" possui o carácter \"{error}\" inválido! Caracteres válidos('_', '.', '\\', '/')."
                );

        internal static ALFBTFormatException GetFlagValueException(int line)
            => GetException(
                    $"(Line: {line})The flag must contain a value!",
                    $"(Linha: {line})A bandeira deve conter um valor!"
                );
    }
}
