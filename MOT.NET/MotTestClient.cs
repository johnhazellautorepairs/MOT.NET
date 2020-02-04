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
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace MOT.NET {
    /// <summary>
    /// Provides a base class for fetching MOT test records.
    /// </summary>
    public class MotTestClient : IMotTestClient, IDisposable {
        private static readonly Uri DEFAULT_URI = new Uri("https://beta.check-mot.service.gov.uk/trade/vehicles/mot-tests");
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
        /// Constructs a new MotTestClient with a specified API key, as a SecureString.
        /// </summary>
        /// <param name="key">The API key.</param>
        public MotTestClient(SecureString key) : this(key, DEFAULT_URI) {}

        /// <summary>
        /// Constructs a new MotTestClient with a specified API key, as a SecureString, and request URI.
        /// </summary>
        /// <param name="key">The API key.</param>
        /// <param name="uri">The URI to use in place of the default URI.</param>
        public MotTestClient(SecureString key, Uri uri) {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        /// <summary>
        /// Constructs a new MotTestClient with a specified API key.
        /// </summary>
        /// <param name="key">The API key.</param>
        public MotTestClient(string key) : this(key, DEFAULT_URI) {}

        /// <summary>
        /// Constructs a new MotTestClient with a specified API key, as a SecureString, and request URI.
        /// </summary>
        /// <param name="key">The API key.</param>
        /// <param name="uri">The URI to use in place of the default URI.</param>
        public MotTestClient(string key, Uri uri) {
            if(key == null)
                throw new ArgumentNullException(nameof(key));
            _key = new SecureString();
            foreach(var character in key) {
                _key.AppendChar(character);
            }
            _key.MakeReadOnly();
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        private HttpRequestMessage BuildRequestMessage(string key) {
            var ub = new UriBuilder(Uri);
            var sb = new StringBuilder();
            if(_page != null) sb.Append($"&page={_page}");
            if(_registration != null) sb.Append($"&registration={_registration}");
            if(_date != null) sb.Append($"&date={_date.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}");
            ub.Query = sb.Length > 0 ? sb.Remove(0, 1).ToString() : string.Empty;
            var message = new HttpRequestMessage(HttpMethod.Get, ub.Uri);
            message.Headers.Add("Accept", "application/json+v6");
            message.Headers.Add("x-api-key", key);
            return message;
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
        public async IAsyncEnumerable<Vehicle> FetchAsync() {
            if(_date != null && _page == null)
                throw new InvalidParametersException("Page must be set when searching by Date.");
            if(_date == null && _page == null && _registration == null)
                throw new InvalidParametersException("At least one parameter must be specified.");
            IntPtr ptr = Marshal.SecureStringToGlobalAllocUnicode(_key);
            string key = Marshal.PtrToStringUni(ptr);
            var message = BuildRequestMessage(key);
            try {
                var response = await _client.SendAsync(message);
                switch(response.StatusCode) {
                    case HttpStatusCode.NotFound:
                        throw new NoRecordsFoundException("No records were found with the specified parameters.");
                    case HttpStatusCode.Forbidden:
                        throw new InvalidApiKeyException("The specified API key was rejected.");
                    default:
                        response.EnsureSuccessStatusCode();
                        using(Stream stream = await response.Content.ReadAsStreamAsync()) {
                            await foreach(var vehicle in StreamJsonAsync<Vehicle>(stream)) {
                                yield return vehicle;
                            }
                        }
                        break;
                }
            } finally {
                message.Headers.Remove("x-api-key");
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        internal async IAsyncEnumerable<T> StreamJsonAsync<T>(Stream stream) {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));
            JsonSerializer serializer = new JsonSerializer();
            using StreamReader reader = new StreamReader(stream);
            using JsonReader json = new JsonTextReader(reader);
            while(await json.ReadAsync()) {
                if(json.TokenType == JsonToken.StartObject) {
                    yield return serializer.Deserialize<T>(json);
                }
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