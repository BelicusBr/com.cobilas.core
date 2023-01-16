namespace System.Xml {
    public class ElementAttribute : ElementBase {
        protected ElementValue value;

        public ElementValue Value { get => value; set => this.value = value; }
        public object ValueOBJ { get => value.Value; set => this.value.Value = value; }

        public ElementAttribute(string nome, ElementValue value) :
            base(nome) => this.value = value;

        public ElementAttribute(string nome, object value) : 
            this(nome, new ElementValue(value)) { }

        public ElementAttribute(string nome, string value) :
            this(nome, new ElementValue(value)) { }

        ~ElementAttribute()
            => Dispose(false);

        public override string ToString()
            => $"Name:{name}\n{value}";

        protected override void Dispose(bool disposing)
            => base.Dispose(disposing);

        protected override void Garbage() {
            name = (string)null;
            value?.Dispose();
        }
    }
}
