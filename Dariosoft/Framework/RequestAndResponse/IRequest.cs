namespace Dariosoft.Framework
{
    public interface IRequest
    {
        DateTimeOffset When { get; }

        string Where { get; }

        Auth.IUserIdentity User { get; }

        string? UserIP { get; }

        string? UserAgent { get; }

        Types.ListQueryModel? ListQuery { get; init; }

        IRequest<T> Transform<T>(T payload);
    }
}
