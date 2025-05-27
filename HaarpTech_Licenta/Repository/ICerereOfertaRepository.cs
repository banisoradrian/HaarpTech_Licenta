using HaarpTech_Licenta.Models;

namespace HaarpTech_Licenta.Repository
{
    public interface ICerereOfertaRepository
    {
        Task<IEnumerable<CerereOferta>> GetAllAsync();
        Task<CerereOferta> GetByIdAsync(string idCerereOferta);
        Task<bool> UpdateCerereOfertaAsync(CerereOferta cereriOferta);
        Task<bool> DeleteCerereOfertaAsync(string idCereOferta);
        Task<bool> AddCerereOfertaAsync(CerereOferta cereriOferta);

    }
}
