using System;
using System.Security;

namespace MOT.NET {
    public class Core : IRequestBuilder {
        private SecureString _key;
        public Uri Uri { get; }

        public Core(SecureString key) : this(key, new Uri("https://beta.check-mot.service.gov.uk/")) {}

        public Core(SecureString key, Uri uri) {
            _key = key;
            Uri = uri;
        }

        public IRequestBuilder Pages(Range<int> range) {
            return new RequestBuilder(Uri, _key, pages: range);
        }

        public IRequestBuilder Page(int page) {
            return Pages(new Range<int>(page));
        }

        public IRequestBuilder Registration(string registration) {
            return new RequestBuilder(Uri, _key, registration: registration);
        }

        public IRequestBuilder Date(DateTime date) {
            return new RequestBuilder(Uri, _key, date: date);
        }
    }
}