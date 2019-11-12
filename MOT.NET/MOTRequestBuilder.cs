using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
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
                return query.Length > 0 ? query.Remove(0, 1).ToString() : string.Empty;
            }
        }

        internal MOTRequestBuilder(Core core, Uri uri, string path = "/trade/vehicles/mot-tests") {
            UriBuilder builder = new UriBuilder(uri ?? throw new ArgumentNullException(nameof(uri)));
            builder.Path = path ?? throw new ArgumentNullException(nameof(path));
            Core = core ?? throw new ArgumentNullException(nameof(core));
            _uri = builder.Uri;
        }

        public IMOTRequestBuilder Page(int page) {
            if(_registration != null)
                throw new InvalidParametersException("Registration searches cannot be paginated.");
            _page = page;
            return this;
        }

        public IMOTRequestBuilder Registration(string registration) {
            if(_page != null)
                throw new InvalidParametersException("Registration searches cannot be paginated.");
            if(_date != null)
                throw new InvalidParametersException("Registration searches cannot be dated.");
            _registration = registration ?? throw new ArgumentNullException(nameof(registration));
            return this;
        }

        public IMOTRequestBuilder Date(DateTime date) {
            if(_registration != null)
                throw new InvalidParametersException("Registration searches cannot be dated.");
            _date = date;
            return this;
        }

        public IAsyncEnumerable<Record> FetchAsync() {
            if(_date != null && _page == null)
                throw new InvalidParametersException("Page must be set when searching by Date.");
            UriBuilder builder = new UriBuilder(_uri);
            builder.Query = Query;
            return Core.GetManyJsonAsync<Record>(builder.Uri);
        }

    }
}