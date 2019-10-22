using System.Collections.Generic;

namespace MOT.NET {
    public interface IFetchable<T> {
        IAsyncEnumerable<T> FetchAsync();
    }
}