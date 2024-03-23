using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return new TimeOnly(reader.TryGetInt64(out var ticks) ? ticks : 0);
                case JsonTokenType.String:
                    if (TimeOnly.TryParse(reader.GetString(), out var value))
                        return value;

                    if (DateTime.TryParse(reader.GetString(), out var dateTime))
                        return new TimeOnly(hour: dateTime.Hour, minute: dateTime.Minute, second: dateTime.Second, millisecond: dateTime.Millisecond);

                    return TimeOnly.MinValue;
                default:
                    return TimeOnly.MinValue;
            }

        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("HH:mm:ss.fff"));
        }
    }
}
