using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using System.Security;
using System.Runtime.InteropServices;

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
        private SecureString _key;
        private Range<int> _pages = null;
        private string _registration = null;
        private DateTime? _date = null;

        private string QueryString {
            get {
                StringBuilder query = new StringBuilder();
                if(_pages != null) query.Append($"&pages={_pages.ToString()}");
                if(_registration != null) query.Append($"&registration={_registration}");
                if(_date != null) query.Append($"&date={_date.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}");
                return query.Remove(0, 1).ToString();
            }
        }

        internal RequestBuilder(Uri uri, SecureString key, Range<int> pages = null, string registration = null, DateTime? date = null) {
            _uri = uri;
            _key = key;
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

        private Uri Build() {
            UriBuilder builder = new UriBuilder(_uri);
            builder.Query = QueryString;
            return builder.Uri;
        }

        public async IAsyncEnumerable<IRecord> FetchAsync() {
            IntPtr ptr = Marshal.SecureStringToBSTR(_key);
            string key = Marshal.PtrToStringBSTR(ptr);
            JsonSerializer serializer = new JsonSerializer();
            try {
                using(HttpClient client = new HttpClient()) {
                    client.DefaultRequestHeaders.Add("x-api-key", key);
                    using(Stream response = await client.GetStreamAsync(Build()))
                        using(StreamReader reader = new StreamReader(response))
                            using(JsonReader json = new JsonTextReader(reader))
                                while(await json.ReadAsync())
                                    if(json.TokenType == JsonToken.StartObject)
                                        yield return serializer.Deserialize<Record>(json);
                }
            } finally {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        public IEnumerable<IRecord> Fetch() {
            throw new NotImplementedException();
        }
    }
}