using Dariosoft.Framework.Types;

namespace Dariosoft.Framework
{
    public class Request
    {
        public required DateTimeOffset When { get; init; }

        public required string Where { get; init; }

        public required bool UserIsAnonymous { get; init; }

        public required string UserId { get; init; }

        public required string UserName { get; init; }

        public string? UserIP { get; init; }

        public string? UserAgent { get; init; }

        public ListQueryModel? ListQuery { get; init; }

        public Request<T> Transform<T>(T payload)
            => new Request<T>
            {
                When = this.When,
                Where = this.Where,
                UserIsAnonymous = this.UserIsAnonymous,
                UserId = this.UserId,
                UserName = this.UserName,
                UserIP = this.UserIP,
                UserAgent = this.UserAgent,
                Payload = payload
            };
    }
}
