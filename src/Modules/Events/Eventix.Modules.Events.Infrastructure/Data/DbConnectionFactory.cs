using Microsoft.Data.SqlClient;

namespace Eventix.Modules.Events.Infrastructure.Data
{
    public sealed class DbConnectionFactory(string connectionString)
    {
        public SqlConnection Create() => new(connectionString);
    }
}