namespace System.Xml {
    public class ElementBase : IDisposable {
        protected bool disposedValue;
        protected string name;

        public string Name { get => name; set => name = value; }

        public ElementBase(string name)
            => this.name = name;

        ~ElementBase()
            => Dispose(false);

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool CompareType(Type type)
            => GetType() == type;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    Garbage();
                disposedValue = true;
            }
        }

        protected virtual void Garbage() { }
    }
}
