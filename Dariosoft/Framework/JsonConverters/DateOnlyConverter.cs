using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    var dateTime = new DateTime(reader.TryGetInt64(out var ticks) ? ticks : 0);
                    return new DateOnly(year: dateTime.Year, month: dateTime.Month, day: dateTime.Day);
                case JsonTokenType.String:
                    if (DateOnly.TryParse(reader.GetString(), out var value))
                        return value;

                    if (DateTime.TryParse(reader.GetString(), out dateTime))
                        return new DateOnly(year: dateTime.Year, month: dateTime.Month, day: dateTime.Day);

                    return DateOnly.MinValue;
                default:
                    return DateOnly.MinValue;
            }

        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}
