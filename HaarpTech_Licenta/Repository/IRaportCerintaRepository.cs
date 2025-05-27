using HaarpTech_Licenta.Models;

namespace HaarpTech_Licenta.Repository
{
    public interface IRaportCerintaRepository
    {
        Task<bool> AddRaportCerintaAsync(RaportCerinta raportCerinta);
        Task<IEnumerable<RaportCerinta>> GetAllAsync();
        Task<RaportCerinta> GetByIdAsync(string idRaportCerinta);
        Task<bool> UpdateRaportCerintaAsync(RaportCerinta raportCerinta);
        Task<bool> DeleteRaportCerintaAsync(string idRaportCerinta);
    }
}
