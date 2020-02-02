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
    /// <summary>
    /// Provides a base class for fetching MOT test records.
    /// </summary>
    public class MotTestClient : IMotTestClient, IDisposable {
        private SecureString _key; // API key stored as a SecureString
        private HttpClient _client = new HttpClient(); // HttpClient to perform requests

        // Parameters
        private int? _page = null;
        private string _registration = null;
        private DateTime? _date = null;

        /// <summary>
        /// Gets the Uri to be accessed when fetching MOT test data.
        /// </summary>
        /// <value>The Uri to be accessed when fetching MOT test data. </value>
        public Uri Uri { get; }

        #if DEBUG
        internal MotTestClient(SecureString key, HttpMessageHandler handler) : this(key) {
            _client = new HttpClient(handler);
        }
        #endif

        /// <summary>
        /// Constructs a new MotTestClient with a specified API key.
        /// </summary>
        /// <param name="key">The API key.</param>
        public MotTestClient(SecureString key) : this(key, new Uri("https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests")) {}

        /// <summary>
        /// Constructs a new MotTestClient with a specified API key and request URI.
        /// </summary>
        /// <param name="key">The API key.</param>
        /// <param name="uri">The URI to use in place of the standard URI.</param>
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

        /// <summary>
        /// Provides a fluid interface for setting the page parameter in the request.
        /// </summary>
        /// <param name="page">The value to set the page parameter to.</param>
        /// <returns>The current IMotTestClient.</returns>
        public IMotTestClient Page(int page) {
            if(_registration != null)
                throw new InvalidParametersException("Registration searches cannot be paginated.");
            _page = page;
            return this;
        }

        /// <summary>
        /// Provides a fluid interface for setting the registration parameter in the request.
        /// </summary>
        /// <param name="registration">The value to set the registration parameter to.</param>
        /// <returns>The current IMotTestClient.</returns>
        public IMotTestClient Registration(string registration) {
            if(_page != null)
                throw new InvalidParametersException("Registration searches cannot be paginated.");
            if(_date != null)
                throw new InvalidParametersException("Registration searches cannot be dated.");
            _registration = registration?.Replace(" ", "") ?? throw new ArgumentNullException(nameof(registration));
            return this;
        }

        /// <summary>
        /// Proviees a fluid interface for setting the date parameter in the request.
        /// </summary>
        /// <param name="date">The value to set the date parameter to.</param>
        /// <remarks>Only the Date part of the date parameter shall be used in the request.</remarks>
        /// <returns>The current IMotTestClient.</returns>
        public IMotTestClient Date(DateTime date) {
            if(_registration != null)
                throw new InvalidParametersException("Registration searches cannot be dated.");
            _date = date;
            return this;
        }

        /// <summary>
        /// Provides a fluid interface for clearing all parameters in the request.
        /// </summary>
        /// <returns>The current IMotTestClient.</returns>
        public IMotTestClient Clear() {
            _page = null;
            _registration = null;
            _date = null;
            return this;
        }

        /// <summary>
        /// Fetches Vehicle records with the given parameters asynchronously
        /// </summary>
        /// <returns>An IAsyncEnumerable of Vehicle objects to be processed individually.</returns>
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
                            while(await json.ReadAsync()) {
                                if(json.TokenType == JsonToken.StartObject) {
                                    yield return serializer.Deserialize<T>(json);
                                }
                            }
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

        /// <summary>
        /// Disposes of the underlying HttpClient.
        /// </summary>
        /// <param name="disposing">Whether the call is from Dispose (TRUE) or the destructor (FALSE).</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _client.Dispose();
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Disposes of the underlying HttpClient.
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }
        #endregion

    }
}