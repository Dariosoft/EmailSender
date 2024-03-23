using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.EndPoint.gRPC.SDK
{
    public static class Startup
    {
        public static IServiceCollection AddEmailSenderGrpcSDK(this IServiceCollection services, Func<IServiceProvider, IConnectionInfo> connectionInfoProvider)
        {
            return services
                .AddSingleton<Abstraction.Contracts.IHostEndPoint, Services.HostService>()
                .AddSingleton<Abstraction.Contracts.IAccountEndPoint, Services.AccountService>()
                .AddSingleton<Abstraction.Contracts.IMessageEndPoint, Services.MessageService>()
                .AddSingleton<IConnectionInfo>(connectionInfoProvider);
        }
    }
}
