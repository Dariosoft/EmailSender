using Dariosoft.Framework.Types;
using Microsoft.AspNetCore.Http;

namespace Dariosoft.EmailSender.EndPoint
{

    public static class HttpRequestExtensions
    {
        public static Framework.Request Transform(this HttpRequest request, ListQueryModel? listQuery = null)
        {
            return new Framework.Request
            {
                When = DateTimeOffset.UtcNow,
                Where = request.Path,
                UserIsAnonymous = !request.IsUserAuthenticated(),
                UserId = request.GetCurrentUserId(),
                UserName = request.GetCurrentUserName(),
                UserIP = request.GetCurrentUserIP(),
                UserAgent = request.GetCurrentUserAgent(),
                ListQuery = listQuery
            };
        }

        public static Framework.Request<T> Transform<T>(this HttpRequest request, T payload, ListQueryModel? listQuery = null)
        {
            return new Framework.Request<T>
            {
                When = DateTimeOffset.UtcNow,
                Where = request.Path,
                Payload = payload,
                UserIsAnonymous = !request.IsUserAuthenticated(),
                UserId = request.GetCurrentUserId(),
                UserName = request.GetCurrentUserName(),
                UserIP = request.GetCurrentUserIP(),
                UserAgent = request.GetCurrentUserAgent(),
                ListQuery = listQuery
            };
        }

        public static bool IsUserAuthenticated(this HttpRequest request)
            => request.HttpContext.User?.Identity?.IsAuthenticated ?? false;

        public static string GetCurrentUserId(this HttpRequest request)
            => request.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Sid)?.Value ?? "";

        public static string GetCurrentUserName(this HttpRequest request)
            => request.HttpContext.User?.Identity?.Name ?? "Anonymous";

        public static string? GetCurrentUserIP(this HttpRequest request)
            => request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

        public static string GetCurrentUserAgent(this HttpRequest request)
        {

            var val = request.Headers["User-Agent"];

            return val.ToString();
        }
    }
}
