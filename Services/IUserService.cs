using VirocGanpati.DTOs;
using VirocGanpati.Models;

namespace VirocGanpati.Services
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(string email, string password);
        Task<(int totalCount, IEnumerable<UserDto>)> GetAllUsersAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string role, bool enablePagination, int? projectId);
        Task<MandalDto> GetUserProjectAsync(int userId);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> AddUserAsync(AddUserDto userDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto);
        Task<UserDto> UpdateManagerAsync(int id, UpdateManagerDto userDto);
        Task DeleteUserAsync(int id);
        Task<bool> CheckIfEmailExists(string email);
        Task<bool> CheckIfMobileExists(string mobile);
        Task MarkOtpVerified(string mobile, bool status);
    }
}
