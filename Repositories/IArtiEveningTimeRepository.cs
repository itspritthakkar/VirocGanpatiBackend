using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IArtiEveningTimeRepository
    {
        Task<IEnumerable<ArtiEveningTime>> GetAllAsync();
        Task<ArtiEveningTime> GetByIdAsync(int id);
        Task<ArtiEveningTime> AddAsync(ArtiEveningTime entity);
        Task<ArtiEveningTime> UpdateAsync(ArtiEveningTime entity);
        Task<bool> DeleteAsync(int id);
    }
}
