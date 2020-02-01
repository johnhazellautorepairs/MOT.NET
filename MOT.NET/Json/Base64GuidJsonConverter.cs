using System;
using Newtonsoft.Json;

namespace MOT.NET.Json {
    internal class Base64GuidJsonConverter : JsonConverter<Guid> {
        public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer) {
            string base64 = reader.Value.ToString().Replace('_', '/').Replace('-', '+');
            byte[] raw = Convert.FromBase64String(base64);
            Guid guid = new Guid(raw);
            return guid;
        }

        public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer) {
            byte[] raw = value.ToByteArray();
            string base64 = Convert.ToBase64String(raw);
            string transposed = base64.Replace('/', '_').Replace('+', '-');
            writer.WriteValue(transposed);
        }
    }
}