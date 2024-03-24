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
            if (!Uri.TryCreate(_connectionInfo.EndPointAddress, UriKind.Absolute, out var uri))
                throw new System.Net.ProtocolViolationException("Invalid end point address.");

            var scheme = uri.Scheme.ToLower();

            if (!(scheme == "http" || scheme == "https"))
                throw new System.Net.ProtocolViolationException("Invalid end point address.");

            var isInSecure = uri!.Scheme.ToLower() == "http";

            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                metadata.Add("Authorization", $"Bearer {_connectionInfo.ClientId.ToString().ToLower()}@{_connectionInfo.ApiKey}");
                return Task.CompletedTask;
            });

            var channel = GrpcChannel.ForAddress(uri, new GrpcChannelOptions
            {
                UnsafeUseInsecureChannelCallCredentials = isInSecure,
                Credentials = ChannelCredentials.Create(isInSecure ? ChannelCredentials.Insecure : ChannelCredentials.SecureSsl, credentials)
            });

            return channel;
        }

        #region Grpc Failed
        protected Abstraction.Models.Result GrpcFailed(RpcException exception)
            => Abstraction.Models.Result.Fail(exception.Message, code: Enum.GetName(exception.StatusCode));

        protected Abstraction.Models.Result<T> GrpcFailed<T>(RpcException exception)
            => Abstraction.Models.Result<T>.Fail(exception.Message, code: Enum.GetName(exception.StatusCode));

        protected Abstraction.Models.ListResult<T> GrpcListFailed<T>(RpcException exception)
            => Abstraction.Models.ListResult<T>.Fail(exception.Message, code: Enum.GetName(exception.StatusCode));
        #endregion

        #region Failed
        protected Abstraction.Models.Result Failed(Exception exception)
            => Abstraction.Models.Result.Fail(exception.Message, code: exception.GetType().FullName);

        protected Abstraction.Models.Result<T> Failed<T>(Exception exception)
            => Abstraction.Models.Result<T>.Fail(exception.Message, code: exception.GetType().FullName);

        protected Abstraction.Models.ListResult<T> ListFailed<T>(Exception exception)
            => Abstraction.Models.ListResult<T>.Fail(exception.Message, code: exception.GetType().FullName);
        #endregion
    }
}
