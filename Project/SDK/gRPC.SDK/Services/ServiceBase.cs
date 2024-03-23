using Grpc.Core;
using Grpc.Net.Client;

namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK.Services
{
    abstract class ServiceBase<TClient>
         where TClient : ClientBase<TClient>
    {
        protected readonly TClient _client;
        protected readonly IConnectionInfo _connectionInfo;

        protected ServiceBase(IConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
            _client = CreateClient(ProvideChannel());
        }

        protected abstract TClient CreateClient(GrpcChannel channel);

        private GrpcChannel ProvideChannel()
        {
            var credentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                metadata.Add("Authorization", $"Bearer {_connectionInfo.ClientId.ToString().ToLower()}@{_connectionInfo.ApiKey}");
            });

            var channel = GrpcChannel.ForAddress(_connectionInfo.EndPointAddress, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(ChannelCredentials.SecureSsl, credentials)
            });

            return channel;
        }
    }
}
