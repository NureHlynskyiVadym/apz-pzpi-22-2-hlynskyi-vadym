using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LevelUp.Services
{
    public class DatabaseBackupService
    {
        private readonly IConfiguration _configuration;

        public DatabaseBackupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> BackupDatabaseAsync(string backupDirectory)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var builder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = builder.InitialCatalog;
            var backupFileName = $"{databaseName}_{DateTime.UtcNow:yyyyMMddHHmmss}.bak";
            var backupFilePath = Path.Combine(backupDirectory, backupFileName);

            var sql = $@"BACKUP DATABASE [{databaseName}] TO DISK = N'{backupFilePath}' WITH FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return backupFilePath;
        }
    }
}
