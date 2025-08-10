using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IMandalRepository
    {
        Task<(int totalCount, IEnumerable<Mandal>)> GetMandalsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string status);  // Return total count and data
        Task<(int totalCount, IEnumerable<Mandal>)> GetAllMandalsAsync();  // Return total count and data
        Task<Mandal> GetMandalByIdAsync(int id);
        Task<List<string>> GetMandalSlugStartingWithAsync(string baseSlug);
        Task<Mandal> AddMandalAsync(Mandal project);
        Task<Mandal> UpdateMandalAsync(Mandal project);
        Task DeleteMandalAsync(int id);
        Task<bool> MandalNameExistsAsync(string projectName);
    }
}
