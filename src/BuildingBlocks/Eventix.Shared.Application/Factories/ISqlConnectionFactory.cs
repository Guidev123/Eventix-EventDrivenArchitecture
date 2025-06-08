using Microsoft.Data.SqlClient;

namespace Eventix.Shared.Application.Factories
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}