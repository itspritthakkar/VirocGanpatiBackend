using VirocGanpati.DTOs;

namespace VirocGanpati.Services
{
    public interface IMandalService
    {
        Task<(int totalElements, IEnumerable<MandalDto> data)> GetMandalsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string status);
        Task<(int totalElements, IEnumerable<MandalDto> data)> GetAllMandalsAsync();
        Task<MandalDto> GetMandalByIdAsync(int id);
        Task<MandalDto> AddMandalAsync(AddMandalDto projectDto);  // Takes AddMandalDto as input
        Task<MandalDto> UpdateMandalAsync(int id, UpdateMandalDto projectDto);  // Update with DTO
        Task DeleteMandalAsync(int id);
    }
}
