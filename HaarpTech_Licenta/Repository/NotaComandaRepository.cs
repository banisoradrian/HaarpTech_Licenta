using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using System.Data;
using System.Reflection.Metadata;

namespace HaarpTech_Licenta.Repository
{

    public interface INotaComandaRepository
    {
        Task<NotaComanda> GetByIdAsync(string id);
        Task<IEnumerable<NotaComanda>> GetAllAsync();
        Task<bool> AddNotaComandaAsync(NotaComanda notaComanda);
        Task<bool> UpdateNotaComandaAsync(NotaComanda notaComanda);
        Task<bool> DeleteNotaComandaAsync(string idNotaComanda);
        Task<IEnumerable<NotaComanda>> GetAllByIdAsync(string id);
    }

    public class NotaComandaRepository : INotaComandaRepository
    {
        private readonly IDatabaseConnection _databaseConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotaComandaRepository(IDatabaseConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _databaseConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<NotaComanda> GetByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"SELECT
                                  ID_NOTA_COMANDA,
                                  ID_CERERE,
                                  DESTINATAR            AS Destinatar,
                                  DATA_NOTA             AS DataNota,
                                  DATA_EMITERII         AS DataEmiterii,
                                  ACTIVITATE            AS Activitate,
                                  DENUMIRE              AS Denumire,
                                  UM                    AS Um,
                                  CANTITATE             AS Cantitate,
                                  PRET_TOTAL_FARA_TVA   AS PretTotalFaraTva,
                                  PRET_TOTAL_TVA        AS PretTotalTva,
                                  TERMEN_LIVRARE        AS TermenLivrare,
                                  IBAN                  AS Iban,
                                  BANCA                 AS Banca,
                                  NUMAR_COMANDA         AS NumarComanda
                             FROM NDC_NOTA_COMANDA_V
                            WHERE ID_NOTA_COMANDA = @id ";
            var parameters = new { id };
            return await conn.QueryFirstOrDefaultAsync<NotaComanda>(sql, parameters);
        }

        public async Task<IEnumerable<NotaComanda>> GetAllAsync()
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"SELECT
                                  ID_NOTA_COMANDA,
                                  ID_CERERE,
                                  DESTINATAR            AS Destinatar,
                                  DATA_NOTA             AS DataNota,
                                  DATA_EMITERII         AS DataEmiterii,
                                  ACTIVITATE            AS Activitate,
                                  DENUMIRE              AS Denumire,
                                  UM                    AS Um,
                                  CANTITATE             AS Cantitate,
                                  PRET_TOTAL_FARA_TVA   AS PretTotalFaraTva,
                                  PRET_TOTAL_TVA        AS PretTotalTva,
                                  TERMEN_LIVRARE        AS TermenLivrare,
                                  IBAN                  AS Iban,
                                  BANCA                 AS Banca,
                                  NUMAR_COMANDA         AS NumarComanda
                             FROM NDC_NOTA_COMANDA_V";
            return await conn.QueryAsync<NotaComanda>(sql);
        }


        public async Task<IEnumerable<NotaComanda>> GetAllByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"SELECT
                                  ID_NOTA_COMANDA,
                                  ID_CERERE,
                                  DESTINATAR            AS Destinatar,
                                  DATA_NOTA             AS DataNota,
                                  DATA_EMITERII         AS DataEmiterii,
                                  ACTIVITATE            AS Activitate,
                                  DENUMIRE              AS Denumire,
                                  UM                    AS Um,
                                  CANTITATE             AS Cantitate,
                                  PRET_TOTAL_FARA_TVA   AS PretTotalFaraTva,
                                  PRET_TOTAL_TVA        AS PretTotalTva,
                                  TERMEN_LIVRARE        AS TermenLivrare,
                                  IBAN                  AS Iban,
                                  BANCA                 AS Banca,
                                  NUMAR_COMANDA         AS NumarComanda
                             FROM NDC_NOTA_COMANDA_V
                           WHERE ID_CERERE= @IdCerere";
            return await conn.QueryAsync<NotaComanda>(
                sql,
                new { IdCerere = id }
                );
        }


        public async Task<bool> AddNotaComandaAsync(NotaComanda notaComanda)
        {
            try
            {
                using var conn = _databaseConnection.GetConnection();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                const string procedureName = "NDC_NOTA_COMANDA_PKG_INSERT";

                DateTime data = notaComanda.DataEmiterii;
                string numarComanda = $"V_{data.Day}_{data.Month}_{data.Year}_{notaComanda.Um}_{notaComanda.Cantitate}";

                var parameters = new
                {
                    i_ID_NOTA_COMANDA = Guid.NewGuid().ToString(),
                    i_ID_CERERE = notaComanda.ID_CERERE,
                    i_DESTINATAR = notaComanda.Destinatar,
                    i_DATA_NOTA = notaComanda.DataNota,
                    i_DATA_EMITERII = notaComanda.DataEmiterii,
                    i_ACTIVITATE = notaComanda.Activitate,
                    i_DENUMIRE = notaComanda.Denumire,
                    i_UM = notaComanda.Um,
                    i_CANTITATE = notaComanda.Cantitate,
                    i_PRET_TOTAL_FARA_TVA = notaComanda.PretTotalFaraTva,
                    i_PRET_TOTAL_TVA = notaComanda.PretTotalFaraTva * 1.19m,
                    i_TERMEN_LIVRARE = notaComanda.TermenLivrare,
                    i_IBAN = notaComanda.Iban,
                    i_BANCA = notaComanda.Banca,
                    i_NUMAR_COMANDA = numarComanda
                };
                
                var rows = await conn.ExecuteAsync
                (
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }





        public async Task<bool> UpdateNotaComandaAsync(NotaComanda notaComanda)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "NDC_NOTA_COMANDA_PKG_UPDATE";
            var parameters = new
            {
                i_ID_NOTA_COMANDA = notaComanda.ID_NOTA_COMANDA,
                i_ID_CERERE = notaComanda.ID_CERERE,
                i_DESTINATAR = notaComanda.Destinatar,
                i_DATA_NOTA = notaComanda.DataNota,
                i_DATA_EMITERII = notaComanda.DataEmiterii,
                i_ACTIVITATE = notaComanda.Activitate,
                i_DENUMIRE = notaComanda.Denumire,
                i_UM = notaComanda.Um,
                i_CANTITATE = notaComanda.Cantitate,
                i_PRET_TOTAL_FARA_TVA = notaComanda.PretTotalFaraTva,
                i_PRET_TOTAL_TVA = notaComanda.PretTotalTva,
                i_TERMEN_LIVRARE = notaComanda.TermenLivrare,
                i_IBAN = notaComanda.Iban,
                i_BANCA = notaComanda.Banca,
                i_NUMAR_COMANDA = notaComanda.NumarComanda
            };
            int affected = 0;
            try
            {
                  affected = await conn.ExecuteAsync(
                  procedureName,
                  parameters,
                  commandType: CommandType.StoredProcedure
              );
            }
            catch (Exception ex)
            {
                throw;
            }

            return affected > 0;
        }

        public async Task<bool> DeleteNotaComandaAsync(string idNotaComanda)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "NDC_NOTA_COMANDA_PKG_DELETE";

            var parameters = new { i_ID_NOTA_COMANDA = idNotaComanda };
            int affected = 0;
            try
            {
                affected = await conn.ExecuteAsync(
                  procedureName,
                  parameters,
                  commandType: CommandType.StoredProcedure
              );
            }
            catch (Exception ex)
            {
                throw;
            }
            return affected > 0;
        }
    }
}
