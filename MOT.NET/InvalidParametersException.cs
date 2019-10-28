using System;
using System.Runtime.Serialization;

namespace MOT.NET {
    public class InvalidParametersException : MOTNETException
    {
        public InvalidParametersException() {
        }

        public InvalidParametersException(string message) : base(message) {
        }

        public InvalidParametersException(string message, Exception inner) : base(message, inner) {
        }
    }
}