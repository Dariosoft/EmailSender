using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Dariosoft.EmailSender.Application;
using Dariosoft.Framework;

namespace Dariosoft.EmailSender.EndPoint
{
    public static class Startup
    {
        public static IServiceCollection AddEndPointLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddApplicationLayer(configuration)
                .RegisterOf<Abstraction.Contracts.IEndPoint, EndPoints.EndPoint>(ServiceLifetime.Scoped);
        }
    }
}
