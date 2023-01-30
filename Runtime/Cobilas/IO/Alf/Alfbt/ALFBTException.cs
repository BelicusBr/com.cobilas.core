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
        
        /// <summary>
        /// <br>expcode:1100 = 'Unknown version.'</br>
        /// <br>expcode:1101 = 'Flag already exists(arg0:<see cref="CharacterCursor.LineEndColumn"/>, agr1:<see cref="string"/> flag name).'</br>
        /// <br>expcode:1102 = 'Flag already exists(agr0:<see cref="string"/> flag name).'</br>
        /// <br>expcode:1103 = 'Invalid text[alfbt 1.0](agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// <br>expcode:1104 = 'Invalid header text(agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// <br>expcode:1105 = 'Invalid markup flag text(agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// <br>expcode:1106 = 'Invalid text[alfbt 1.5](agr0:<see cref="CharacterCursor.LineEndColumn"/> agr1:<see cref="char"/>).'</br>
        /// <br>expcode:1107 = 'Invalid text[alfbt 1.5](agr0:<see cref="string"/> flag name).'</br>
        /// </summary>
        /// <param name="expcode">Exception code.</param>
        public static ALFBTException GetALFBTException(uint expcode, params object[] args) {
            switch (expcode) {
                case 1100: return new ALFBTException("The alfbt format was not identified");
                case 1101: return new ALFBTException(string.Format("{0} Flag '{1}' already exists!", args));
                case 1102: return new ALFBTException(string.Format("Flag '{0}' already exists!", args));
                case 1103: return new ALFBTException(string.Format("{0} The '{1}' character is invalid, use the escape character in the following characters " +
                "['\\{1}']('\\\\', '\\)', '\\@') or use the 'AddEscapeOnSpecialCharacters' property.", args));
                case 1104: return new ALFBTException(string.Format("{0}The '{1}' character in the header text is invalid", args));
                case 1105: return new ALFBTException(string.Format("{0}The '{1}' character in the markup flag text is invalid", args));
                case 1106: return new ALFBTException(string.Format("{0} The '{1}' character is invalid, use the escape character in the following characters " +
                "['\\{1}']('\\\\', '\\/', '\\*') or use the 'AddEscapeOnSpecialCharacters' property.", args));
                case 1107: return new ALFBTException(string.Format("Flag '{0}', use the escape character in the following characters " +
                "['\\{1}']('\\\\', '\\/', '\\*') or use the 'AddEscapeOnSpecialCharacters' property.", args));
                default: return new ALFBTException();
            }
        }
    }
}
