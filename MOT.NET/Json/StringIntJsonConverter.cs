using System;
using Newtonsoft.Json;

namespace MOT.NET.Json {
    internal class StringIntJsonConverter : JsonConverter<int> {
        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer) {
            return int.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer) {
            writer.WriteValue(value.ToString());
        }
    }
}