using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class Int16Convertor : JsonConverter<short>
    {
        public override short Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            const short zero = 0, one = 1;

            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                case JsonTokenType.False: return zero;
                case JsonTokenType.True: return one;
                case JsonTokenType.Number: return reader.TryGetInt16(out var value) ? value : zero;
                case JsonTokenType.String: return short.TryParse(reader.GetString(), out value) ? value : zero;
                default: return one;
            }
        }

        public override void Write(Utf8JsonWriter writer, short value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
