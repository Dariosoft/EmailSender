using Dariosoft.Framework.Types;
using Microsoft.AspNetCore.Http;

namespace Dariosoft.EmailSender.EndPoint
{

    public static class HttpRequestExtensions
    {
        public static Framework.IRequest Transform(this HttpRequest request, ListQueryModel? listQuery = null)
        {
            return new Framework.Request
            {
                When = DateTimeOffset.UtcNow,
                Where = request.Path,
                UserIP = request.GetCurrentUserIP(),
                UserAgent = request.GetCurrentUserAgent(),
                ListQuery = listQuery,
                User = request.GetUserInfo()
            };
        }

        public static Framework.IRequest<T> Transform<T>(this HttpRequest request, T payload, ListQueryModel? listQuery = null)
        {
            return new Framework.Request<T>
            {
                When = DateTimeOffset.UtcNow,
                Where = request.Path,
                Payload = payload,
                UserIP = request.GetCurrentUserIP(),
                UserAgent = request.GetCurrentUserAgent(),
                ListQuery = listQuery,
                User = request.GetUserInfo()
            };
        }

        public static Framework.Auth.IUserIdentity GetUserInfo(this HttpRequest request)
            => Framework.Auth.UserIdentityFactory.Create(request.HttpContext.User);

        public static string? GetCurrentUserIP(this HttpRequest request)
            => request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

        public static string GetCurrentUserAgent(this HttpRequest request)
        {

            var val = request.Headers["User-Agent"];

            return val.ToString();
        }
    }
}
