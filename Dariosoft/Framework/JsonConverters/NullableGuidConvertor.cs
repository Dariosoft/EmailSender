using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class NullableGuidConvertor : JsonConverter<Guid?>
    {
        public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null) return null;
            return Guid.TryParse(reader.GetString(), out var value) ? value : null;
        }

        public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString().ToLower());
        }
    }
}
