using VirocGanpati.DTOs.ArtiEveningTime;

namespace VirocGanpati.Services
{
    public interface IArtiEveningTimeService
    {
        Task<IEnumerable<ArtiEveningTimeDto>> GetAllAsync();
        Task<ArtiEveningTimeDto> GetByIdAsync(int id);
        Task<ArtiEveningTimeDto> CreateAsync(CreateArtiEveningTimeDto dto);
        Task<ArtiEveningTimeDto> UpdateAsync(UpdateArtiEveningTimeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
