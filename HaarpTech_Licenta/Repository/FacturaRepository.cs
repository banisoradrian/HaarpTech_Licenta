using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using System.Data;

namespace HaarpTech_Licenta.Repository
{
    public interface IFacturaRepository
    {
        Task<Factura> GetByIdAsync(string idFactura);
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<IEnumerable<Factura>> GetAllByIdAsync(string id);
        Task<bool> AddFacturaAsync(Factura factura);
        Task<bool> DeleteFacturaAsync(string idFactura);
        Task<bool> AddElementFacturaAsync(ElementeFactura element);
        Task<FacturaCompletaViewModel> GetFacturaWithElementsByIdAsync(string idFactura);
        Task<FacturaCompletaViewModel> GetFacturaWithElementsAndClientDataByIdAsync(string idFactura);
    }

    public class FacturaRepository : IFacturaRepository
    {
        private readonly IDatabaseConnection _databaseConnection;

        public FacturaRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<Factura> GetByIdAsync(string idFactura)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT
                                        ID_FACTURA       AS ID_FACTURA,
                                        ID_CLIENT       AS ID_CLIENT,
                                        SERIA_FACTURA    AS SeriaFactura,
                                        NUMAR_FACTURA    AS NumarFactura,
                                        DATA_EMITERII    AS DataEmitere,
                                        MONEDA           AS Moneda,
                                        TOTAL_FARA_TVA   AS TotalFaraTva,
                                        TOTAL_TVA        AS TotalCuTva,
                                        TOTAL_FACTURA    AS TotalFactura
                                    FROM FACT_FACTURI_V
                                    WHERE ID_FACTURA = @IdFactura";

            return await conn.QueryFirstOrDefaultAsync<Factura>(sql, new { IdFactura = idFactura });
        }

        public async Task<IEnumerable<Factura>> GetAllAsync()
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT
                                        ID_FACTURA       AS ID_FACTURA,
                                        ID_CLIENT       AS ID_CLIENT,
                                        SERIA_FACTURA    AS SeriaFactura,
                                        NUMAR_FACTURA    AS NumarFactura,
                                        DATA_EMITERII    AS DataEmitere,
                                        MONEDA           AS Moneda,
                                        TOTAL_FARA_TVA   AS TotalFaraTva,
                                        TOTAL_TVA        AS TotalCuTva,
                                        TOTAL_FACTURA    AS TotalFactura
                                   FROM FACT_FACTURI_V";

            return await conn.QueryAsync<Factura>(sql);
        }

        public async Task<IEnumerable<Factura>> GetAllByIdAsync(string id)
        {
           using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"SELECT
                                        ID_FACTURA       AS ID_FACTURA,
                                        ID_CLIENT       AS ID_CLIENT,
                                        SERIA_FACTURA    AS SeriaFactura,
                                        NUMAR_FACTURA    AS NumarFactura,
                                        DATA_EMITERII    AS DataEmitere,
                                        MONEDA           AS Moneda,
                                        TOTAL_FARA_TVA   AS TotalFaraTva,
                                        TOTAL_TVA        AS TotalCuTva,
                                        TOTAL_FACTURA    AS TotalFactura
                                   FROM FACT_FACTURI
                                  WHERE ID_FACTURA = @IdFactura";
            return await conn.QueryAsync<Factura>(sql, new { IdFactura = id});

                                    
        }


