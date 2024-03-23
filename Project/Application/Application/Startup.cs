using Dariosoft.EmailSender.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<Concrete.ServiceInjection>()
                .RegisterOf<IService, Concrete.Service>(ServiceLifetime.Singleton)
                .RegisterInfrastructures(configuration);
        }
    }
}
