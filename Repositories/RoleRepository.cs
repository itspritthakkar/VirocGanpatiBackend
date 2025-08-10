using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get users with pagination and search functionality
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            // Base query, filtering out deleted users
            var roles = await _context.Roles.ToListAsync();

            return roles;
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id);

            return role;
        }

        public async Task<Role?> GetRoleByIdentifierAsync(string identifier)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleIdentifier == identifier);

            return role;
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
