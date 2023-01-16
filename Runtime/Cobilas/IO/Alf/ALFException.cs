using System;
using System.Runtime.Serialization;

namespace Cobilas.IO.Alf {
    [Serializable]
    public class ALFException : Exception {
        public ALFException() { }
        public ALFException(string message) : base(message) { }
        public ALFException(string message, Exception inner) : base(message, inner) { }
        protected ALFException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}