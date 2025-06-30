using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using System.Data;


namespace HaarpTech_Licenta.Repository
{
    public interface IContractRepository
    {
        Task<Contract> GetByIdAsync(string idContract);
        Task<IEnumerable<Contract>> GetAllAsync();
        Task<IEnumerable<Contract>> GetAllByIdAsync(string id);
        Task<bool> AddContractAsync(Contract contract);
        Task<bool> UpdateContractAsync(Contract contract);
        Task<bool> DeleteContractAsync(string idContract);
    }

    public class ContractRepository : IContractRepository
    {
        private readonly IDatabaseConnection _databaseConnection;

        public ContractRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<Contract> GetByIdAsync(string idContract)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @" SELECT
                                        ID_CONTRACT,
                                        ID_CERERE,
                                        NUMAR_CONTRACT              AS NumarContract,
                                        NUME_BENEFICIAR             AS NumeBeneficiar,
                                        DATA_CONTRACT               AS DataContract
                                   FROM CON_CONTRACTE_V
                                  WHERE ID_CONTRACT = @IdContract";
            return await conn.QueryFirstOrDefaultAsync<Contract>(sql, new { IdContract = idContract });
        }

        public async Task<IEnumerable<Contract>> GetAllAsync()
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @" SELECT
                                        ID_CONTRACT,
                                        ID_CERERE,
                                        NUMAR_CONTRACT              AS NumarContract,
                                        NUME_BENEFICIAR             AS NumeBeneficiar,
                                        DATA_CONTRACT               AS DataContract
                                   FROM CON_CONTRACTE_V";
            return await conn.QueryAsync<Contract>(sql);
        }


        public async Task<IEnumerable<Contract>> GetAllByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @" SELECT
                                        ID_CONTRACT,
                                        ID_CERERE,
                                        NUMAR_CONTRACT              AS NumarContract,
                                        NUME_BENEFICIAR             AS NumeBeneficiar,
                                        DATA_CONTRACT               AS DataContract
                                   FROM CON_CONTRACTE_V
                                   WHERE ID_CERERE = @IdCerere";
            return await conn.QueryAsync<Contract>( sql,new { IdCerere = id });
        }



        public async Task<bool> AddContractAsync(Contract contract)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            const string procedureName = "CON_CONTRACTE_PKG_INSERT";

            DateTime dataActuala = DateTime.Now;
            string firstLetters = contract.NumeBeneficiar.Substring(0, 4);
            string numarContract = $"C_{dataActuala.Day}_{dataActuala.Month}_{dataActuala.Year}_{firstLetters}";
            var parameters = new
            {
                i_ID_CONTRACT = contract.ID_CONTRACT ?? Guid.NewGuid().ToString(),
                i_ID_CERERE = contract.ID_CERERE,
                i_NUMAR_CONTRACT = numarContract,
                i_NUME_BENEFICIAR = contract.NumeBeneficiar,
                i_DATA_CONTRACT = dataActuala,

            };

            var rows = await conn.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> UpdateContractAsync(Contract contract)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "CON_CONTRACTE_PKG_UPDATE";
            var parameters = new
            {
                i_ID_CONTRACT = contract.ID_CONTRACT,
                i_ID_CERERE = contract.ID_CERERE,
                i_NUMAR_CONTRACT = contract.NumarContract,
                i_NUME_BENEFICIAR = contract.NumeBeneficiar,
                i_DATA_CONTRACT = contract.DataContract,
            };

            var rows = await conn.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> DeleteContractAsync(string idContract)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "CON_CONTRACTE_PKG_DELETE";
            var parameters = new { i_ID_CONTRACT = idContract };

            var rows = await conn.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }
    }
}
