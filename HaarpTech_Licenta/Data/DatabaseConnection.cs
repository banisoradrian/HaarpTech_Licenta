using Microsoft.Data.SqlClient;
using static HaarpTech_Licenta.Data.DatabaseConnection;
using System.Data;

namespace HaarpTech_Licenta.Data
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
