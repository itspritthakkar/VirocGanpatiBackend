using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class MandalAreaRepository : IMandalAreaRepository
    {
        private readonly ApplicationDbContext _context;

        public MandalAreaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MandalArea>> GetAllAsync()
        {
            return await _context.MandalAreas.ToListAsync();
        }

        public async Task<MandalArea> GetByIdAsync(int id)
        {
            return await _context.MandalAreas.FindAsync(id);
        }

        public async Task<MandalArea> AddAsync(MandalArea mandalArea)
        {
            _context.MandalAreas.Add(mandalArea);
            await _context.SaveChangesAsync();
            return mandalArea;
        }

        public async Task<MandalArea> UpdateAsync(MandalArea mandalArea)
        {
            _context.MandalAreas.Update(mandalArea);
            await _context.SaveChangesAsync();
            return mandalArea;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.MandalAreas.FindAsync(id);
            if (entity == null) return false;

            _context.MandalAreas.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
