using Microsoft.Data.SqlClient;

namespace Eventix.Modules.Events.Application.Abstractions.Data
{
    public interface ISqlConnectionFactory
    {
        SqlConnection Create();
    }
}