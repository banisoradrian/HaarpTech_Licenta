using Dapper;
using HaarpTech_Licenta.Data;
using HaarpTech_Licenta.Models;
using System.Data;

namespace HaarpTech_Licenta.Repository
{
    public interface IAngajatRepository
    {
        Task<Angajat> GetByIdAsync(string id);
        Task<IEnumerable<Angajat>> GetAllAsync();
        Task<bool> AddAngajatAsync(Angajat angajat);
        Task<bool> UpdateAngajatAsync(Angajat angajat);
        Task<bool> DeleteAngajatAsync(string idAngajat);
        //Task<IEnumerable<StatusTichet>> GetAllWithTichetAsync(string id);
        //Task<IEnumerable<StatusTichet>> GetAllByIdAsync(string id);
        Task<IEnumerable<Angajat>> SearchAngajatiAsync(string searchTerm);
    }
    public class AngajatRepository : IAngajatRepository
    {

        private readonly IDatabaseConnection _databaseConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AngajatRepository(IDatabaseConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _databaseConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Angajat> GetByIdAsync(string id)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            const string sql = @"SELECT 
                                         ID_ANGAJAT					
                                        ,NUME           AS Nume				
                                        ,PRENUME		AS Prenume		
                                        ,EMAIL			AS Email
                                        ,DATA_ANGAJARE	AS DataAngajare	
                                        ,CNP			AS Cnp
                                        ,SERIE_NR		AS SerieNr		
                                        ,LOCALITATE		AS Localitate
                                        ,TARA			AS Tara	
                                        ,SALARIU		AS Salariu		
                                        ,POZITIE		AS Pozitie	
                                        ,DEPARTAMENT	AS Departament	  
                                        ,TELEFON        AS Telefon
                                    FROM ANG_ANGAJATI_V
                                   WHERE ID_ANGAJAT = @IdAngajat";
            return await conn.QueryFirstOrDefaultAsync<Angajat>(
                sql,
                new { IdAngajat = id }
            );
        }

        public async Task<IEnumerable<Angajat>> GetAllAsync()
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            const string sql = @"SELECT 
                                         ID_ANGAJAT					
                                        ,NUME           AS Nume				
                                        ,PRENUME		AS Prenume		
                                        ,EMAIL			AS Email
                                        ,DATA_ANGAJARE	AS DataAngajare	
                                        ,CNP			AS Cnp
                                        ,SERIE_NR		AS SerieNr		
                                        ,LOCALITATE		AS Localitate
                                        ,TARA			AS Tara	
                                        ,SALARIU		AS Salariu		
                                        ,POZITIE		AS Pozitie	
                                        ,DEPARTAMENT	AS Departament	
                                        ,TELEFON        AS Telefon
                                    FROM ANG_ANGAJATI_V";
            return await conn.QueryAsync<Angajat>(sql);
        }

        public async Task<bool> AddAngajatAsync(Angajat angajat)
        {
            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    string procedureName = "ANG_ANGAJATI_PKG_INSERT";
                    var parameter = new
                    {
                      i_ID_ANGAJAT = Guid.NewGuid().ToString(),
                      i_NUME = angajat.Nume,
                      i_PRENUME = angajat.Prenume,
                      i_EMAIL = angajat.Email,
                      i_DATA_ANGAJARE = angajat.DataAngajare,
                      i_CNP = angajat.Cnp,
                      i_SERIE_NR = angajat.SerieNr,
                      i_LOCALITATE = angajat.Localitate,
                      i_TARA = angajat.Tara,
                      i_SALARIU = angajat.Salariu,
                      i_POZITIE = angajat.Pozitie,
                      i_DEPARTAMENT = angajat.Departament,
                      i_TELEFON = angajat.Telefon
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

        public async Task<bool> UpdateAngajatAsync(Angajat angajat)
        {
            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    string procedureName = "ANG_ANGAJATI_PKG_UPDATE";
                    var parameter = new
                    {
                        i_ID_ANGAJAT = angajat.ID_ANGAJAT,
                        i_NUME = angajat.Nume,
                        i_PRENUME = angajat.Prenume,
                        i_EMAIL = angajat.Email,
                        i_DATA_ANGAJARE = angajat.DataAngajare,
                        i_CNP = angajat.Cnp,
                        i_SERIE_NR = angajat.SerieNr,
                        i_LOCALITATE = angajat.Localitate,
                        i_TARA = angajat.Tara,
                        i_SALARIU = angajat.Salariu,
                        i_POZITIE = angajat.Pozitie,
                        i_DEPARTAMENT = angajat.Departament,
                        i_TELEFON = angajat.Telefon
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

        public async Task<bool> DeleteAngajatAsync(string idAngajat)
        {
            try
            {
                using var conn = _databaseConnection.GetConnection();
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                int affected = await conn.ExecuteScalarAsync<int>(
                    "ANG_ANGAJATI_PKG_DELETE",
                    new { i_ID_ANGAJAT = idAngajat},
                    commandType: CommandType.StoredProcedure
                );
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Angajat>> SearchAngajatiAsync(string searchTerm)
        {
            using var conn = _databaseConnection.GetConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            string sql = @"
                SELECT
                    ID_ANGAJAT,
                    NUME AS Nume,
                    PRENUME AS Prenume,
                    EMAIL AS Email,
                    DATA_ANGAJARE AS DataAngajare,
                    CNP AS Cnp,
                    SERIE_NR AS SerieNr,
                    LOCALITATE AS Localitate,
                    TARA AS Tara,
                    SALARIU AS Salariu,
                    POZITIE AS Pozitie,
                    DEPARTAMENT AS Departament,
                    TELEFON AS Telefon
                FROM ANG_ANGAJATI_V
                WHERE UPPER(NUME) LIKE UPPER('%' || @SearchTerm || '%')
                   OR UPPER(PRENUME) LIKE UPPER('%' || @SearchTerm || '%')
                   OR UPPER(EMAIL) LIKE UPPER('%' || @SearchTerm || '%')"; // Căutare pe Nume, Prenume, Email

            return await conn.QueryAsync<Angajat>(
                sql,
                new { SearchTerm = searchTerm }
            );
        }
    }
}

