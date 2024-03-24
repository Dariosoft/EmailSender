namespace Dariosoft.Framework
{
    public class Request : IRequest
    {
        public required DateTimeOffset When { get; init; }

        public required string Where { get; init; }

        public required Auth.IUserIdentity User { get; init; }

        public string? UserIP { get; init; }

        public string? UserAgent { get; init; }

        public Types.ListQueryModel? ListQuery { get; init; }

        public IRequest<T> Transform<T>(T payload)
            => new Request<T>
            {
                When = this.When,
                Where = this.Where,
                User = this.User,
                UserIP = this.UserIP,
                UserAgent = this.UserAgent,
                Payload = payload
            };
    }
}
