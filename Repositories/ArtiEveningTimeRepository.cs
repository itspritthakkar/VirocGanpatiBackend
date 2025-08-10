using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class ArtiEveningTimeRepository : IArtiEveningTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtiEveningTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArtiEveningTime>> GetAllAsync()
        {
            return await _context.ArtiEveningTimes.ToListAsync();
        }

        public async Task<ArtiEveningTime> GetByIdAsync(int id)
        {
            return await _context.ArtiEveningTimes.FindAsync(id);
        }

        public async Task<ArtiEveningTime> AddAsync(ArtiEveningTime entity)
        {
            _context.ArtiEveningTimes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ArtiEveningTime> UpdateAsync(ArtiEveningTime entity)
        {
            _context.ArtiEveningTimes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ArtiEveningTimes.FindAsync(id);
            if (entity == null) return false;

            _context.ArtiEveningTimes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
