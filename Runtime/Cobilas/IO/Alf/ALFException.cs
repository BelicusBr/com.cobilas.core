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

        /// <summary>
        /// <br>expcode:1000 = 'Get object disposed exception(arg0:<see cref="Type"/>).'</br>
        /// <br>expcode:1001 = 'Blank name.'</br>
        /// <br>expcode:1002 = 'Blank name(agr0:<see cref="CharacterCursor.LineEndColumn"/>).'</br>
        /// <br>expcode:1003 = 'Comment not finalized(agr0:<see cref="CharacterCursor.LineEndColumn"/>).'</br>
        /// <br>expcode:1004 = 'Header in second.'</br>
        /// <br>expcode:1005 = 'Symbol not identified(agr0:<see cref="CharacterCursor.LineEndColumn"/>, arg1:<see cref="char"/>).'</br>
        /// <br>expcode:1006 = 'Unfinished flag(agr0:<see cref="string"/> flag name).'</br>
        /// <br>expcode:1007 = 'Do not leave the root.'</br>
        /// <br>expcode:1008 = 'Unfinished name(agr0:<see cref="CharacterCursor.LineEndColumn"/>).'</br>
        /// <br>expcode:1009 = 'Invalid name(agr0:<see cref="string"/> flag name).'</br>
        /// <br>expcode:1010 = 'Invalid name(agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="string"/> flag name).'</br>
        /// <br>expcode:1011 = 'Invalid text.'</br>
        /// <br>expcode:1012 = 'Invalid text(agr0:<see cref="char"/>).'</br>
        /// <br>expcode:1013 = 'Invalid text(agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// <br>expcode:1014 = 'Unfinished text block(agr0:<see cref="CharacterCursor.LineEndColumn"/>).'</br>
        /// <br>expcode:1015 = 'Symbol not identified in text flag(agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// <br>expcode:1016 = 'Character in name is invalid(agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// </summary>
        /// <param name="expcode">Exception code.</param>
        public static ALFException GetALFException(uint expcode, params object[] args) {
            switch (expcode) {
                case 1000: return new ALFException(string.Format("The object {0} has already been discarded", args));
                case 1001: return new ALFException("The flag cannot have a blank name.");
                case 1002: return new ALFException(string.Format("{0} The flag cannot have a blank name.", args));
                case 1003: return new ALFException(string.Format("{0} Comment not finalized.", args));
                case 1004: return new ALFException("The header must start first.");
                case 1005: return new ALFException(string.Format("{0}'{1}' Symbol not identified.", args));
                case 1006: return new ALFException(string.Format("Flag '{0}' is not finalized!", args));
                case 1007: return new ALFException("Unable to exit root element.");
                case 1008: return new ALFException(string.Format("{0} Unfinished name.", args));
                case 1009: return new ALFException(string.Format("name '{0}' is invalid.\n" +
                "The name must only contain numerical, alphanumeric and special characters ('\\', '/', '_', '.').", args));
                case 1010: return new ALFException(string.Format("{0} name '{1}' is invalid.\n" +
                "The name must only contain numerical, alphanumeric and special characters ('\\', '/', '_', '.').", args));
                case 1011: return new ALFException("The text is invalid, use the escape character in the following characters " +
                "('\\\\', '\\:', '\\[', '\\ ]', '\\<', '\\>') or use the 'AddEscapeOnSpecialCharacters' property.");
                case 1012: return new ALFException(string.Format("The '{0}' character is invalid, use the escape character in the following characters " +
                "['\\{0}']('\\\\', '\\:', '\\[', '\\ ]', '\\<', '\\>') or use the 'AddEscapeOnSpecialCharacters' property.", args));
                case 1013: return new ALFException(string.Format("{0} The '{1}' character is invalid, use the escape character in the following characters " +
                "['\\{1}']('\\\\', '\\:', '\\[', '\\ ]', '\\<', '\\>') or use the 'AddEscapeOnSpecialCharacters' property.", args));
                case 1014: return new ALFException(string.Format("{0}Text block is not finalized!", args));
                case 1015: return new ALFException(string.Format("{0}'{1}' Unidentified symbol," +
                " use ':' to determine the beginning of the text.", args));
                case 1016: return new ALFException(string.Format("{0} character '{1}' is invalid.\n" +
                "The name must only contain numerical, alphanumeric and special characters ('\\', '/', '_', '.').", args));
                default: return new ALFException();
            }
        }
    }
}