using System;
using System.Collections.Generic;

namespace MOT.NET {
    public interface IRequestBuilder {
        IRequestBuilder Pages(Range<int> range);

        IRequestBuilder Page(int page);

        IRequestBuilder Registration(string Registration);

        IRequestBuilder Date(DateTime date);
    }

    public interface IFetcher {
        IAsyncEnumerable<IRecord> FetchAsync();

        IEnumerable<IRecord> Fetch();
    }

    public class RequestBuilder : IRequestBuilder, IFetcher {
        private Uri _uri;
        private Range<int> _pages = null;
        private string _registration = null;
        private DateTime? _date = null;

        internal RequestBuilder(Uri uri, Range<int> pages = null, string registration = null, DateTime? date = null) {
            _uri = uri;
            _pages = pages;
            _registration = registration;
            _date = date;
        }

        public IRequestBuilder Pages(Range<int> range) {
            _pages = range;
            return this;
        }

        public IRequestBuilder Page(int page) {
            return Pages(new Range<int>(page));
        }

        public IRequestBuilder Registration(string registration) {
            _registration = registration;
            return this;
        }

        public IRequestBuilder Date(DateTime date) {
            _date = date;
            return this;
        }

        public async IAsyncEnumerable<IRecord> FetchAsync() {
            throw new NotImplementedException();
        }

        public IEnumerable<IRecord> Fetch() {
            throw new NotImplementedException();
        }
    }
}