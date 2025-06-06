using Eventix.Modules.Events.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace Eventix.Modules.Events.Infrastructure.Data
{
    public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public SqlConnection Create() => new(connectionString);
    }
}