using VirocGanpati.DTOs.MandalAreas;

namespace VirocGanpati.Services
{
    public interface IMandalAreaService
    {
        Task<IEnumerable<MandalAreaDto>> GetAllAsync();
        Task<MandalAreaDto> GetByIdAsync(int id);
        Task<MandalAreaDto> CreateAsync(CreateMandalAreaDto dto);
        Task<MandalAreaDto> UpdateAsync(UpdateMandalAreaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
