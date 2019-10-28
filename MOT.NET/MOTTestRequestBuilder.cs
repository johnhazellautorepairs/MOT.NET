using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using System.Security;
using System.Runtime.InteropServices;
using MOT.NET.Models.MOT;

namespace MOT.NET {
    public interface IMOTRequestBuilder : IFetchable<Record> {
        IMOTRequestBuilder Page(int page);

        IMOTRequestBuilder Registration(string Registration);

        IMOTRequestBuilder Date(DateTime date);
    }

    public class MOTRequestBuilder : IMOTRequestBuilder {
        public Core Core { get; }
        private Uri _uri;
        private int? _page = null;
        private string _registration = null;
        private DateTime? _date = null;

        private string Query {
            get {
                StringBuilder query = new StringBuilder();
                if(_page != null) query.Append($"&page={_page}");
                if(_registration != null) query.Append($"&registration={_registration}");
                if(_date != null) query.Append($"&date={_date.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}");
                return query.Remove(0, 1).ToString();
            }
        }

        internal MOTRequestBuilder(Core core, Uri uri, string path = "/trade/vehicles/mot-tests") {
            UriBuilder builder = new UriBuilder(uri);
            builder.Path = path;
            Core = core;
            _uri = builder.Uri;
        }

        public IMOTRequestBuilder Page(int page) {
            _page = page;
            return this;
        }

        public IMOTRequestBuilder Registration(string registration) {
            _registration = registration;
            return this;
        }

        public IMOTRequestBuilder Date(DateTime date) {
            _date = date;
            return this;
        }

        public IAsyncEnumerable<Record> FetchAsync() {
            UriBuilder builder = new UriBuilder(_uri);
            builder.Query = Query;
            return Core.GetManyJsonAsync<Record>(builder.Uri);
        }

    }
}