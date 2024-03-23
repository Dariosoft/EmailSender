using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class BooleanConvertor : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                case JsonTokenType.False: return false;
                case JsonTokenType.True: return true;
                case JsonTokenType.Number: return reader.TryGetInt32(out var value) ? value != 0 : false;
                case JsonTokenType.String: return bool.TryParse(reader.GetString()?.ToString() ?? "false", out var bvalue) ? bvalue : false;
                default: return false;
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
