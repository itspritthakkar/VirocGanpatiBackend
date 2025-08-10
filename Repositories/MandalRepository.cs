using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class MandalRepository : IMandalRepository
    {
        private readonly ApplicationDbContext _context;

        public MandalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(int totalCount, IEnumerable<Mandal>)> GetMandalsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string status)
        {
            var query = _context.Mandals
                                .Include(p => p.Updater)
                                .Where(p => !p.IsDeleted)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(p => p.MandalName.Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(status))
            {
                _ = Enum.TryParse(status, out ActiveInactiveStatus statusEnum);

                query = query.Where(p => p.Status == statusEnum);
            }

            query = ApplySorting(query, sortField, sortOrder);

            int totalCount = await query.CountAsync();  // Get total count before pagination
            var projects = await query
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            return (totalCount, projects);
        }

        public async Task<(int totalCount, IEnumerable<Mandal>)> GetAllMandalsAsync()
        {
            var query = _context.Mandals
                                .Include(p => p.Updater)
                                .Where(p => !p.IsDeleted)
                                .AsQueryable();

            int totalCount = await query.CountAsync();  // Get total count before pagination
            var projects = await query.ToListAsync();

            return (totalCount, projects);
        }

        public async Task<Mandal> GetMandalByIdAsync(int id)
        {
            return await _context.Mandals
                                 .Include(p => p.Updater)
                                 .FirstOrDefaultAsync(p => p.MandalId == id && !p.IsDeleted);
        }

        public async Task<List<string>> GetMandalSlugStartingWithAsync(string baseSlug)
        {
            return await _context.Mandals
                .Where(m => m.MandalSlug.StartsWith(baseSlug))
                .Select(m => m.MandalSlug)
                .ToListAsync();
        }

        public async Task<Mandal> AddMandalAsync(Mandal mandal)
        {
            mandal.CreatedAt = DateTime.UtcNow;
            mandal.UpdatedAt = DateTime.UtcNow;
            _context.Mandals.Add(mandal);
            await _context.SaveChangesAsync();
            return mandal;
        }

        public async Task<Mandal> UpdateMandalAsync(Mandal project)
        {
            project.UpdatedAt = DateTime.UtcNow;
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return project;
        }

        //public async Task DeleteMandalAsync(int id)
        //{
        //    var mandal = await _context.Mandals.FindAsync(id);
        //    if (mandal != null)
        //    {
        //        mandal.IsDeleted = true;
        //        mandal.UpdatedAt = DateTime.UtcNow;
        //        _context.Entry(mandal).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<bool> IsMandalAssignedToAnyUserAsync(int mandalId)
        {
            return await _context.Users.AnyAsync(u => u.MandalId == mandalId && !u.IsDeleted);
        }

        public async Task DeleteMandalAsync(int id)
        {
            var mandal = await _context.Mandals.FindAsync(id);
            if (mandal != null)
            {
                if (await IsMandalAssignedToAnyUserAsync(id))
                {
                    throw new InvalidOperationException("The Mandal is still assigned to a user.");
                }

                _context.Mandals.Remove(mandal);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> MandalNameExistsAsync(string mandalName)
        {
            return await _context.Mandals.AnyAsync(p => p.MandalName == mandalName && !p.IsDeleted);
        }
        public async Task<bool> MandalSlugExistsAsync(string mandalSlug)
        {
            return await _context.Mandals.AnyAsync(p => p.MandalSlug == mandalSlug && !p.IsDeleted);
        }
        private IQueryable<Mandal> ApplySorting(IQueryable<Mandal> query, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField)) sortField = "UpdatedAt"; // Default sort field
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "desc"; // Default sort order

            switch (sortField.ToLower())
            {
                case "mandalname":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.MandalName) : query.OrderByDescending(w => w.MandalName);
                    break;
                case "mandaldesc":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.MandalDescription) : query.OrderByDescending(w => w.MandalDescription);
                    break;
                case "createdat":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.CreatedAt) : query.OrderByDescending(w => w.CreatedAt);
                    break;
                case "updatedat":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.UpdatedAt) : query.OrderByDescending(w => w.UpdatedAt);
                    break;
                case "createdby":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.CreatedBy) : query.OrderByDescending(w => w.CreatedBy);
                    break;
                default:
                    query = query.OrderByDescending(w => w.UpdatedAt); // Default sorting by UpdatedAt in descending order
                    break;
            }

            return query;
        }

    }
}
