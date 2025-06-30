using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HaarpTech_Licenta.Repository;
using HaarpTech_Licenta.Models;
using HaarpTech_Licenta.Data;

namespace HaarpTech_Licenta.Repository
{
    public interface IClientDataRepository
    {
        Task<ClientData> GetByIdAsync(string idClient);
        Task<IEnumerable<ClientData>> GetAllAsync();
        Task<IEnumerable<ClientData>> GetAllByIdAsync(string id);
        Task<bool> AddClientAsync(ClientData client);
        Task<bool> UpdateClientAsync(ClientData client);
        Task<bool> DeleteClientAsync(string idClient);
    }

    public class ClientDataRepository : IClientDataRepository
    {
        private readonly IDatabaseConnection _databaseConnection;

        public ClientDataRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<ClientData> GetByIdAsync(string idClient)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @" SELECT
                                         ID_CLIENT         AS ID_CLIENT,
                                         ID_CONTRACT       AS ID_CONTRACT,
                                         DENUMIRE_FISCALA  AS DenumireFiscala,
                                         CUI               AS Cui,
                                         NR_REG_COM        AS NrRegCom,
                                         SEDIU             AS Seduiu,
                                         JUDET             AS Judet,
                                         TARA              AS Tara,
                                         CONT_IBAN         AS ContIban,
                                         BANCA             AS Banca,
                                         TELEFON           AS Telefon,
                                         EMAIL             AS Email
                                     FROM CLI_CLIENT_DATA_V
                                    WHERE ID_CLIENT = @IdClient";

            return await conn.QueryFirstOrDefaultAsync<ClientData>(sql, new { IdClient = idClient });
        }

        public async Task<IEnumerable<ClientData>> GetAllAsync()
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT
                                        ID_CLIENT         AS ID_CLIENT,
                                        ID_CONTRACT       AS ID_CONTRACT,
                                        DENUMIRE_FISCALA  AS DenumireFiscala,
                                        CUI               AS Cui,
                                        NR_REG_COM        AS NrRegCom,
                                        SEDIU             AS Seduiu,
                                        JUDET             AS Judet,
                                        TARA              AS Tara,
                                        CONT_IBAN         AS ContIban,
                                        BANCA             AS Banca,
                                        TELEFON           AS Telefon,
                                        EMAIL             AS Email
                                    FROM CLI_CLIENT_DATA_V";

            return await conn.QueryAsync<ClientData>(sql);
        }

        public async Task<IEnumerable<ClientData>> GetAllByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT
                                        ID_CLIENT         AS ID_CLIENT,
                                        ID_CONTRACT       AS ID_CONTRACT,
                                        DENUMIRE_FISCALA  AS DenumireFiscala,
                                        CUI               AS Cui,
                                        NR_REG_COM        AS NrRegCom,
                                        SEDIU             AS Seduiu,
                                        JUDET             AS Judet,
                                        TARA              AS Tara,
                                        CONT_IBAN         AS ContIban,
                                        BANCA             AS Banca,
                                        TELEFON           AS Telefon,
                                        EMAIL             AS Email
                                    FROM CLI_CLIENT_DATA_V
                                   WHERE ID_CONTRACT = @IdContract";
            return await conn.QueryAsync<ClientData>(sql , new { IdContract = id } );
        }
        public async Task<bool> AddClientAsync(ClientData client)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "CLI_CLIENT_DATA_PKG_INSERT";

            var parameters = new
            {
                i_ID_CLIENT = client.ID_CLIENT ?? Guid.NewGuid().ToString(),
                i_ID_CONTRACT = client.ID_CONTRACT,
                i_DENUMIRE_FISCALA = client.DenumireFiscala,
                i_CUI = client.Cui,
                i_NR_REG_COM = client.NrRegCom,
                i_SEDIU = client.Seduiu,
                i_JUDET = client.Judet,
                i_TARA = client.Tara,
                i_CONT_IBAN = client.ContIban,
                i_BANCA = client.Banca,
                i_TELEFON = client.Telefon,
                i_EMAIL = client.Email
            };

            var rows = await conn.ExecuteAsync(
                procedureName,
                parameters, 
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> UpdateClientAsync(ClientData client)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "CLI_CLIENT_DATA_PKG_UPDATE";
            var parameters = new
            {
                i_ID_CLIENT = client.ID_CLIENT,
                i_ID_CONTRACT = client.ID_CONTRACT,
                i_DENUMIRE_FISCALA = client.DenumireFiscala,
                i_CUI = client.Cui,
                i_NR_REG_COM = client.NrRegCom,
                i_SEDIU = client.Seduiu,
                i_JUDET = client.Judet,
                i_TARA = client.Tara,
                i_CONT_IBAN = client.ContIban,
                i_BANCA = client.Banca,
                i_TELEFON = client.Telefon,
                i_EMAIL = client.Email
            };

            var rows = await conn.ExecuteAsync(
                procedureName,
                parameters, 
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> DeleteClientAsync(string idClient)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "CLI_CLIENT_DATA_PKG_DELETE";
            var parameters = new { i_ID_CLIENT = idClient };

            var rows = await conn.ExecuteAsync(
                procedureName, 
                parameters, 
                commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
