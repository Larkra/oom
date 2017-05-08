using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task2.Utilities
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            return _epoch.AddSeconds((long)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //Not necessary for this example...
            throw new NotImplementedException();
        }

    }
}
