using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class Int64Convertor : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                case JsonTokenType.False: return 0;
                case JsonTokenType.True: return 1;
                case JsonTokenType.Number: return reader.TryGetInt64(out var value) ? value : 0;
                case JsonTokenType.String: return long.TryParse(reader.GetString(), out value) ? value : 0;
                default: return 0;
            }
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
