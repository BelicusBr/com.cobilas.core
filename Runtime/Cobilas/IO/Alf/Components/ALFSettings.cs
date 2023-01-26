using System;
using System.Text;

namespace Cobilas.IO.Alf.Components {
    public abstract class ALFSettings : IDisposable {
        public abstract Encoding Encoding { get; }

        public abstract void Close();
        public abstract void Flush();
        public abstract void Dispose();
        public abstract void Set(MarshalByRefObject obj, Encoding encoding);
        public abstract TypeStream GetStrem<TypeStream>() where TypeStream : MarshalByRefObject;

        protected abstract bool IsStream();
    }
}
