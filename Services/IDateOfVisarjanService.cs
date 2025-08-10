using VirocGanpati.DTOs.DateOfVisarjans;

namespace VirocGanpati.Services
{
    public interface IDateOfVisarjanService
    {
        Task<IEnumerable<DateOfVisarjanDto>> GetAllAsync();
        Task<DateOfVisarjanDto> GetByIdAsync(int id);
        Task<DateOfVisarjanDto> CreateAsync(CreateDateOfVisarjanDto dto);
        Task<DateOfVisarjanDto> UpdateAsync(UpdateDateOfVisarjanDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
