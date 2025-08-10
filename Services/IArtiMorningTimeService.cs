using VirocGanpati.DTOs.ArtiMorningTimes;

namespace VirocGanpati.Services
{
    public interface IArtiMorningTimeService
    {
        Task<IEnumerable<ArtiMorningTimeDto>> GetAllAsync();
        Task<ArtiMorningTimeDto> GetByIdAsync(int id);
        Task<ArtiMorningTimeDto> CreateAsync(CreateArtiMorningTimeDto dto);
        Task<ArtiMorningTimeDto> UpdateAsync(UpdateArtiMorningTimeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
