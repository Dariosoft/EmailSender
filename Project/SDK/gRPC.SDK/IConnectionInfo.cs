namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK
{
    public interface IConnectionInfo
    {
        string EndPointAddress { get; }

        Guid ClientId { get; }

        string ApiKey { get; }
    }
}
