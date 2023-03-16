using System;
using System.Text;
using Cobilas.Collections;
using System.Globalization;

namespace Cobilas.Numeric {
    public class Calculator {
        private BaseCalcItem[] itens;
        private bool applyOrderOfOperations = false;

        public bool ApplyOrderOfOperations {
            get => applyOrderOfOperations;
            set => applyOrderOfOperations = value;
        }

        public int Count => ArrayManipulation.ArrayLength(itens);

        public Calculator(BaseCalcItem[] itens, bool applyOrderOfOperations) {
            this.itens = itens;
            this.applyOrderOfOperations = applyOrderOfOperations;
        }

        public Calculator(BaseCalcItem[] itens) : this(itens, false) { }

        public Calculator(string value, bool applyOrderOfOperations) {
            AddFormula(value);
            this.applyOrderOfOperations = applyOrderOfOperations;
        }

        public Calculator(string value) : this(value, false) { }

        public Calculator() : this((BaseCalcItem[])null) { }

        public void Clear() {
            int indexTemp = Count - 1;
            ArrayManipulation.Remove(indexTemp < 0 ? 0 : indexTemp, ref itens);
        }

        public void ClearAll()
            => ArrayManipulation.ClearArraySafe(ref itens);

        public void Abs() {
            if (itens[Count - 1] is CalcValue)
                (itens[Count - 1] as CalcValue).ABS();
        }

        public void Negt() {
            if (itens[Count - 1] is CalcValue)
                (itens[Count - 1] as CalcValue).NEGT();
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            foreach (BaseCalcItem item in itens)
                if (item is CalcSign) {
                    CalcSign Temp = item as CalcSign;
                    if (Temp.IsBinarySign())
                        builder.AppendFormat("{0} ", (string)Temp);
                    else if (Temp.IsUnarySign())
                        builder.AppendFormat(" {0} ", (string)Temp);
                    else if (Temp.IsMathSign())
                        builder.AppendFormat(":{0}:", (string)Temp);
                    else builder.Append((string)Temp);
                } else builder.Append((string)item);
            return builder.ToString();
        }

        public void AddAddition()
            => AddSignal('+');

        public void AddSubtraction()
            => AddSignal('-');

        public void AddMultiplication()
            => AddSignal('*');

        public void AddDivision()
            => AddSignal('/');

        public void AddRest()
            => AddSignal('%');

        public void AddIncrement()
            => AddSignal("++");

        public void AddDecrement()
            => AddSignal("--");

        public void AddMult_Increment()
            => AddSignal("**");

        public void AddDivi_Decrement()
            => AddSignal("//");

        public void OpenParentheses()
            => AddSignal("(");

        public void CloseParentheses()
            => AddSignal(")");

        public void AddSqrt()
            => AddSignal("Sqrt");

        public void AddPow()
            => AddSignal("Pow");

