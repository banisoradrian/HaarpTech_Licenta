using HaarpTech_Licenta.Models;

namespace HaarpTech_Licenta.Repository
{
    public interface ISedintaRepository
    {
        Task<bool> AddSedintaAsync(Sedinta sedinta);
        Task<IEnumerable<Sedinta>> GetAllAsync();
        Task<Sedinta> GetByIdAsync(string idSedinta);
        Task<bool> UpdateSedintaAsync(Sedinta sedinta);
        Task<bool> DeleteSedintaAsync(string idSedinta);
    }
}
