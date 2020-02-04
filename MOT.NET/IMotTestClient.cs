using System;
using MOT.NET.Models;

namespace MOT.NET {
    /// <summary>
    /// A client which can request MOT tests.
    /// </summary>
    public interface IMotTestClient : IFetchable<Vehicle> {
        /// <summary>
        /// Provides a fluid interface for setting the page parameter in the request.
        /// </summary>
        /// <param name="page">The value to set the page parameter to.</param>
        /// <returns>The current IMotTestClient.</returns>
        IMotTestClient Page(int page);

        /// <summary>
        /// Provides a fluid interface for setting the registration parameter in the request.
        /// </summary>
        /// <param name="registration">The value to set the registration parameter to.</param>
        /// <returns>The current IMotTestClient.</returns>
        IMotTestClient Registration(string registration);

        /// <summary>
        /// Proviees a fluid interface for setting the date parameter in the request.
        /// </summary>
        /// <param name="date">The value to set the date parameter to.</param>
        /// <remarks>Only the Date part of the date parameter shall be used in the request.</remarks>
        /// <returns>The current IMotTestClient.</returns>
        IMotTestClient Date(DateTime date);

        /// <summary>
        /// Provides a fluid interface for clearing all parameters in the request.
        /// </summary>
        /// <returns>The current IMotTestClient.</returns>
        IMotTestClient Clear();
    }
}