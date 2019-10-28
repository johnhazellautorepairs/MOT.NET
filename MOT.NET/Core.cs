using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using Newtonsoft.Json;

namespace MOT.NET {
    public class Core : IDisposable {
        private SecureString _key;
        private HttpClient _client = new HttpClient();
        public Uri Uri { get; }

        public Core(SecureString key) : this(key, new Uri("https://beta.check-mot.service.gov.uk/")) {}

        public Core(SecureString key, Uri uri) {
            _key = key;
            Uri = uri;
        }

        public IMOTRequestBuilder MOTs() {
            return new MOTRequestBuilder(this, Uri);
        }

        public IMOTRequestBuilder MOTs(string path) {
            return new MOTRequestBuilder(this, Uri, path: path);
        }

        internal async IAsyncEnumerable<T> GetManyJsonAsync<T>(Uri uri) {
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