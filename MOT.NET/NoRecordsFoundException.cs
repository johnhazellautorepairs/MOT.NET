using System;

namespace MOT.NET {
    /// <summary>
    /// An exception thrown when the API service finds no records for the specified parameters.
    /// </summary>
    public class NoRecordsFoundException : MOTNETException {
        /// <summary>
        /// Constructs a new NoRecordsFoundException.
        /// </summary>
        public NoRecordsFoundException() {
        }

        /// <summary>
        /// Constructs a new NoRecordsFoundException with a descriptive message.
        /// </summary>
        /// <param name="message">A message descibing the Exception.</param>
        public NoRecordsFoundException(string message) : base(message) {
        }

        /// <summary>
        /// Constructs a new NoRecordsFoundException with a descriptive message and inner exception.
        /// </summary>
        /// <param name="message">A message describing the Exception.</param>
        /// <param name="inner">The inner Exception.</param>
        /// <returns></returns>
        public NoRecordsFoundException(string message, Exception inner) : base(message, inner) {
        }
    }
}