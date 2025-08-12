using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get users with pagination and search functionality
        public async Task<(int totalCount, IEnumerable<User>)> GetAllUsersAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string role, bool enablePagination, int? projectId)
        {
            // Base query, filtering out deleted users
            var query = _context.Users
                                .Include(u => u.Role)
                                .Include(u => u.Mandal)
                                .Where(u => !u.IsDeleted)
                                .AsQueryable();

            if (projectId != null & projectId != 0)
            {
                query = query.Where(u => u.MandalId == projectId);
            }

            // Apply search filtering (if searchValue is provided)
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(u => u.FirstName.Contains(searchValue) ||
                                         u.LastName.Contains(searchValue) ||
                                         u.Email.Contains(searchValue) ||
                                         u.Role.RoleName.Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(u => u.Role.RoleIdentifier == role);
            }

            query = ApplySorting(query, sortField, sortOrder);
            // Get the total number of users before pagination
            int totalCount = await query.CountAsync();

            if (enablePagination)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            // Apply pagination
            var users = await query.ToListAsync();

            return (totalCount, users);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Mandal)
                .FirstOrDefaultAsync(u => u.UserId == id && !u.IsDeleted);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Mandal)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User?> GetUserByMobileAsync(string mobile)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Mandal)
                .FirstOrDefaultAsync(u => u.Mobile == mobile && !u.IsDeleted);
        }

        public async Task<Mandal?> GetUserProjectAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);

            var project = await _context.Mandals.Where(p => p.MandalId == user.MandalId).FirstOrDefaultAsync();

            return project;
        }

        public async Task<User> AddUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                //user.IsDeleted = true;  // Soft delete
                //user.UpdatedAt = DateTime.UtcNow;
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkUserLoggin(User user) 
        {
            user.LastLoginAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField)) sortField = "UpdatedAt"; // Default sort field
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "desc"; // Default sort order

            switch (sortField.ToLower())
            {
                case "name":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.FirstName) : query.OrderByDescending(w => w.FirstName);
                    break;
                case "mobile":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.Email) : query.OrderByDescending(w => w.Email);
                    break;
                case "role":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.Role.RoleName) : query.OrderByDescending(w => w.Role.RoleName);
                    break;
                case "status":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.Status) : query.OrderByDescending(w => w.Status);
                    break;
                default:
                    query = query.OrderByDescending(w => w.UpdatedAt); // Default sorting by UpdatedAt in descending order
                    break;
            }

            return query;
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            User? user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public async Task<bool> CheckIfMobileExists(string mobile)
        {
            User? user = await _context.Users.Where(u => u.Mobile == mobile).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task MarkOtpVerified(string mobile, bool status)
        {
            User? user = await _context.Users.Where(u => u.Mobile == mobile).FirstOrDefaultAsync();

            if (user != null)
            {
                user.IsMobileVerified = status;
                await UpdateUserAsync(user);
            }
        }
    }
}
