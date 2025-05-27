using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace HaarpTech_Licenta.Repository
{
    public class SedintaRepository : ISedintaRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SedintaRepository(IDatabaseConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Sedinta> GetByIdAsync(string id)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT 
                                       ID_SEDINTA,
                                       DATA_SEDINTA    AS DataSedinta,
                                       SUBIECT         AS Subiect,
                                       DESCRIERE       AS Descriere,
                                       STATUS_SEDINTA  AS StatusSedinta,
                                       LOCATIE         AS Locatie,
                                       TIP_SEDINTA     AS TipSedinta
                                   FROM SED_SEDINTE_V
                                  WHERE ID_SEDINTA = @IdSedinta";

            return await conn.QueryFirstOrDefaultAsync<Sedinta>(
                sql,
                new { IdSedinta = id }
            );
        }

        public async  Task<bool> AddSedintaAsync(Sedinta sedinta)
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
                        i_ID_SEDINTA = Guid.NewGuid().ToString(),
                        i_DATA_SEDINTA = sedinta.DataSedinta,
                        i_SUBIECT = sedinta.Subiect,
                        i_DESCRIERE = sedinta.Descriere,
                        i_STATUS_SEDINTA = sedinta.StatusSedinta,
                        i_LOCATIE = sedinta.Locatie,
                        i_TIP_SEDINTA = sedinta.TipSedinta,
                        i_ID_CERERE = sedinta.ID_CERERE,
                        i_ID_USER = userId
                    };

                    int affectedRows = await connection.ExecuteAsync(
                        "SED_SEDINTE_PKG_INSERT",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return affectedRows > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Eroare la adăugarea ședinței.", ex);
                
            }
        }

        public async Task<IEnumerable<Sedinta>> GetAllAsync()
        {
            using var connection = _dbConnection.GetConnection();
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (user.IsInRole("Admin") || user.IsInRole("Consultant"))
            {
                return await connection.QueryAsync<Sedinta>(@"SELECT 
                                                                          ID_SEDINTA,
                                                                          DATA_SEDINTA    AS DataSedinta,
                                                                          SUBIECT         AS Subiect,
                                                                          DESCRIERE       AS Descriere,
                                                                          STATUS_SEDINTA  AS StatusSedinta,
                                                                          LOCATIE         AS Locatie,
                                                                          TIP_SEDINTA     AS TipSedinta
                                                                    FROM SED_SEDINTE_V");
            }
            else
            {
                const string sql = @"SELECT
                                          ID_SEDINTA,
                                          DATA_SEDINTA    AS DataSedinta,
                                          SUBIECT         AS Subiect,
                                          DESCRIERE       AS Descriere,
                                          STATUS_SEDINTA  AS StatusSedinta,
                                          LOCATIE         AS Locatie,
                                          TIP_SEDINTA     AS TipSedinta
                                      FROM SED_SEDINTE_V 
                                   WHERE ID_SEDINTA = @ID_SEDINTA";

                return await connection.QueryAsync<Sedinta>(
                    sql,
                    new { ID_SEDINTA = userId }
                );
            }
        }

        public async Task<bool> UpdateSedintaAsync(Sedinta s)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open) 
                conn.Open();

            string userId = _httpContextAccessor
                            .HttpContext!
                            .User.FindFirstValue(ClaimTypes.NameIdentifier);

            var p = new
            {
                i_ID_SEDINTA = s.ID_SEDINTA,
                i_DATA_SEDINTA = s.DataSedinta,
                i_SUBIECT = s.Subiect,
                i_DESCRIERE = s.Descriere,
                i_STATUS_SEDINTA = s.StatusSedinta,
                i_LOCATIE = s.Locatie,
                i_TIP_SEDINTA = s.TipSedinta,
                i_ID_USER = userId,
                i_ID_CERERE = s.ID_CERERE
            };

            var affected = await conn.ExecuteAsync(
                "SED_SEDINTE_PKG_UPDATE",
                p,
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }

        public async Task<bool> DeleteSedintaAsync(string idSedinta)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            var affected = await conn.ExecuteAsync(
                "SED_SEDINTE_PKG_DELETE",
                new { i_ID_SEDINTA = idSedinta },
                commandType: CommandType.StoredProcedure
            );

            return affected > 0;
        }

    }
}
