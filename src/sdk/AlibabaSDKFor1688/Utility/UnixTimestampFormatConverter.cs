using Newtonsoft.Json;
using System;

namespace AlibabaSDK.Utility
{
    public class UnixTimestampFormatConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTimeOffset);
        }
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var s = Convert.ToDouble(reader.Value);
            var data = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(s);
            return objectType == typeof(DateTimeOffset) ? new DateTimeOffset(data) : data;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
