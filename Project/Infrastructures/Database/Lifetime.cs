using Dariosoft.EmailSender.Infrastructure.Database.Migrations;
using Microsoft.Extensions.Configuration;

namespace Dariosoft.EmailSender.Infrastructure.Database
{
    class Lifetime(Migrator migrator) : Framework.ILifetime
    {
        public int Order { get; } = 1;

        public bool Enabled { get; } = true;

        public Task OnAppStarted()
        {
          
            return migrator.MigrateUp();

            // return Task.CompletedTask;
        }

        public Task OnAppStoping()
        {
            return Task.CompletedTask;
        }

        public Task OnAppStopped()
        {
            return Task.CompletedTask;
        }
    }
}
