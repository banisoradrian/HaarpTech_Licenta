using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using HaarpTech_Licenta.Repository;

namespace HaarpTech_Licenta.Repository
{
    public class RaportCerintaRepository : IRaportCerintaRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaportCerintaRepository(IDatabaseConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        } 

        public async Task<RaportCerinta> GetByIdAsync(string idRaportCerinta)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT 
                                       ID_RAPORT,
                                       DESCRIERE      AS Descriere,
                                       STATUS_RAPORT  AS StatusRaport,
                                       PRIORITATE     AS Prioritate,
                                       STATUS_RESURSE  AS StatusResurse,
                                       STATUS_OFERTA   AS StatusOferta
                                   FROM RAP_RAPORT_CERINTE_V
                                  WHERE ID_RAPORT = @IdRaportSedinta";

            return await conn.QueryFirstOrDefaultAsync<RaportCerinta>(
                sql,
                new { IdRaportSedinta = idRaportCerinta }
            );
        }

        public async Task<bool> AddRaportCerintaAsync(RaportCerinta raportCerinta)
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext!
                             .User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                    return false;

                using (var connection = _dbConnection.GetConnection())
                {
                    var parameters = new
                    {
                        i_ID_RAPORT = Guid.NewGuid().ToString(),
                        i_DESCRIERE = raportCerinta.Descriere ,
                        i_STATUS_RAPORT = raportCerinta.StatusRaport ,
                        i_PRIORITATE = raportCerinta.Prioritate ,
                        i_ID_SEDINTA = raportCerinta.ID_SEDINTA
                    };

                    int affectedRows = await connection.ExecuteAsync(
                        "RAP_RAPORT_CERINTE_PKG_INSERT",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return affectedRows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Eroare la adăugarea raportului de cerințe.", ex);

            }
        }

        public async Task<IEnumerable<RaportCerinta>> GetAllAsync()
        {
            using var connection = _dbConnection.GetConnection();
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            const string sqlAll = @"SELECT 
                                          ID_RAPORT,
                                          DESCRIERE       AS Descriere,
                                          STATUS_RAPORT   AS StatusRaport,
                                          PRIORITATE      AS Prioritate,
                                          STATUS_RESURSE  AS StatusResurse,
                                          STATUS_OFERTA   AS StatusOferta
                                     FROM RAP_RAPORT_CERINTE_V";

            const string sqlAdminConsultatn = @"SELECT 
                                   rc.ID_RAPORT,
                                   rc.DESCRIERE       AS Descriere,
                                   rc.STATUS_RAPORT   AS StatusRaport,
                                   rc.PRIORITATE      AS Prioritate,
                                   rc.STATUS_RESURSE  AS StatusResurse,
                                   rc.STATUS_OFERTA   AS StatusOferta
                               FROM RAP_RAPORT_CERINTE_V rc
                         INNER JOIN SED_SEDINTE_V s
                                 ON rc.ID_RAPORT = s.ID_SEDINTA
                              WHERE s.ID_USER = @ID_USER";

            if (user.IsInRole("Admin") || user.IsInRole("Consultant"))
            {
                return await connection.QueryAsync<RaportCerinta>(sqlAll);
            }
            else
            {
                return await connection.QueryAsync<RaportCerinta>(
                  sqlAdminConsultatn,
                  new { ID_USER = userId }
              );
            }
        }

        public async Task<bool> UpdateRaportCerintaAsync(RaportCerinta raportCerinta)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            var p = new
            {
                i_ID_RAPORT      = raportCerinta.ID_RAPORT,
                i_DESCRIERE      = raportCerinta.Descriere,
                i_STATUS_RAPORT  = raportCerinta.StatusRaport,
                i_PRIORITATE     = raportCerinta.Prioritate,
                i_ID_SEDINTA     = raportCerinta.ID_SEDINTA,
                i_STATUS_RESURSE = raportCerinta.StatusResurse,
                i_STATUS_OFERTA  = raportCerinta.StatusOferta,
            };

            var affected = await conn.ExecuteAsync(
                "RAP_RAPORT_CERINTE_PKG_UPDATE",
                p,
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }

        public async Task<bool> DeleteRaportCerintaAsync(string idRaport)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            var affected = await conn.ExecuteScalarAsync<int>(
                "RAP_RAPORT_CERINTE_PKG_DELETE",
                new { i_ID_RAPORT = idRaport },
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }
    }
}
