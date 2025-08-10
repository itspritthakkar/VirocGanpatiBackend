using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class DateOfVisarjanRepository : IDateOfVisarjanRepository
    {
        private readonly ApplicationDbContext _context;

        public DateOfVisarjanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DateOfVisarjan>> GetAllAsync()
        {
            return await _context.DateOfVisarjans.ToListAsync();
        }

        public async Task<DateOfVisarjan> GetByIdAsync(int id)
        {
            return await _context.DateOfVisarjans.FindAsync(id);
        }

        public async Task<DateOfVisarjan> AddAsync(DateOfVisarjan entity)
        {
            _context.DateOfVisarjans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<DateOfVisarjan> UpdateAsync(DateOfVisarjan entity)
        {
            _context.DateOfVisarjans.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.DateOfVisarjans.FindAsync(id);
            if (entity == null) return false;

            _context.DateOfVisarjans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
