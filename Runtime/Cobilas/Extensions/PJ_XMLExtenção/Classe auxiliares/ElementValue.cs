using System.Globalization;

namespace System.Xml {
    public class ElementValue : IDisposable {
        private object value;
        private bool disposedValue;

        public bool IsEmpty => value == null;
        public object Value { get => value; set => this.value = value; }
        public string ValueToString => value.ToString();
        public char[] ValueToCharArray => ValueToString.ToCharArray();

        public sbyte ValueToSByte => Convert.ToSByte(value);
        public short ValueToShort => Convert.ToInt16(value);
        public int ValueToInt => Convert.ToInt32(value);
        public long ValueToLong => Convert.ToInt64(value);

        public byte ValueToByte => Convert.ToByte(value);
        public ushort ValueToUShort => Convert.ToUInt16(value);
        public uint ValueToUInt => Convert.ToUInt32(value);
        public ulong ValueToULong => Convert.ToUInt64(value);

        public float ValueToFloat => Convert.ToSingle(value, CultureInfo.InvariantCulture);
        public double ValueToDouble => Convert.ToDouble(value, CultureInfo.InvariantCulture);
        public decimal ValueToDecimal => Convert.ToDecimal(value, CultureInfo.InvariantCulture);

        public bool ValueToBool => Convert.ToBoolean(value);

        public ElementValue(object value)
            => this.value = value;

        public ElementValue(string value) : this((object)value) { }

        public ElementValue() : this((object)null) { }

        ~ElementValue()
            => Dispose(false);

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
            => $"Value:{value}";

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    value = default;
                disposedValue = true;
            }
        }
    }
}
