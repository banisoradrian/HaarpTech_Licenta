using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace HaarpTech_Licenta.Repository
{
    public class CerereOfertaRepository : ICerereOfertaRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CerereOfertaRepository(IDatabaseConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddCerereOfertaAsync(CerereOferta cereriOferta)
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext!
                             .User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var connection = _dbConnection.GetConnection())
                {
                    var parameter = new
                    {
                        i_ID_CERERE = Guid.NewGuid().ToString(),
                        i_ID_USER = cereriOferta.ID_USER ,
                        i_DENUMIRE_CERERE = cereriOferta.DenumireCerere,
                        i_DATA_CERERI = cereriOferta.DataCereri,
                        i_PRET = cereriOferta.Pret,
                        i_DESCRIERE = cereriOferta.DenumireCerere,
                        i_STATUS_CERERE = cereriOferta.StatusCerere,
                        i_TIP_APLICATIE = cereriOferta.TipAplicatie,
                        i_SERVICII_AI = cereriOferta.ServiciiAI,
                        i_NIVEL_DE_SECURITATE = cereriOferta.NivelDeSecuritate,
                        i_BAZA_DE_DATE = cereriOferta.BazaDeDate,
                        i_TIP_LOGARE = cereriOferta.TipLogare,
                        i_SERVICII_CLOUD = cereriOferta.ServiciiCloud,
                        i_TIP_ACCESS = cereriOferta.TipAccess,
                        i_INTEGRARE_CU_ALTE_SISTEME = cereriOferta.IntegrareCuAlteSisteme,
                        i_ACCESIBILITATE = cereriOferta.Accesibilitate,
                        i_CONTURI_LOGATE = cereriOferta.ConturiLogate,
                        i_TIMP_DE_REALIZARE = cereriOferta.TimpDeRealizare,
                        i_TEHNOLOGIE = cereriOferta.Tehnologie,
                        i_SPECIFICATII_TEHNICE = cereriOferta.SpecificatiiTehnice,
                        i_BUGET_ESTIMAT = cereriOferta.BugetEstimat,
                        i_DATA_OFERTEI = cereriOferta.DataOfertei,
                        i_VOLUM_DE_DATE = cereriOferta.VolumDeDate,
                        i_STATUS_OFERTA = cereriOferta.StatusOferta,
                        i_TIP_OFERTA = cereriOferta.TipOferta
                    };

                    int affectedRows = await connection.ExecuteAsync(
                        "CRO_CERERI_OFERTE_PKG_INSERT",
                        parameter,
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

        public async Task<IEnumerable<CerereOferta>> GetAllAsync()
        {
            using var connection = _dbConnection.GetConnection();
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (connection.State != ConnectionState.Open)
                connection.Open();

                string sql = @"
                                 SELECT
                                       ID_CERERE			      
                                       ,ID_USER					  
                                       ,DENUMIRE_CERERE          AS DenumireCerere
                                       ,DATA_CERERI				 AS	DataCereri
                                       ,PRET				     AS Pret
                                       ,DESCRIERE				 AS	Descriere
                                       ,STATUS_CERERE			 AS	StatusCerere
                                       ,TIP_APLICATIE			 AS	TipAplicatie
                                       ,SERVICII_AI				 AS	ServiciiAi
                                       ,NIVEL_DE_SECURITATE		 AS	NivelDeSecuritate
                                       ,BAZA_DE_DATE				 AS BazaDeDate
                                       ,TIP_LOGARE				 AS	TipLogare
                                       ,SERVICII_CLOUD			 AS	ServiciiCloud
                                       ,TIP_ACCESS				 AS	TipAccess
                                       ,INTEGRARE_CU_ALTE_SISTEME AS IntegrareCuAlteSisteme
                                       ,ACCESIBILITATE		     AS	Accesibilitate
                                       ,CONTURI_LOGATE			 AS ConturiLogate
                                       ,TIMP_DE_REALIZARE		 AS	TimpDeRealizare
                                       ,TEHNOLOGIE				 AS	Tehnologie
                                       ,SPECIFICATII_TEHNICE		 AS SpecificatiiTehnice
                                       ,BUGET_ESTIMAT			 AS	BugetEstimat
                                       ,DATA_OFERTEI				 AS DataOfertei
                                       ,VOLUM_DE_DATE			 AS	VolumDeDate
                                       ,STATUS_OFERTA			 AS	StatusOferta
                                       ,TIP_OFERTA                AS TipOferta
                                   FROM CRO_CERERI_OFERTE_V";
                return await connection.QueryAsync<CerereOferta>(sql);
        }

        public async Task<CerereOferta> GetByIdAsync(string idCerereOferta)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"
                                 SELECT
                                        ID_CERERE			      	
                                       ,ID_USER					  
                                       ,DENUMIRE_CERERE           AS DenumireCerere
                                       ,DATA_CERERI				  AS DataCereri
                                       ,PRET				      AS Pret
                                       ,DESCRIERE				  AS Descriere
                                       ,STATUS_CERERE			  AS StatusCerere
                                       ,TIP_APLICATIE			  AS TipAplicatie
                                       ,SERVICII_AI				  AS ServiciiAi
                                       ,NIVEL_DE_SECURITATE		  AS NivelDeSecuritate
                                       ,BAZA_DE_DATE		      AS BazaDeDate
                                       ,TIP_LOGARE				  AS TipLogare
                                       ,SERVICII_CLOUD			  AS ServiciiCloud
                                       ,TIP_ACCESS				  AS TipAccess
                                       ,INTEGRARE_CU_ALTE_SISTEME AS IntegrareCuAlteSisteme
                                       ,ACCESIBILITATE		      AS Accesibilitate
                                       ,CONTURI_LOGATE			  AS ConturiLogate
                                       ,TIMP_DE_REALIZARE		  AS TimpDeRealizare
                                       ,TEHNOLOGIE				  AS Tehnologie
                                       ,SPECIFICATII_TEHNICE	  AS SpecificatiiTehnice
                                       ,BUGET_ESTIMAT			  AS BugetEstimat
                                       ,DATA_OFERTEI		      AS DataOfertei
                                       ,VOLUM_DE_DATE			  AS VolumDeDate
                                       ,STATUS_OFERTA			  AS StatusOferta
                                       ,TIP_OFERTA                AS TipOferta
                                   FROM CRO_CERERI_OFERTE_V
                                   WHERE ID_CERERE = @IdCerere";
            return await conn.QueryFirstOrDefaultAsync<CerereOferta>(
                sql,
                new { IdCerere = idCerereOferta }
            );
        }

        public async Task<bool> UpdateCerereOfertaAsync(CerereOferta cereriOferta)
        {
            using var conn = _dbConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            var parameter = new
            {

               i_ID_CERERE = cereriOferta.ID_CERERE
              ,i_ID_USER = cereriOferta.ID_USER
              ,i_DENUMIRE_CERERE = cereriOferta.DenumireCerere
              ,i_DATA_CERERI = cereriOferta.DataCereri
              ,i_PRET = cereriOferta.Pret
              ,i_DESCRIERE = cereriOferta.DenumireCerere
              ,i_STATUS_CERERE = cereriOferta.StatusCerere
              ,i_TIP_APLICATIE = cereriOferta.TipAplicatie
              ,i_SERVICII_AI = cereriOferta.ServiciiAI
              ,i_NIVEL_DE_SECURITATE = cereriOferta.NivelDeSecuritate
              ,i_BAZA_DE_DATE = cereriOferta.BazaDeDate
              ,i_TIP_LOGARE = cereriOferta.TipLogare
              ,i_SERVICII_CLOUD = cereriOferta.ServiciiCloud
              ,i_TIP_ACCESS = cereriOferta.TipAccess
              ,i_INTEGRARE_CU_ALTE_SISTEME = cereriOferta.IntegrareCuAlteSisteme
              ,i_ACCESIBILITATE = cereriOferta.Accesibilitate
              ,i_CONTURI_LOGATE = cereriOferta.ConturiLogate
              ,i_TIMP_DE_REALIZARE = cereriOferta.TimpDeRealizare
              ,i_TEHNOLOGIE = cereriOferta.Tehnologie
              ,i_SPECIFICATII_TEHNICE = cereriOferta.SpecificatiiTehnice
              ,i_BUGET_ESTIMAT = cereriOferta.BugetEstimat
              ,i_DATA_OFERTEI = cereriOferta.DataOfertei
              ,i_VOLUM_DE_DATE = cereriOferta.VolumDeDate
              ,i_STATUS_OFERTA = cereriOferta.StatusOferta
              ,i_TIP_OFERTA = cereriOferta.TipOferta
            };
            int affected = 0;
            try
            {
              affected = await conn.ExecuteAsync(
                "CRO_CERERI_OFERTE_PKG_UPDATE",
                parameter,
                commandType: CommandType.StoredProcedure
            );
            }
            catch(Exception ex)
            {
                throw;
            }

            return affected > 0;
        }

        public async Task<bool> DeleteCerereOfertaAsync(string idCereOferta)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                var affected = await conn.ExecuteScalarAsync<int>(
                    "CRO_CERERI_OFERTE_PKG_DELETE",
                    new { i_ID_CERERE = idCereOferta },
                    commandType: CommandType.StoredProcedure
                );
                return affected > 0;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<bool> AddSedintaAsync(Sedinta sedinta)
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
    }
}
