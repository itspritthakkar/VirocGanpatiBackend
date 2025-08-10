using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IArtiMorningTimeRepository
    {
        Task<IEnumerable<ArtiMorningTime>> GetAllAsync();
        Task<ArtiMorningTime> GetByIdAsync(int id);
        Task<ArtiMorningTime> AddAsync(ArtiMorningTime entity);
        Task<ArtiMorningTime> UpdateAsync(ArtiMorningTime entity);
        Task<bool> DeleteAsync(int id);
    }
}
