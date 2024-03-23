using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.Infrastructure.Database
{
    public static class Startup
    {
        public static IServiceCollection RegisterDatabaseLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddRepositories(configuration)
                .AddMigrator(configuration)
                .AddSingleton<Framework.ILifetime, Lifetime>();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                  .AddSingleton<Repositories.RepositoryInjection>()
                  .RegisterOf<Core.Repositories.IRepository, Repositories.Repository>(ServiceLifetime.Singleton);
        }

        private static IServiceCollection AddMigrator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    rb.AddPostgres()
                    .WithGlobalConnectionString(configuration.GetConnectionString("main"))
                    .ScanIn(typeof(Migrations.DatabaseInitializer).Assembly)
                    .For
                    .Migrations();
                });

            return services
                .AddSingleton<Migrations.Migrator>()
                .AddSingleton<Migrations.MigrationRunnerProvider>()
                .AddSingleton<Migrations.IDatabaseInitializer, Migrations.DatabaseInitializer>();
        }
    }
}
