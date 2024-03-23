namespace Dariosoft.EmailSender.Infrastructure.Database.Migrations
{
    interface IDatabaseInitializer
    {
        Task EnsureDatabase(string connectionstring);
    }

    internal class DatabaseInitializer : IDatabaseInitializer
    {
        public async Task EnsureDatabase(string connectionstring)
        {
            var info = ExtractInfo(connectionstring);

            if (info is null) return;

            if (await IsDatabaseExists(postgresDbConnectionString: info.Value.PostgresDbConnectionString, dbName: info.Value.CurrentDbConnectionString.Database!))
                return;

            await CreateDatabase(
                postgresDbConnectionString: info.Value.PostgresDbConnectionString,
                dbName: info.Value.CurrentDbConnectionString.Database!,
                dbOwner: info.Value.CurrentDbConnectionString.Username!);
        }

        private async Task CreateDatabase(string postgresDbConnectionString, string dbName, string dbOwner)
        {
            using (var connection = new Npgsql.NpgsqlConnection(postgresDbConnectionString))
            {
                using (var command = new Npgsql.NpgsqlCommand($"CREATE DATABASE \"{dbName}\" WITH OWNER={dbOwner} ENCODING='UTF8' LC_COLLATE='English_United States.1252' LC_CTYPE = 'English_United States.1252' CONNECTION LIMIT = -1;", connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception e)
                    {
                        LogError("Infrastructure.DatabaseInitializer.CreateDatabase", e.Message);
                        throw;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
        }

        private async Task<bool> IsDatabaseExists(string postgresDbConnectionString, string dbName)
        {
            var isDatabaseExists = false;

            using (var connection = new Npgsql.NpgsqlConnection(postgresDbConnectionString))
            {
                using (var command = new Npgsql.NpgsqlCommand("select datname from pg_database where datname = @AName", connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("AName", dbName);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            isDatabaseExists = await reader.ReadAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        LogError("Infrastructure.DatabaseInitializer.IsDatabaseExists", e.Message);
                        throw;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }

            return isDatabaseExists;
        }

        private (string PostgresDbConnectionString, Npgsql.NpgsqlConnectionStringBuilder CurrentDbConnectionString)? ExtractInfo(string connectionstring)
        {
            try
            {
                var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionstring);
                var originalDbName = builder.Database ?? "";
                builder.Database = "postgres";
                var postgreDbConnectionString = builder.ConnectionString;
                builder.Database = originalDbName;

                return (PostgresDbConnectionString: postgreDbConnectionString, CurrentDbConnectionString: builder);
            }
            catch (Exception e)
            {
                LogError("Infrastructure.DatabaseInitializer.ExtractInfo", e.Message);
                return null;
            }
        }

        private void LogError(string section, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("----- ERROR:{0} -----", DateTimeOffset.Now.ToString("MMM-dd  HH:mm"));
            Console.WriteLine("\tSection = {0}", section);
            Console.WriteLine("\t{0}", message);
            Console.WriteLine("----- END ERROR -----");
            Console.ResetColor();
        }

        private void LogInfo(string section, string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("----- INFO:{0} -----", DateTimeOffset.Now.ToString("MMM-dd  HH:mm"));
            Console.WriteLine("\tSection = {0}", section);
            Console.WriteLine("\t{0}", message);
            Console.WriteLine("----- END INFO -----");
            Console.ResetColor();
        }
    }
}
