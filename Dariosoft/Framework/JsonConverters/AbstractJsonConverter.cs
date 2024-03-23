using System.Text.Json;

namespace Dariosoft.Framework.JsonConverters
{
    public class AbstractJsonConverter<TInterface, TImplementation> : System.Text.Json.Serialization.JsonConverter<TInterface>
        where TImplementation : TInterface, new()
    {

        private Type TTInterface = typeof(TInterface);
        private Type TTImplementation = typeof(TImplementation);

        public AbstractJsonConverter()
        {
            if (TTInterface == TTImplementation)
                throw new ApplicationException("TInterface and TImplementation should not be same.");
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == TTInterface && typeToConvert != TTImplementation && TTInterface.IsAssignableFrom(typeToConvert);
        }

        public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            return JsonSerializer.Deserialize<TImplementation>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteRawValue(JsonSerializer.Serialize(value, options));
            else
                writer.WriteRawValue(JsonSerializer.Serialize(value, value.GetType(), options));
        }
    }
}
