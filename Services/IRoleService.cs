using VirocGanpati.DTOs;

namespace VirocGanpati.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto?> GetRoleByIdentifierAsync(string identifier);
        Task<RoleDto> AddRoleAsync(AddRoleDto roleDto);
        Task<RoleDto?> UpdateRoleAsync(int id, AddRoleDto roleDto);
        Task DeleteRoleAsync(int id);
    }
}
