using Dariosoft.EmailSender.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.Application
{
    public static class Startup
    {
        public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .RegisterInfrastructures(configuration);
        }
    }
}
