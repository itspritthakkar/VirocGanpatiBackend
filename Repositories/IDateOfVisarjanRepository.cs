using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IDateOfVisarjanRepository
    {
        Task<IEnumerable<DateOfVisarjan>> GetAllAsync();
        Task<DateOfVisarjan> GetByIdAsync(int id);
        Task<DateOfVisarjan> AddAsync(DateOfVisarjan entity);
        Task<DateOfVisarjan> UpdateAsync(DateOfVisarjan entity);
        Task<bool> DeleteAsync(int id);
    }
}
