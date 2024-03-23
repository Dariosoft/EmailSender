using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Dariosoft.Framework
{
    public class JSON : IJSON
    {
        private readonly JsonSerializerOptions _options;
        private static JSON _default = new JSON(new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic), });
        private static JSON _camelCase = new JSON(new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.Cyrillic), PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        private JSON(JsonSerializerOptions options)
        {
            _options = options;

            _options.Converters.Add(new JsonConverters.MailAddressJsonConverter());
            _options.Converters.Add(new JsonConverters.DateTimeConverter());
            _options.Converters.Add(new JsonConverters.DateOnlyConverter());
            _options.Converters.Add(new JsonConverters.TimeOnlyConverter());
            _options.Converters.Add(new JsonConverters.BooleanConvertor());
            _options.Converters.Add(new JsonConverters.GuidConvertor());
            _options.Converters.Add(new JsonConverters.NullableGuidConvertor());
            _options.Converters.Add(new JsonConverters.Int16Convertor());
            _options.Converters.Add(new JsonConverters.Int32Convertor());
            _options.Converters.Add(new JsonConverters.Int64Convertor());
        }

        /// <summary>
        /// Serialize to string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string? Serialize(object? input, Action<Exception>? onError = null)
        {
            if (Equals(input, null))
                return null;

            if (input is string)
                return (string)input;

            if (input.GetType().IsPrimitive)
                return input.ToString();

            try
            {
                return JsonSerializer.Serialize(input, _options);
            }
            catch (Exception err)
            {
                onError?.Invoke(err);
                return null;
            }
        }

        /// <summary>
        /// Serialize to buffer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[]? SerializeToBuffer(object? input, Action<Exception>? onError = null)
        {
            var json = Serialize(input, onError);
            return json is null ? null : Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// Deserialize from string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public T? Deserialize<T>(string? json, Action<Exception>? onError = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(json, _options);
            }
            catch (Exception err)
            {
                onError?.Invoke(err);
                return default;
            }
        }

        /// <summary>
        /// Deserialize from buffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public T? Deserialize<T>(byte[]? buffer, Action<Exception>? onError = null)
        {
            if (buffer == null || buffer.Length == 0)
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(buffer, _options);
            }
            catch (Exception err)
            {
                onError?.Invoke(err);
                return default;
            }
        }

        public object? Deserialize(string? json, Type returnType, Action<Exception>? onError = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                return JsonSerializer.Deserialize(json: json, returnType: returnType, options: _options);
            }
            catch (Exception err)
            {
                onError?.Invoke(err);
                return null;
            }
        }

        public object? Deserialize(byte[]? buffer, Type returnType, Action<Exception>? onError = null)
        {
            if (buffer == null || buffer.Length == 0)
                return default;

            try
            {
                return JsonSerializer.Deserialize(utf8Json: buffer, returnType: returnType, options: _options);
            }
            catch (Exception err)
            {
                onError?.Invoke(err);
                return null;
            }
        }

        /// <summary>
        /// Returns true if the input string is a valid json
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsJson(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            input = input.Trim();

            var isJson = input.StartsWith('{') && input.EndsWith('}') || input.StartsWith('[') && input.EndsWith(']');

            if (!isJson) return false;

            try
            {
                JsonDocument.Parse(input);
            }
            catch
            {
                isJson = false;
            }

            return isJson;
        }

        public readonly static IJSON Default = _default;

        public readonly static IJSON CamelCase = _camelCase;

        /// <summary>
        /// Adds a new converter to the JsonSerializerOptions
        /// </summary>
        /// <param name="converter"></param>
        public static bool AddConverter(JsonConverter converter)
        {
            if (converter is null)
                throw new ArgumentNullException(nameof(converter));

            int x = _default._options.Converters.Count, y = _camelCase._options.Converters.Count;

            _default._options.Converters.Add(converter);
            _camelCase._options.Converters.Add(converter);

            return _default._options.Converters.Count > x || _camelCase._options.Converters.Count > y;
        }

        /// <summary>
        /// Clears all converter from the JsonSerializerOptions
        /// </summary>
        public static void ClearConverters()
        {
            _default._options.Converters.Clear();
            _camelCase._options.Converters.Clear();
        }

        /// <summary>
        /// Removes a converter from the JsonSerializerOptions
        /// </summary>
        /// <param name="converter"></param>
        public static bool RemoveConverter(JsonConverter converter)
        {
            if (converter is null)
                throw new ArgumentNullException(nameof(converter));

            int x = _default._options.Converters.Count, y = _camelCase._options.Converters.Count;

            _default._options.Converters.Remove(converter);
            _camelCase._options.Converters.Remove(converter);

            return _default._options.Converters.Count < x || _camelCase._options.Converters.Count < y;
        }
    }
}
