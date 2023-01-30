using System;

namespace Cobilas.Numeric {
    public class CalcSign : BaseCalcItem {
        private readonly string value;
        public override string Item => value;

        public CalcSign(string sign) {
            if (InvalidSign(sign))
                throw new FormatException("Invalid arithmetic sign!!!");
            value = sign;
        }

        public CalcSign(char sign) : this(sign.ToString()) { }

        public bool IsUnarySign() {
            switch (value) {
                case "+": return true;
                case "-": return true;
                case "*": return true;
                case "/": return true;
                case "%": return true;
                default: return false;
            }
        }

        public bool IsBinarySign() {
            switch (value) {
                case "++": return true;
                case "--": return true;
                case "**": return true;
                case "//": return true;
                default: return false;
            }
        }

        public bool IsSeparatorSign() {
            switch (value) {
                case "(": return true;
                case ")": return true;
                default: return false;
            }
        }

        public bool IsMathSign() {
            switch (value) {
                case "Sqrt": return true;
                case "Pow": return true;
                default: return false;
            }
        }

        public override string ToString()
                => $"Sign[{value}]";

        private bool InvalidSign(string sign) {
            switch (sign) {
                case "+": return false;
                case "-": return false;
                case "*": return false;
                case "/": return false;
                case "%": return false;
                case "(": return false;
                case ")": return false;
                case "++": return false;
                case "--": return false;
                case "**": return false;
                case "//": return false;
                case "Sqrt": return false;
                case "Pow": return false;
                default: return true;
            }
        }
    }
}
