using System;
using System.Text;
using Cobilas.Collections;
using Cobilas.IO.Alf.Components;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cobilas.IO.Alf {
    [Serializable]
    public class ALFException : Exception {

        public ALFException() { }
        public ALFException(string message) : base(message) { }
        public ALFException(string message, Exception inner) : base(message, inner) { }
        protected ALFException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static ALFException SymbolNotIdentified(CharacterCursor.LineEndColumn column, char c)
            => new ALFException($"{column}'{c.EscapeSequenceToString()}' Symbol not identified.");

        public static ALFException SymbolNotIdentifiedInTextFlag(CharacterCursor.LineEndColumn column, char c)
            => new ALFException($"{column}'{c.EscapeSequenceToString()}' Unidentified symbol," +
                $" use ':' to determine the beginning of the text.");

        public static ALFException BlankName()
            => new ALFException("The flag cannot have a blank name.");

        public static ALFException BlankName(CharacterCursor.LineEndColumn column)
            => new ALFException(column.ToString(), BlankName());

        public static ALFException UnfinishedName(CharacterCursor.LineEndColumn column)
            => new ALFException($"{column} Unfinished name.");

        public static ALFException CommentNotFinalized(CharacterCursor.LineEndColumn column)
            => new ALFException($"{column} Comment not finalized.");

        public static ALFException InvalidName(string name)
            => new ALFException(string.Format(
                "name '{0}' is invalid.\n" +
                "The name must only contain numerical, alphanumeric and special characters ('\\', '/', '_', '.').", name));

        public static ALFException InvalidName(CharacterCursor.LineEndColumn column, string name)
            => new ALFException(column.ToString(), InvalidName(name));

        public static ALFException DoNotLeaveTheRoot()
            => new ALFException("Unable to exit root element");

        public static ALFException HeaderInSecond()
            => new ALFException("The header must start first.");

        public static ALFException InvalidText(char c)
            => new ALFException($"The '{c}' character is invalid, use the escape character in the following characters " +
                $"['\\{c}']('\\\\', '\\:', '\\[', '\\ ]', '\\<', '\\>') or use the 'AddEscapeOnSpecialCharacters' property.");

        public static ALFException InvalidText(CharacterCursor.LineEndColumn column, char c)
            => new ALFException(column.ToString(), InvalidText(c));

        public static ALFException UnfinishedTextBlock(CharacterCursor.LineEndColumn column)
            => new ALFException($"{column}Text block is not finalized!");

        public static ALFException UnfinishedFlag(string flagName)
            => new ALFException($"Flag '{flagName}' is not finalized!");

        public static ObjectDisposedException GetObjectDisposedException<T>()
            => GetObjectDisposedException(typeof(T));

        public static ObjectDisposedException GetObjectDisposedException(Type type)
            => new ObjectDisposedException($"The object {type} has already been discarded");
    }
}