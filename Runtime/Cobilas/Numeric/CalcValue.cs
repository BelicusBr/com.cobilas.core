using System;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Cobilas.Numeric {
    public class CalcValue : BaseCalcItem {
        private string value;
        private static readonly CalcValue TempCalcValue = new CalcValue("0");

        public override string Item => value;

        public CalcValue(string value) {
            if (!IsValidNumber(value))
                throw new FormatException("Invalid numeric value!!!");
            this.value = value;
        }

        public bool IsNegativeValue()
            => value.Contains('-');

        public void ABS() {
            if (IsNegativeValue())
                value = value.Replace("-", "");
        }

        public void NEGT() {
            if (!IsNegativeValue())
                value = $"-{value}";
        }

        public override string ToString()
            => $"Value[{value}]";

        public static bool IsValidNumber(CalcValue calc)
            => IsValidNumber(calc.value);

        public static bool IsValidNumber(string calc)
            => double.TryParse(calc, NumberStyles.Any, CultureInfo.InvariantCulture, out _);

        public static CalcValue CreateTemporaryCalcValue(string value)
            => ModifyValue(TempCalcValue, value);

        public static CalcValue ModifyValue(CalcValue calc, string value) {
            if (!IsValidNumber(value))
                throw new FormatException("Invalid numeric value!!!");
            StringBuilder builder = new StringBuilder(calc.value);
            builder.Append(value);
            calc.value = builder.ToString();
            return calc;
        }
    }
}
