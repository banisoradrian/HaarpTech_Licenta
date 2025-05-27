using Microsoft.Data.SqlClient;
using System.Data;

namespace HaarpTech_Licenta.Data
{


    public interface IDatabaseConnection
    {
        IDbConnection GetConnection();
    }
}

