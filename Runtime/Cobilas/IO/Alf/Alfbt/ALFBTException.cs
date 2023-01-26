using System;
using Cobilas.IO.Alf.Components;
using System.Runtime.Serialization;

namespace Cobilas.IO.Alf.Alfbt {
    [Serializable]
    public class ALFBTException : ALFException {
        public ALFBTException() { }
        public ALFBTException(string message) : base(message) { }
        public ALFBTException(string message, Exception inner) : base(message, inner) { }
        protected ALFBTException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        //The alfbt format was not identified
        public static ALFBTException UnknownVersion()
            => new ALFBTException("The alfbt format was not identified");

        public static ALFBTException FlagAlreadyExists(string name)
            => new ALFBTException($"Flag '{name}' already exists!");

        public static ALFBTException FlagAlreadyExists(CharacterCursor.LineEndColumn column, string name)
            => new ALFBTException(column.ToString(), FlagAlreadyExists(name));
    }
}
