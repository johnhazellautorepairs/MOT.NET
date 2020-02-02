using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using MOT.NET.Models;

namespace MOT.NET {
    public class MotTestClient : IMotTestClient, IDisposable {
        private SecureString _key;
        private HttpClient _client = new HttpClient();
        public Uri Uri { get; }
        private int? _page = null;
        private string _registration = null;
        private DateTime? _date = null;

        #if DEBUG
        internal MotTestClient(SecureString key, HttpMessageHandler handler) : this(key) {
            _client = new HttpClient(handler);
        }
        #endif

        public MotTestClient(SecureString key) : this(key, new Uri("https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests")) {}

        public MotTestClient(SecureString key, Uri uri) {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        private string Query {
            get {
                StringBuilder query = new StringBuilder();
                if(_page != null) query.Append($"&page={_page}");
                if(_registration != null) query.Append($"&registration={_registration}");
                if(_date != null) query.Append($"&date={_date.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}");
                return query.Length > 0 ? query.Remove(0, 1).ToString() : string.Empty;
            }
        }

        public IMotTestClient Page(int page) {
            if(_registration != null)
                throw new InvalidParametersException("Registration searches cannot be paginated.");
            _page = page;
            return this;
        }

        public IMotTestClient Registration(string registration) {
            if(_page != null)
                throw new InvalidParametersException("Registration searches cannot be paginated.");
            if(_date != null)
                throw new InvalidParametersException("Registration searches cannot be dated.");
            _registration = registration ?? throw new ArgumentNullException(nameof(registration));
            return this;
        }

        public IMotTestClient Date(DateTime date) {
            if(_registration != null)
                throw new InvalidParametersException("Registration searches cannot be dated.");
            _date = date;
            return this;
        }

        public IMotTestClient Clear() {
            _page = null;
            _registration = null;
            _date = null;
            return this;
        }

        public IAsyncEnumerable<Vehicle> FetchAsync() {
            if(_date != null && _page == null)
                throw new InvalidParametersException("Page must be set when searching by Date.");
            if(_date == null && _page == null && _registration == null)
                throw new InvalidParametersException("At least one parameter must be specified.");
            UriBuilder builder = new UriBuilder(Uri);
            builder.Query = Query;
            return GetManyJsonAsync<Vehicle>(builder.Uri);
        }

        internal async IAsyncEnumerable<T> GetManyJsonAsync<T>(Uri uri) {
            if(uri == null)
                throw new ArgumentNullException(nameof(uri));
            JsonSerializer serializer = new JsonSerializer();
            IntPtr ptr = Marshal.SecureStringToGlobalAllocUnicode(_key);
            string key = Marshal.PtrToStringUni(ptr);
            try {
                _client.DefaultRequestHeaders.Add("x-api-key", key);
                using(Stream response = await _client.GetStreamAsync(uri)) {
                    using(StreamReader reader = new StreamReader(response)) {
                        using(JsonReader json = new JsonTextReader(reader)) {
                            while(await json.ReadAsync())
                                if(json.TokenType == JsonToken.StartObject)
                                    yield return serializer.Deserialize<T>(json);
                        }
                    }
                }
                _client.DefaultRequestHeaders.Remove("x-api-key");
            } finally {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        #region IDisposable Support
        private bool disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _client.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion

    }
}