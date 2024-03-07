using Dariosoft.EmailSender.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection RegisterInfrastructures(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .RegisterDatabaseLayer(configuration);
        }
    }
}
