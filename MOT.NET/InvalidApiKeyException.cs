using System;

namespace MOT.NET {
    /// <summary>
    /// An exception thrown when the API service denies access.
    /// </summary>
    public class InvalidApiKeyException : MOTNETException {
        /// <summary>
        /// Constructs a new InvalidApiKeyException.
        /// </summary>
        public InvalidApiKeyException() {
        }

        /// <summary>
        /// Constructs a new InvalidApiKeyException with a descriptive message.
        /// </summary>
        /// <param name="message">A message descibing the Exception.</param>
        public InvalidApiKeyException(string message) : base(message) {
        }

        /// <summary>
        /// Constructs a new InvalidApiKeyException with a descriptive message and inner exception.
        /// </summary>
        /// <param name="message">A message describing the Exception.</param>
        /// <param name="inner">The inner Exception.</param>
        /// <returns></returns>
        public InvalidApiKeyException(string message, Exception inner) : base(message, inner) {
        }
    }
}