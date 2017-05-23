using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task4.Utilities
{
    public class ReleaseDateDateTimeConverter : DateTimeConverterBase
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            return reader.ValueType == typeof(DateTime) ? serializer.Deserialize(reader, objectType) : _epoch.AddSeconds((long)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
