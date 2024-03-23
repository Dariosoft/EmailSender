using FluentMigrator;
namespace Dariosoft.EmailSender.Infrastructure.Database.Migrations
{
    public abstract class DariosoftMigration : Migration
    {
        protected FluentMigrator.Builders.Create.Table.ICreateTableWithColumnSyntax CreateFlaggedTable(string schema, string tableName)
        {
            var syntax = Create.Table(tableName)
                 .InSchema(schema)
                 .WithColumn(nameof(DataSource.Tables.FlaggedTable.Id)).AsGuid().PrimaryKey($"PK_{schema}_{tableName}")
                 .WithColumn(nameof(DataSource.Tables.FlaggedTable.Serial)).AsInt32().Unique($"UQ_{schema}_{tableName}_Serial")
                 .WithColumn(nameof(DataSource.Tables.FlaggedTable.Flags)).AsInt32()
                 .WithColumn(nameof(DataSource.Tables.FlaggedTable.CreationTime)).AsDateTime();

            return syntax;
        }

        protected FluentMigrator.Builders.Create.Table.ICreateTableWithColumnSyntax CreateTable(string schema, string tableName)
        {
            var syntax = Create.Table(tableName)
                 .InSchema(schema)
                 .WithColumn(nameof(DataSource.Tables.BaseTable.Id)).AsGuid().PrimaryKey($"PK_{schema}_{tableName}")
                 .WithColumn(nameof(DataSource.Tables.BaseTable.Serial)).AsInt32().Unique($"UQ_{schema}_{tableName}_Serial");

            return syntax;
        }

        protected void SetSerialIdentity(string schema, string tableName)
        {
            var tableFullName = $"\"{schema}\".\"{tableName}\"";

            IfDatabase("postgres")
               .Execute
               .Sql($"ALTER TABLE {tableFullName} ALTER \"Serial\" ADD GENERATED ALWAYS AS IDENTITY(START WITH 10 INCREMENT BY 1);");

            Create.Index($"IX_core_{tableName}_Serial")
               .OnTable(tableName)
               .InSchema(schema)
               .OnColumn("Serial")
               .Descending();
        }
    }
}
