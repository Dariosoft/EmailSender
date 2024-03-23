using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Dariosoft.Framework
{
    public interface IJSON
    {
        string? Serialize(object? input, Action<Exception>? onError = null);

        byte[]? SerializeToBuffer(object? input, Action<Exception>? onError = null);

        T? Deserialize<T>(string? json, Action<Exception>? onError = null);

        T? Deserialize<T>(byte[]? buffer, Action<Exception>? onError = null);

        object? Deserialize(string? json, Type returnType, Action<Exception>? onError = null);

        object? Deserialize(byte[]? buffer, Type returnType, Action<Exception>? onError = null);
    }
}
