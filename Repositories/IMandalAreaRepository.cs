using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IMandalAreaRepository
    {
        Task<IEnumerable<MandalArea>> GetAllAsync();
        Task<MandalArea> GetByIdAsync(int id);
        Task<MandalArea> AddAsync(MandalArea mandalArea);
        Task<MandalArea> UpdateAsync(MandalArea mandalArea);
        Task<bool> DeleteAsync(int id);
    }
}
