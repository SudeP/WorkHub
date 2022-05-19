using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Properties.ASPNET
{
    public class HelperDateTime
    {
        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToUniversalTime().ToLocalTime().ToString("dd-MM-yyyyTHH:mm:ss"));
            }
        }
    }
}
