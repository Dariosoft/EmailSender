using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.Infrastructure.Database
{
    public static class Startup
    {
        public static IServiceCollection RegisterDatabaseLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
