using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dariosoft.Framework.JsonConverters
{
    public class MailAddressJsonConverter : JsonConverter<Types.MailAddress>
    {
        public override Types.MailAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return new Types.MailAddress(reader.GetString());
            }
            catch
            {
                return default;
            }
        }

        public override void Write(Utf8JsonWriter writer, Types.MailAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
