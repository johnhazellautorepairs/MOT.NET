using System;

namespace MOT.NET {
    /// <summary>
    /// An exception thrown when an invalid set of parameters has been specified.
    /// </summary>
    public class InvalidParametersException : MOTNETException
    {
        /// <summary>
        /// Constructs a new InvalidParametersException.
        /// </summary>
        public InvalidParametersException() {
        }

        /// <summary>
        /// Constructs a new InvalidParametersException with a descriptive message.
        /// </summary>
        /// <param name="message">A message descibing the Exception.</param>
        public InvalidParametersException(string message) : base(message) {
        }

        /// <summary>
        /// Constructs a new InvalidParametersException with a descriptive message and inner exception.
        /// </summary>
        /// <param name="message">A message describing the Exception.</param>
        /// <param name="inner">The inner Exception.</param>
        public InvalidParametersException(string message, Exception inner) : base(message, inner) {
        }
    }
}