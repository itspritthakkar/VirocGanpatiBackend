using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByMobileAsync(string mobile);
        Task<Mandal?> GetUserProjectAsync(int userId);
        Task<(int totalCount, IEnumerable<User>)> GetAllUsersAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string role, bool enablePagination, int? projectId);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> UserExistsAsync(string email);
        Task MarkUserLoggin(User user);
        Task DeleteUserAsync(int id);
        Task<bool> CheckIfEmailExists(string email);
        Task<bool> CheckIfMobileExists(string email);
        Task MarkOtpVerified(string mobile, bool status);
    }
}
