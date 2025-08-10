using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class ArtiMorningTimeRepository : IArtiMorningTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtiMorningTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArtiMorningTime>> GetAllAsync()
        {
            return await _context.ArtiMorningTimes.ToListAsync();
        }

        public async Task<ArtiMorningTime> GetByIdAsync(int id)
        {
            return await _context.ArtiMorningTimes.FindAsync(id);
        }

        public async Task<ArtiMorningTime> AddAsync(ArtiMorningTime entity)
        {
            _context.ArtiMorningTimes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ArtiMorningTime> UpdateAsync(ArtiMorningTime entity)
        {
            _context.ArtiMorningTimes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ArtiMorningTimes.FindAsync(id);
            if (entity == null) return false;

            _context.ArtiMorningTimes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