        public void AddFormula(string format)
            => AddFormula(format.Replace("(", " ( ").Replace(")", " ) ").Replace(':', ' ').Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

        public void AddFormula(params string[] itens) {
            foreach (string item in itens) {
                switch (item) {
                    case "+": case "-": case "*":
                    case "/": case "%": case "++":
                    case "--": case "**": case "//":
                    case "(": case ")": case "Sqrt":
                    case "Pow":
                        AddSignal(item);
                        break;
                    default:
                        try {
                            AddValue(item);
                        } catch {
                            throw new FormatException($"Invalid item:{item}");
                        }
                        break;
                }
            }
        }

        public virtual bool TryCalc(out double res) {
            try {
                res = Calc();
                return true;
            } catch {
                res = 0;
                return false;
            }
        }

        public virtual bool TryCalc(out double res, out Exception exception) {
            try {
                res = Calc();
                exception = (Exception)null;
                return true;
            } catch (Exception e) {
                res = 0;
                exception = e;
                return false;
            }
        }

        public virtual void AddValue(string _value) {
            if (Count == 0) ArrayManipulation.Add(new CalcValue(_value), ref itens);
            else {
                if (itens[Count - 1] is CalcValue)
                    itens[Count - 1] = CalcValue.ModifyValue(itens[Count - 1] as CalcValue, _value);
                else ArrayManipulation.Add(new CalcValue(_value), ref itens);
            }
        }

        public virtual void AddValue(double _value)
            => AddValue(_value.ToString(CultureInfo.InvariantCulture));

        public virtual double Calc() {
            if (applyOrderOfOperations)
                return double.Parse((itens = CalcSecondOrder(CalcFirstOrder(CalcMathOrder(CalcBinaryOrder(CalcBetweenParenthesis(itens))))))[0].Item, CultureInfo.InvariantCulture);

            return double.Parse((itens = CalcThirdOrder(CalcMathOrder(CalcBinaryOrder(CalcBetweenParenthesis(itens)))))[0].Item, CultureInfo.InvariantCulture);
        }

        protected virtual void AddSignal(string signal)
            => ArrayManipulation.Add(new CalcSign(signal), ref itens);

        protected virtual void AddSignal(char signal)
            => AddSignal(signal.ToString());

        /// <summary> Realiza calculos de (*, /, %) </summary>
        protected virtual BaseCalcItem[] CalcFirstOrder(BaseCalcItem[] itens) {
            int Index;
            while (
                (Index = BaseCalcItem.IndexOf(itens, "*", "/", "%")) > -1) {
                string Res = CalcUnary(
                    (CalcValue)itens[Index - 1],
                    (CalcSign)itens[Index],
                    (CalcValue)itens[Index + 1]);
                ArrayManipulation.Remove(Index - 1, 3, ref itens);
                if (Index - 1 > ArrayManipulation.ArrayLength(itens) - 1)
                    ArrayManipulation.Add(new CalcValue(Res), ref itens);
                else ArrayManipulation.Insert(new CalcValue(Res), Index - 1, ref itens);
            }
            return itens;
        }

        /// <summary> Realiza calculos de (+, -) </summary>
        protected virtual BaseCalcItem[] CalcSecondOrder(BaseCalcItem[] itens) {
            int Index;
            while ((Index = BaseCalcItem.IndexOf(itens, "+", "-")) > -1) {
                string Res = CalcUnary(
                    (CalcValue)itens[Index - 1],
                    (CalcSign)itens[Index],
                    (CalcValue)itens[Index + 1]);
                ArrayManipulation.Remove(Index - 1, 3, ref itens);
                if (Index - 1 > ArrayManipulation.ArrayLength(itens) - 1)
                    ArrayManipulation.Add(new CalcValue(Res), ref itens);
                else ArrayManipulation.Insert(new CalcValue(Res), Index - 1, ref itens);
            }
            return itens;
        }

        protected virtual BaseCalcItem[] CalcThirdOrder(BaseCalcItem[] itens)
            => CalcFirstOrder(CalcSecondOrder(itens));

        protected virtual BaseCalcItem[] CalcBetweenParenthesis(BaseCalcItem[] itens) {
            int Index = -1;
            int ParamsStart = 0;
            int ParamsEnd;
            while ((ParamsEnd = Index = BaseCalcItem.IndexOf(Index + 1, itens, "(", ")")) > -1) {
                if ((string)itens[Index] == "(") ParamsStart = Index;
                else if ((string)itens[Index] == ")") {
                    Index = ParamsEnd - (ParamsStart + 1);
                    BaseCalcItem[] Temp = CalcSecondOrder(CalcFirstOrder(CalcMathOrder(CalcBinaryOrder(ArrayManipulation.TakeStretch(ParamsStart + 1, Index, itens)))));
                    Index = ParamsEnd - ParamsStart;
                    ArrayManipulation.Remove(ParamsStart, Index + 1, ref itens);
                    if (ParamsStart > ArrayManipulation.ArrayLength(itens) - 1) ArrayManipulation.Add(Temp, ref itens);
                    else ArrayManipulation.Insert(Temp, ParamsStart, ref itens);
                    Index = -1;
                }
            }
            return itens;
        }

        protected virtual BaseCalcItem[] CalcBinaryOrder(BaseCalcItem[] itens) {
            int Index;
            while ((Index = BaseCalcItem.IndexOf(itens, "++", "--", "**", "//")) > -1) {
                string Res = CalcBinary(
                    (CalcValue)itens[Index - 1],
                    (CalcSign)itens[Index]);
                ArrayManipulation.Remove(Index - 1, 2, ref itens);
                if (Index - 1 > ArrayManipulation.ArrayLength(itens) - 1)
                    ArrayManipulation.Add(new CalcValue(Res), ref itens);
                else ArrayManipulation.Insert(new CalcValue(Res), Index - 1, ref itens);
            }
            return itens;
        }

        protected virtual BaseCalcItem[] CalcMathOrder(BaseCalcItem[] itens) {
            int Index;
            while ((Index = BaseCalcItem.IndexOf(itens, "Sqrt", "Pow")) > -1) {
                int Compri;
                string Res;
                if ((string)itens[Index] == "Sqrt") {
                    Compri = 2;
                    if (Index + 1 <= ArrayManipulation.ArrayLength(itens) - 1) {
                        if (itens[Index + 1] is CalcSign) {
                            Res = CalcMath(
                                (CalcValue)itens[Index - 1],
                                (CalcSign)itens[Index],
                                CalcValue.CreateTemporaryCalcValue("1"));
                        } else {
                            Res = CalcMath(
                                (CalcValue)itens[Index - 1],
                                (CalcSign)itens[Index],
                                (CalcValue)itens[Index + 1]);
                            Compri = 3;
                        }
                    } else {
                        Res = CalcMath(
                            (CalcValue)itens[Index - 1],
                            (CalcSign)itens[Index],
                            CalcValue.CreateTemporaryCalcValue("1"));
                    }
                } else {
                    Res = CalcMath(
                        (CalcValue)itens[Index - 1],
                        (CalcSign)itens[Index],
                        (CalcValue)itens[Index + 1]);
                    Compri = 3;
                }
                ArrayManipulation.Remove(Index - 1, Compri, ref itens);
                if (Index - 1 > ArrayManipulation.ArrayLength(itens) - 1)
                    ArrayManipulation.Add(new CalcValue(Res), ref itens);
                else ArrayManipulation.Insert(new CalcValue(Res), Index - 1, ref itens);
            }
            return itens;
        }

        protected virtual string CalcMath(CalcValue A, CalcSign S, CalcValue B) {
            double Ad = double.Parse((string)A, CultureInfo.InvariantCulture);
            double Bd = double.Parse((string)B, CultureInfo.InvariantCulture);
            switch ((string)S) {
                case "Sqrt": return (Math.Sqrt(Ad) * Bd).ToString(CultureInfo.InvariantCulture);
                case "Pow": return Math.Pow(Ad, Bd).ToString(CultureInfo.InvariantCulture);
                default: return "0";
            }
        }

        protected virtual string CalcBinary(CalcValue A, CalcSign S) {
            double Ad = double.Parse((string)A, CultureInfo.InvariantCulture);
            switch ((string)S) {
                case "++": return (Ad + 1).ToString(CultureInfo.InvariantCulture);
                case "--": return (Ad - 1).ToString(CultureInfo.InvariantCulture);
                case "**": return (Ad * 2).ToString(CultureInfo.InvariantCulture);
                case "//": return (Ad / 2).ToString(CultureInfo.InvariantCulture);
                default: return "0";
            }
        }

        protected virtual string CalcUnary(CalcValue A, CalcSign S, CalcValue B) {
            double Ad = double.Parse((string)A, CultureInfo.InvariantCulture);
            double Bd = double.Parse((string)B, CultureInfo.InvariantCulture);
            switch ((string)S) {
                case "+": return (Ad + Bd).ToString(CultureInfo.InvariantCulture);
                case "-": return (Ad - Bd).ToString(CultureInfo.InvariantCulture);
                case "*": return (Ad * Bd).ToString(CultureInfo.InvariantCulture);
                case "/": return (Ad / Bd).ToString(CultureInfo.InvariantCulture);
                case "%": return (Ad % Bd).ToString(CultureInfo.InvariantCulture);
                default: return "0";
            }
        }

        public static double Calc(string valeu, bool applyOrderOfOperations)
            => new Calculator(valeu, applyOrderOfOperations).Calc();

        public static double Calc(string valeu)
            => Calc(valeu, false);

        public static void TryCalc(string valeu, bool applyOrderOfOperations, out double res)
            => new Calculator(valeu, applyOrderOfOperations).TryCalc(out res);

        public static void TryCalc(string valeu, bool applyOrderOfOperations, out double res, out Exception exception)
            => new Calculator(valeu, applyOrderOfOperations).TryCalc(out res, out exception);

        public static void TryCalc(string valeu, out double res)
            => TryCalc(valeu, false, out res);

        public static void TryCalc(string valeu, out double res, out Exception exception)
            => TryCalc(valeu, false, out res, out exception);
    }
}
