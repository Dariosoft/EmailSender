using FluentMigrator;
using FluentMigrator.Postgres;
namespace Dariosoft.EmailSender.Infrastructure.Database.Migrations
{
    [Migration(2024_03_21_1515_01, description: "Database setup")]
    public class M2024_03_21_15_15_01 : Migration
    {
        public override void Up()
        {
            IfDatabase("postgres").Execute.Sql("CREATE EXTENSION ltree;");

            if (!Schema.Schema(DataSource.DbSchema.Core).Exists())
                Create.Schema(DataSource.DbSchema.Core);
        }

        public override void Down()
        {
            Delete.Schema(DataSource.DbSchema.Core);
            IfDatabase("postgres").Execute.Sql("DROP EXTENSION ltree;");
        }
    }
}
