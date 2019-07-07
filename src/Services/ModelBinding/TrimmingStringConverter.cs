using System;
using Newtonsoft.Json;

namespace Firepuma.Api.Common.Services.ModelBinding
{
    public class TrimmingStringConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string value)
            {
                return value.Trim();
            }

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
