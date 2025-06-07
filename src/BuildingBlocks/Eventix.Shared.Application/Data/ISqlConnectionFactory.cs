using Microsoft.Data.SqlClient;

namespace Eventix.Shared.Application.Data
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}