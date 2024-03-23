using LinqToDB.Common;
using LinqToDB.Interceptors;
using System.Data;
using System.Data.Common;

namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource
{
    internal class DbContext : PostgresDbContext
    {
        public DbContext(string connectionString)
            : base(connectionString)
        {
            //this.AddInterceptor(new QueryInterceptor());

            // one-time command prepared interceptor
            //this.OnNextCommandInitialized((args, cmd) =>
            //{
            //    return cmd;
            //});
        }
        public ITable<Tables.Admin> Admins => this.GetTable<Tables.Admin>().SchemaName(DbSchema.Core);
        public ITable<Tables.Client> Clients => this.GetTable<Tables.Client>().SchemaName(DbSchema.Core);
        public ITable<Tables.Host> Hosts => this.GetTable<Tables.Host>().SchemaName(DbSchema.Core);
        public ITable<Tables.Account> Accounts => this.GetTable<Tables.Account>().SchemaName(DbSchema.Core);
        public ITable<Tables.Message> Messages => this.GetTable<Tables.Message>().SchemaName(DbSchema.Core);
        public ITable<Tables.MessageTrySendLog> MessageSendLogs => this.GetTable<Tables.MessageTrySendLog>().SchemaName(DbSchema.Core);
        public ITable<Tables.MailAddressCollection> MailAddressCollections => this.GetTable<Tables.MailAddressCollection>().SchemaName(DbSchema.Core);
        public ITable<Tables.MailAddressCollectionItem> MailAddressCollectionItems => this.GetTable<Tables.MailAddressCollectionItem>().SchemaName(DbSchema.Core);


    }

    class QueryInterceptor : LinqToDB.Interceptors.CommandInterceptor
    {
        public override Option<DbDataReader> ExecuteReader(CommandEventData eventData, DbCommand command, CommandBehavior commandBehavior, Option<DbDataReader> result)
        {
            var sql = command.CommandText;

            return base.ExecuteReader(eventData, command, commandBehavior, result);
        }

        public override Task<Option<DbDataReader>> ExecuteReaderAsync(CommandEventData eventData, DbCommand command, CommandBehavior commandBehavior, Option<DbDataReader> result, CancellationToken cancellationToken)
        {
            var sql = command.CommandText;

            return base.ExecuteReaderAsync(eventData, command, commandBehavior, result, cancellationToken);
        }
    }
}
