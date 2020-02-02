using System;
using System.Runtime.Serialization;

namespace MOT.NET {
    /// <summary>
    /// A base exception for all exceptions occurring in the MOT.NET library.
    /// </summary>
    public class MOTNETException : Exception {
        /// <summary>
        /// Initializes a new instance of the MOTNETException class.
        /// </summary>
        public MOTNETException() {}

        /// <summary>
        /// Initializes a new instance of the MOTNETException class with a specified error message.
        /// </summary>
        /// <param name="message">The specified error message.</param>
        public MOTNETException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The specified error message.</param>
        /// <param name="inner">The inner exception that is the cause of this exception.</param>
        public MOTNETException(string message, Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initializes a new instance of the MOTNETException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected MOTNETException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}