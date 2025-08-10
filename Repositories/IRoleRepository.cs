using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdentifierAsync(string identifier);
        Task<Role> AddRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int id);
    }
}
