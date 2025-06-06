using Eventix.Modules.Events.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace Eventix.Modules.Events.Infrastructure.Factories
{
    public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public SqlConnection Create() => new(connectionString);
    }
}