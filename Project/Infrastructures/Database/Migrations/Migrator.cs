using Microsoft.Extensions.Configuration;

namespace Dariosoft.EmailSender.Infrastructure.Database.Migrations
{

    internal class Migrator(IConfiguration configuration, IDatabaseInitializer dbInitializer, MigrationRunnerProvider migrationRunnerProvider)
    {
        public async Task MigrateUp()
        {
            try
            {
                await dbInitializer.EnsureDatabase(configuration.GetConnectionString("main") ?? "");

                migrationRunnerProvider.Provide().MigrateUp();
                //migrationRunnerProvider.Provide().MigrateDown(2024_03_21_1515_01);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
