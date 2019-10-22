using System;

namespace MOT.NET {
    /// <summary>
    /// Represents a Range of values of type T.
    /// </summary>
    /// <typeparam name="T">The type the Range represents.</typeparam>
    public interface IRange<T> where T : IComparable {
        /// <summary>
        /// Gets the minimum value of the Range<T>.
        /// </summary>
        /// <value>The minimum value of the range.</value>
        T Minimum { get; }

        /// <summary>
        /// Gets the maximum value of the Range<T>.
        /// </summary>
        /// <value>The maximum value of the range.</value>
        T Maximum { get; }

        /// <summary>
        /// Determines whether the value is in the current Range<T>.
        /// </summary>
        /// <param name="value">The value to determine if it is in the current Range<T>.</param>
        /// <returns>TURE if the value is in the current Range<T>, FALSE otherwise.</returns>
        bool Contains(T value);
        
        /// <summary>
        /// Determines whether the Range<T> contains the specified Range<T>.
        /// </summary>
        /// <param name="range">The Range<T> to determine if it is within the current Range<T>.</param>
        /// <returns>TRUE if the specified Range<T> is within the current Range<T>, FALSE otherwise.</returns>
        bool Contains(Range<T> range);

        /// <summary>
        /// Returns a string that Represents the current Range<T>.
        /// </summary>
        /// <returns>A string representation of the current Range<T>.</returns>
        string ToString();

        /// <summary>
        /// Determines whether the Range<T> is within the specified Range<T>.
        /// </summary>
        /// <param name="range">The Range<T> to determine if the current Range<T> is within.</param>
        /// <returns>TRUE if the current Range<T> is within the specified Range<T>, FALSE otherwise.</returns>
        bool Within(Range<T> range);
    }

    /// <summary>
    /// Represents a Range of values of type T.
    /// </summary>
    /// <typeparam name="T">The type the Range represents.</typeparam>
    public class Range<T> : IRange<T> where T : IComparable {
        /// <summary>
        /// Gets the minimum value of the Range<T>.
        /// </summary>
        /// <value>The minimum value of the range.</value>
        public T Minimum { get; }

        /// <summary>
        /// Gets the maximum value of the Range<T>.
        /// </summary>
        /// <value>The maximum value of the range.</value>
        public T Maximum { get; }

        /// <summary>
        /// Initialses a Range<T> with a minimum and maximum value.
        /// </summary>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        public Range(T min, T max)
        {
            if (min.CompareTo(max) > 0)
                throw new InvalidRangeException($"`{nameof(max)}` must be larger than or equal to `{nameof(min)}`.");
            Minimum = min;
            Maximum = max;
        }

        /// <summary>
        /// Initialises a Range<T> with a single value to assign to both the minimum and maximum values.
        /// </summary>
        /// <param name="single">The single value to use for both minimum and maximum.</param>
        public Range(T single) : this(single, single) { }

        /// <summary>
        /// Determines whether the value is in the current Range<T>.
        /// </summary>
        /// <param name="value">The value to determine if it is in the current Range<T>.</param>
        /// <returns>TURE if the value is in the current Range<T>, FALSE otherwise.</returns>
        public bool Contains(T value)
        {
            return (Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
        }

        /// <summary>
        /// Determines whether the Range<T> is within the specified Range<T>.
        /// </summary>
        /// <param name="range">The Range<T> to determine if the current Range<T> is within.</param>
        /// <returns>TRUE if the current Range<T> is within the specified Range<T>, FALSE otherwise.</returns>
        public bool Within(Range<T> range)
        {
            return range.Contains(Minimum) && range.Contains(Maximum);
        }

        /// <summary>
        /// Determines whether the Range<T> contains the specified Range<T>.
        /// </summary>
        /// <param name="range">The Range<T> to determine if it is within the current Range<T>.</param>
        /// <returns>TRUE if the specified Range<T> is within the current Range<T>, FALSE otherwise.</returns>
        public bool Contains(Range<T> range)
        {
            return range.Within(this);
        }

        /// <summary>
        /// Returns a string that Represents the current Range<T>.
        /// </summary>
        /// <returns>A string representation of the current Range<T>.</returns>
        public override string ToString()
        {
            return $"[{Minimum}-{Maximum}]";
        }
    }

    /// <summary>
    /// The exception that is thrown when a Range<T> is initialised with invalid 
    /// </summary>
    public class InvalidRangeException : Exception {
        /// <summary>
        /// Initialises an InvalidRangeException.
        /// </summary>
        public InvalidRangeException() : base() {}
        /// <summary>
        /// Initialises an InvalidRangeException with a message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidRangeException(string message) : base(message) {}
        /// <summary>
        /// Initialises an InvalidRangeException with 
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The exception that caused the exception.</param>
        public InvalidRangeException(string message, Exception inner) : base(message, inner) {}
    }
}