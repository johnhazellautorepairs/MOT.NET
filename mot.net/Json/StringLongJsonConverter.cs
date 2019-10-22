using System;
using Newtonsoft.Json;

namespace MOT.NET.Json {
    public class StringLongJsonConverter : JsonConverter<long>
    {
        public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return long.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}