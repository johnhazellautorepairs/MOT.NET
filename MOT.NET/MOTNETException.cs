using System;
using System.Runtime.Serialization;

namespace MOT.NET {
    public class MOTNETException : Exception {
        public MOTNETException() {}

        public MOTNETException(string message) : base(message) {}

        public MOTNETException(string message, Exception inner) : base(message, inner) {}

        protected MOTNETException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}