public async Task<FacturaCompletaViewModel> GetFacturaWithElementsByIdAsync(string idFactura)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"
                                  -- 1) aduce capul facturii
                                  SELECT
                                    ID_FACTURA       AS ID_FACTURA,
                                    ID_CLIENT        AS ID_CLIENT,
                                    SERIA_FACTURA    AS SeriaFactura,
                                    NUMAR_FACTURA    AS NumarFactura,
                                    DATA_EMITERII    AS DataEmitere,
                                    MONEDA           AS Moneda,
                                    TOTAL_FARA_TVA   AS TotalFaraTva,
                                    TOTAL_TVA        AS TotalCuTva,
                                    TOTAL_FACTURA    AS TotalFactura
                                  FROM FACT_FACTURI
                                  WHERE ID_FACTURA = @IdFactura;

                                  -- 2) aduce liniile
                                  SELECT
                                    ID_ELEMENT_FACTURA   AS ID_ELEMENT_FACTURA,
                                    ID_FACTURA           AS ID_FACTURA,
                                    NR_CRT               AS NrCrt,
                                    DENUMIRE_PRODUS      AS DenumireProdus,
                                    UM                   AS Um,
                                    CANTITATE            AS Cantitate,
                                    VALOARE_FARA_TVA     AS ValoareFaraTva,
                                    VALOARE_TVA          AS ValoareCuTva
                                  FROM FACT_ELEMENTE_FACTURA
                                  WHERE ID_FACTURA = @IdFactura
                                  ORDER BY NR_CRT;";

            using var multi = await conn.QueryMultipleAsync(sql, new { IdFactura = idFactura });

            var fact = await multi.ReadFirstOrDefaultAsync<Factura>();
            var elements = (fact != null)
                ? (await multi.ReadAsync<ElementeFactura>()).ToList()
                : new List<ElementeFactura>();

            return new FacturaCompletaViewModel
            {
                Factura = fact,
                ElementeFactura = elements
            };
        }


        public async Task<FacturaCompletaViewModel> GetFacturaWithElementsAndClientDataByIdAsync(string idFactura)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string sql = @"
        -- 1) Fetch invoice header with client data
        SELECT
            f.ID_FACTURA,
            f.ID_CLIENT,
            f.SERIA_FACTURA    AS SeriaFactura,
            f.NUMAR_FACTURA    AS NumarFactura,
            f.DATA_EMITERII    AS DataEmitere,
            f.MONEDA           AS Moneda,
            f.TOTAL_FARA_TVA   AS TotalFaraTva,
            f.TOTAL_TVA        AS TotalCuTva,
            f.TOTAL_FACTURA    AS TotalFactura,
            c.ID_CLIENT        AS ID_CLIENT,
            c.ID_CONTRACT      AS ID_CONTRACT,
            c.DENUMIRE_FISCALA AS DenumireFiscala,
            c.CUI              AS Cui,
            c.NR_REG_COM       AS NrRegCom,
            c.SEDIU            AS Sediu,
            c.JUDET            AS Judet,
            c.TARA             AS Tara,
            c.CONT_IBAN        AS ContIban,
            c.BANCA            AS Banca,
            c.TELEFON          AS Telefon,
            c.EMAIL            AS Email
        FROM FACT_FACTURI_V f
        INNER JOIN CLI_CLIENT_DATA_V c ON f.ID_CLIENT = c.ID_CLIENT
        WHERE f.ID_FACTURA = @IdFactura;

        -- 2) Fetch invoice lines
        SELECT
            ID_ELEMENT_FACTURA   AS ID_ELEMENT_FACTURA,
            ID_FACTURA           AS ID_FACTURA,
            NR_CRT               AS NrCrt,
            DENUMIRE_PRODUS      AS DenumireProdus,
            UM                   AS Um,
            CANTITATE            AS Cantitate,
            VALOARE_FARA_TVA     AS ValoareFaraTva,
            VALOARE_TVA          AS ValoareCuTva
        FROM FACT_ELEMENTE_FACTURA
        WHERE ID_FACTURA = @IdFactura
        ORDER BY NR_CRT; ";

            using var multi = await conn.QueryMultipleAsync(sql, new { IdFactura = idFactura });

          
            var header = (await multi.ReadAsync<dynamic>()).FirstOrDefault();
            if (header == null)
                return null;

            var factura = new Factura
            {
                ID_FACTURA = header.ID_FACTURA,
                ID_CLIENT = header.ID_CLIENT,
                SeriaFactura = header.SeriaFactura,
                NumarFactura = header.NumarFactura,
                DataEmitere = header.DataEmitere,
                Moneda = header.Moneda,
                TotalFaraTva = header.TotalFaraTva,
                TotalCuTva = header.TotalCuTva,
                TotalFactura = header.TotalFactura
            };

            var client = new ClientData
            {
                ID_CLIENT = header.ID_CLIENT,
                ID_CONTRACT = header.ID_CONTRACT,
                DenumireFiscala = header.DenumireFiscala,
                Cui = header.Cui,
                NrRegCom = header.NrRegCom,
                Seduiu = header.Sediu,
                Judet = header.Judet,
                Tara = header.Tara,
                ContIban = header.ContIban,
                Banca = header.Banca,
                Telefon = header.Telefon,
                Email = header.Email
            };

            // 2) Citim liniile facturii
            var lines = (await multi.ReadAsync<ElementeFactura>()).ToList();

            // 3) Returnam ViewModel-ul
            return new FacturaCompletaViewModel
            {
                Factura = factura,
                Client = client,
                ElementeFactura = lines
            };
        }



        public async Task<bool> AddFacturaAsync(Factura factura)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            const string procedureName = "FACT_FACTURI_PKG_INSERT";

            var parameters = new
            {
                i_ID_FACTURA = factura.ID_FACTURA ?? Guid.NewGuid().ToString(),
                i_ID_CLIENT = factura.ID_CLIENT,
                i_SERIA_FACTURA = factura.SeriaFactura,
                i_NUMAR_FACTURA = factura.NumarFactura,
                i_DATA_EMITERII = factura.DataEmitere,
                i_MONEDA = factura.Moneda,
                i_TOTAL_FARA_TVA = factura.TotalFaraTva,
                i_TOTAL_TVA = factura.TotalCuTva,
                i_TOTAL_FACTURA = factura.TotalFactura
            };

            var rows = await conn.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return rows > 0;
        }

        public async Task<bool> AddElementFacturaAsync(ElementeFactura element)
        {
            try
            {
                using var conn = _databaseConnection.GetConnection(); // SqlConnection
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                const string sql = @"
                INSERT INTO FACT_ELEMENTE_FACTURA (
                    ID_ELEMENT_FACTURA, ID_FACTURA, NR_CRT, DENUMIRE_PRODUS, UM, CANTITATE, VALOARE_FARA_TVA, VALOARE_TVA
                ) VALUES (
                    @ID_ELEMENT_FACTURA, @ID_FACTURA, @NR_CRT, @DENUMIRE_PRODUS, @UM, @CANTITATE, @VALOARE_FARA_TVA, @VALOARE_TVA
                )";

                var parameters = new
                {
                    element.ID_ELEMENT_FACTURA,
                    element.ID_FACTURA,
                    NR_CRT = element.NrCrt,
                    DENUMIRE_PRODUS = element.DenumireProdus,
                    UM = element.Um,
                    CANTITATE = element.Cantitate,
                    VALOARE_FARA_TVA = element.ValoareFaraTva,
                    VALOARE_TVA = element.ValoareCuTva
                };

                var rows = await conn.ExecuteAsync(sql, parameters);
                return rows > 0;
            }
            catch (Exception ex)
            {
                // Poți loga excepția aici sau o poți propaga mai departe
                Console.WriteLine($"Eroare la salvarea elementului facturii: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteFacturaAsync(string idFactura)
        {
            try
            {
                using var conn = _databaseConnection.GetConnection();
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                const string procedureName = "FACT_FACTURI_PKG_DELETE";
                var parameters = new { i_ID_FACTURA = idFactura };

                var rows = await conn.ExecuteAsync(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);
                return rows > 0;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

   


    }
}