using LinqToDB.Data;
using System;
using System.Linq;

namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource
{
    internal abstract class PostgresDbContext(string connectionString)
        : DataConnection(providerName: ProviderName.PostgreSQL, connectionString: connectionString)
    {
    }
}
