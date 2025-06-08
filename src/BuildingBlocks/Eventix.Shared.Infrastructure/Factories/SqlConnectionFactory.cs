using Eventix.Shared.Application.Factories;
using Microsoft.Data.SqlClient;

namespace Eventix.Shared.Infrastructure.Factories
{
    public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public SqlConnection Create() => new(connectionString);
    }
}