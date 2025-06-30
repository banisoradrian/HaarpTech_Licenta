using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using System.Data;

namespace HaarpTech_Licenta.Repository
{
    public interface IStatusTichetRepository
    {
        Task<StatusTichet> GetByIdAsync(string id);
        Task<IEnumerable<StatusTichet>> GetAllAsync();
        Task<bool> AddTichetAsync(StatusTichet tichet);
        Task<bool> UpdateTichetAsync(StatusTichet tichet);
        Task<bool> DeleteTichetAsync(string idTichet);
        //Task<IEnumerable<StatusTichet>> GetAllWithTichetAsync(string id);
        Task<IEnumerable<StatusTichet>> GetAllByIdAsync(string id);
        Task<bool> AssignToAngajatAsync(string tichetId, string angajatId);

    }

    public class StatusTichetRepository : IStatusTichetRepository
    {

        private readonly IDatabaseConnection _databaseConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StatusTichetRepository(IDatabaseConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _databaseConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AssignToAngajatAsync(string tichetId, string angajatId)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "STS_STATUS_TICHETE_ASSIGN_ANGAJAT_PKG_INSERT";

            var parameters = new
            {
                i_ID_TICHET = tichetId,
                i_ID_ANGAJAT = angajatId
            };

            var rows = await conn.ExecuteAsync(procedureName, parameters , commandType: CommandType.StoredProcedure);
            return rows > 0;
        }


        public async Task<StatusTichet> GetByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"
                             SELECT
                                    ID_TICHET	
                                   ,ID_CERINTA	
                                   ,ID_ANGAJAT	
                                   ,DATA_CREARE     AS DataCreare
                                   ,DATA_FINALIZARE	AS DataFinalizare
                                   ,SUBIECT	        AS Subiect
                                   ,DESCRIERE	    AS Descriere
                                   ,STATUS_TICHET   As Status_Tichet
                                   ,PRIORITATE      AS Prioritate
                              FROM STS_STATUS_TICHETE_V
                            WHERE  ID_TICHET = @IdCerere";
            return await conn.QueryFirstOrDefaultAsync<StatusTichet>(
                sql,
                new { IdCerere = id }
            );
        }

        public async Task<IEnumerable<StatusTichet>> GetAllAsync()
        {
            using var connection = _databaseConnection.GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            string sql = @"
                            SELECT
                                    ID_TICHET	
                                   ,ID_CERINTA	
                                   ,ID_ANGAJAT	
                                   ,DATA_CREARE     AS DataCreare
                                   ,DATA_FINALIZARE	AS DataFinalizare
                                   ,SUBIECT	        AS Subiect
                                   ,DESCRIERE	    AS Descriere
                                   ,STATUS_TICHET   As Status_Tichet
                                   ,PRIORITATE      AS Prioritate
                             FROM STS_STATUS_TICHETE_V";
            return await connection.QueryAsync<StatusTichet>(sql);
        }

        public async Task<IEnumerable<StatusTichet>> GetAllByIdAsync(string id)
        {
            using var connection = _databaseConnection.GetConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            string sql = @"
                            SELECT
                                    ID_TICHET	
                                   ,ID_CERINTA	
                                   ,ID_ANGAJAT	
                                   ,DATA_CREARE     AS DataCreare
                                   ,DATA_FINALIZARE	AS DataFinalizare
                                   ,SUBIECT	        AS Subiect
                                   ,DESCRIERE	    AS Descriere
                                   ,STATUS_TICHET   As Status_Tichet
                                   ,PRIORITATE      AS Prioritate
                             FROM STS_STATUS_TICHETE_V
                            WHERE ID_CERINTA =@IdCerinta";
            return await connection.QueryAsync<StatusTichet>
            (
                sql,
                new {IdCerinta = id}
            );
        }

        public async Task<bool> AddTichetAsync(StatusTichet tichet)
        {

            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    string procedureName = "STS_STATUS_TICHETE_PKG_INSERT";
                    var parameter = new
                    {
                        i_ID_TICHET = Guid.NewGuid().ToString(),
                        i_ID_CERINTA = tichet.ID_CERINTA,
                        i_ID_ANGAJAT = tichet.ID_ANGAJAT,
                        i_DATA_CREARE = DateTime.Now,
                        i_DATA_FINALIZARE = (object)DBNull.Value,
                        i_SUBIECT = tichet.Subiect,
                        i_DESCRIERE = tichet.Descriere,
                        i_STATUS_TICHET = tichet.Status_Tichet,
                        i_PRIORITATE = tichet.Prioritate
                    };

                    int affectedRows = await conn.ExecuteAsync
                    (
                        procedureName, parameter, commandType: CommandType.StoredProcedure
                    );
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTichetAsync(StatusTichet tichet)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string procedureName = "STS_STATUS_TICHETE_PKG_UPDATE";
            var parameter = new
            {
                i_ID_TICHET = tichet.ID_TICHET,
                i_ID_CERINTA = tichet.ID_CERINTA,
                i_ID_ANGAJAT = tichet.ID_ANGAJAT,
                i_DATA_CREARE = tichet.DataCreare,
                i_DATA_FINALIZARE = tichet.DataFinalizare,
                i_SUBIECT = tichet.Subiect,
                i_DESCRIERE = tichet.Descriere,
                i_STATUS_TICHET = tichet.Status_Tichet,
                i_PRIORITATE = tichet.Prioritate
            };
            int affected = 0;
            try
            {
                affected = await conn.ExecuteAsync(
                  procedureName,
                  parameter,
                  commandType: CommandType.StoredProcedure
              );
            }
            catch (Exception ex)
            {
                throw;
            }

            return affected > 0;
        }

        public async Task<bool> DeleteTichetAsync(string idTichet)
        {
            try
            {
                using var conn = _databaseConnection.GetConnection();
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int affected = await conn.ExecuteScalarAsync<int>(
                    "STS_STATUS_TICHETE_PKG_DELETE",
                    new { i_ID_TICHET = idTichet },
                    commandType: CommandType.StoredProcedure
                );
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
