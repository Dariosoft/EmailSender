using Microsoft.Extensions.Configuration;

namespace Dariosoft.EmailSender.Infrastructure.Database.Repositories
{
    internal sealed class RepositoryInjection(IConfiguration configuration)
    {
        public string GetMainConnectionString() => configuration.GetConnectionString("main") ?? "";
    }
}
