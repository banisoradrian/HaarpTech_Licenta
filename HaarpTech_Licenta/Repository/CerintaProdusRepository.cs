using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using System.Data;
using System.Data.Common;

namespace HaarpTech_Licenta.Repository
{
    public interface ICerintaProdusRepository
    {

        Task<CerintaProdus> GetByIdAsync(string id);
        Task<IEnumerable<CerintaProdus>> GetAllAsync();
        Task<bool> AddCerintaProdus(CerintaProdus cerintaProdus);
        Task<bool> UpdateCerintaProdusAsync(CerintaProdus cerintaProdus);
        Task<bool> DeleteCerintaProdusAsync(string idCerinta);
        Task<IEnumerable<CerintaProdus>> GetAllWithCereriOferteAsync(string id);
        Task<IEnumerable<CerintaProdus>> GetAllByIdAsync(string id);
    }

    public class CerintaProdusRepository : ICerintaProdusRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CerintaProdusRepository(IDatabaseConnection databaseConnection, IHttpContextAccessor httpContextAccessor)
        {
            _databaseConnection = databaseConnection;
            _httpContextAccessor = httpContextAccessor;
        }
    
        public async Task<CerintaProdus> GetByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if(conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"
                            SELECT 
	                             ID_CERINTA
                                ,ID_CERERE
                                ,ID_ANGAJAT
                                ,DATA_ELABORARE AS DataElaborare
                                ,DESCRIERE_DETALIATA AS DescriereDetaliata
                                ,PRIORITATE	AS Prioritate
                                ,STATUS_CERINTA AS StatusCerinta
                            FROM CRP_CERINTE_PRODUSE_V
                           WHERE ID_CERINTA = @ID_CERINTA";
            return await conn.QueryFirstOrDefaultAsync<CerintaProdus>(
                sql,
                new { ID_CERINTA = id }
                );
        }

        public async Task<IEnumerable<CerintaProdus>> GetAllByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"
                            SELECT 
	                             ID_CERINTA
                                ,ID_CERERE
                                ,ID_ANGAJAT
                                ,DATA_ELABORARE AS DataElaborare
                                ,DESCRIERE_DETALIATA AS DescriereDetaliata
                                ,PRIORITATE	AS Prioritate
                                ,STATUS_CERINTA AS StatusCerinta
                            FROM CRP_CERINTE_PRODUSE_V
                           WHERE ID_CERERE= @IdCerere";
            return await conn.QueryAsync<CerintaProdus>(
                sql,
                new { IdCerere = id }
                );
        }


        public async Task<IEnumerable<CerintaProdus>> GetAllWithCereriOferteAsync(string idCerere)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"
        SELECT 
            CP.ID_CERINTA,
            CP.ID_CERERE,
            CP.DATA_ELABORARE    AS DataElaborare,
            CP.DESCRIERE_DETALIATA AS DescriereDetaliata,
            CP.PRIORITATE         AS Prioritate,
            CP.STATUS_CERINTA     AS StatusCerinta,

            CO.ID_CERERE          AS ID_CERERE_OFERTA,
            CO.DENUMIRE_CERERE    AS DenumireCerere
        FROM CRP_CERINTE_PRODUSE_V CP
        JOIN CRO_CERERI_OFERTE_V CO
          ON CO.ID_CERERE = CP.ID_CERERE
        WHERE CP.ID_CERERE = @IdCerere;";

            var result = await conn.QueryAsync<CerintaProdus, CerereOferta, CerintaProdus>(
                sql,
                map: (cerinta, oferta) =>
                {
                    cerinta.CerereOferta = oferta;
                    return cerinta;
                },
                splitOn: "ID_CERERE_OFERTA",
                param: new { IdCerere = idCerere }
            );

            return result;
        }

        public async Task<IEnumerable<CerintaProdus>> GetAllAsync()
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"
                            SELECT 
	                               ID_CERINTA
                                  ,ID_CERERE
                                  ,ID_ANGAJAT
                                  ,DATA_ELABORARE AS DataElaborare
                                  ,DESCRIERE_DETALIATA AS DescriereDetaliata
                                  ,PRIORITATE	AS Prioritate
                                  ,STATUS_CERINTA AS StatusCerinta
                             FROM CRP_CERINTE_PRODUSE_V ";
            return await conn.QueryAsync<CerintaProdus>(sql);
        }

        

        public async Task<bool> AddCerintaProdus(CerintaProdus cerintaProdus)
        {
            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    string procedureName = "CRP_CERINTE_PRODUSE_PKG_INSERT";
                    var parameter = new
                    {
                        i_ID_CERINTA = Guid.NewGuid().ToString(),
                        i_ID_CERERE = cerintaProdus.ID_CERERE,
                        i_ID_ANGAJAT = cerintaProdus.ID_ANGJAT,
                        i_DATA_ELABORARE = DateTime.Now,
                        i_DESCRIERE_DETALIATA = cerintaProdus.DescriereDetaliata,
                        i_PRIORITATE = cerintaProdus.Prioritate,
                        i_STATUS_CERINTA = cerintaProdus.StatusCerinta
                    };

                    int affectedRows = await conn.ExecuteAsync
                    (
                        procedureName, parameter, commandType: CommandType.StoredProcedure
                    );
                    return affectedRows > 0;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCerintaProdusAsync(CerintaProdus cerintaProdus)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string procedureName = "CRP_CERINTE_PRODUSE_PKG_UPDATE";
            var parameter = new
            {
                i_ID_CERINTA = cerintaProdus.ID_CERINTA,
                i_ID_CERERE = cerintaProdus.ID_CERERE,
                i_ID_ANGAJAT = cerintaProdus.ID_ANGJAT,
                i_DATA_ELABORARE = cerintaProdus.DataElaborare,
                i_DESCRIERE_DETALIATA = cerintaProdus.DescriereDetaliata,
                i_PRIORITATE = cerintaProdus.Prioritate,
                i_STATUS_CERINTA = cerintaProdus.StatusCerinta
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

        public async Task<bool> DeleteCerintaProdusAsync(string idCerinta)
        {
            try
            {
                using var conn = _databaseConnection.GetConnection();
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int affected = await conn.ExecuteScalarAsync<int>(
                    "CRP_CERINTE_PRODUSE_PKG_DELETE",
                    new { i_ID_CERINTA = idCerinta},
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
