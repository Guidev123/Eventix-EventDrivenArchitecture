using Eventix.Shared.Application.Data;
using Microsoft.Data.SqlClient;

namespace Eventix.Shared.Infrastructure.Factories
{
    public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public SqlConnection Create() => new(connectionString);
    }
}