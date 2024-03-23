using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return new DateTime(reader.TryGetInt64(out var ticks) ? ticks : 0);
                case JsonTokenType.String:
                    return DateTime.TryParse(reader.GetString(), out var value) ? value : DateTime.MinValue;
                default:
                    return DateTime.MinValue;
            }

        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}
