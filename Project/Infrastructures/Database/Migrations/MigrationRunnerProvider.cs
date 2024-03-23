using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.Infrastructure.Database.Migrations
{
    class MigrationRunnerProvider(IServiceProvider serviceProvider)
    {
        public IMigrationRunner Provide()
            => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IMigrationRunner>();
    }
}
