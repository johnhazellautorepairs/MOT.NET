using System;
using System.Security;

namespace MOT.NET {
    public class Core {
        private SecureString _key;
        public Uri Uri { get; }

        public Core(SecureString key) : this(key, new Uri("https://beta.check-mot.service.gov.uk/")) {}

        public Core(SecureString key, Uri uri) {
            _key = key;
            Uri = uri;
        }

        public IMOTRequestBuilder MOTs() {
            return new MOTRequestBuilder(Uri, _key);
        }

        public IMOTRequestBuilder MOTs(string path) {
            return new MOTRequestBuilder(Uri, _key, path: path);
        }
    }
}