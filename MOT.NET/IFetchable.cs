using System.Collections.Generic;

namespace MOT.NET {
    /// <summary>
    /// Provides a mechanism for fetching multiple objects asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of object to fetch.</typeparam>
    public interface IFetchable<T> {
        /// <summary>
        /// Fetches multiple objects asynchronously.
        /// </summary>
        /// <returns>An IAsyncEnumerable to access and process the returned objects individually.</returns>
        IAsyncEnumerable<T> FetchAsync();
    }
}