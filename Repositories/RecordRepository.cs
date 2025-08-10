using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public RecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(int totalCount, int filteredTotalCount, int projectCount, IEnumerable<Record>)> GetRecordsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, int? projectId, List<int>? users, string status, bool locationMarkedOnly, bool enablePagination, DateTime? startDate, DateTime? endDate, bool? resurveyOnly, bool? resurveyDoneOnly)
        {
            var query = _context.Records
                                .Include(r => r.Updater)
                                .Where(r => !r.IsDeleted)
                                .AsQueryable();

            int totalCount = await query.CountAsync();

            string userTimeZoneId = "India Standard Time";

            // Get the user's time zone
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);

            // Convert startDate and endDate from user's local time to UTC
            startDate = startDate.HasValue
                ? (DateTime?)TimeZoneInfo.ConvertTimeToUtc(startDate.Value.Date, userTimeZone)
                : null;
            endDate = endDate.HasValue
                ? (DateTime?)TimeZoneInfo.ConvertTimeToUtc(endDate.Value.Date.AddDays(1).AddTicks(-1), userTimeZone)
                : null;

            if (status.Equals("pending", StringComparison.InvariantCultureIgnoreCase) && (users != null && users.Count > 0))
            {
                return (0, 0, 0, new List<Record>());
            }

            int projectCount = await query.CountAsync();
            
            if (users != null && users.Count > 0)
            {
                query = query.Where(r => users.Contains(r.Updater.UserId));
            }
            // Apply date range filters if provided
            if (startDate.HasValue)
            {
                query = query.Where(r => r.UpdatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.UpdatedAt <= endDate.Value);
            }

            query = ApplySorting(query, sortField, sortOrder);

            int filteredTotalCount = await query.CountAsync();

            if (enablePagination)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var records = await query.ToListAsync();

            return (totalCount, filteredTotalCount, projectCount, records);
        }

        public async Task<(int totalCount, IEnumerable<Record>)> GetExportRecordsAsync(string sortField, string sortOrder, int? projectId, List<int>? users, string status, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Records
                                .Include(r => r.Updater)
                                .Where(r => !r.IsDeleted)
                                .AsQueryable();

            startDate = startDate?.Date;
            endDate = endDate?.Date.AddDays(1).AddTicks(-1);

            if (users != null && users.Count > 0)
            {
                query = query.Where(r => users.Contains(r.Updater.UserId));
            }
            // Apply date range filters if provided
            if (startDate.HasValue)
            {
                query = query.Where(r => r.UpdatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.UpdatedAt <= endDate.Value);
            }

            query = ApplySorting(query, sortField, sortOrder);

            int totalCount = await query.CountAsync();

            var records = await query.ToListAsync();

            return (totalCount, records);
        }

        public async Task<Record> GetRecordByIdAsync(int id)
        {
            return await _context.Records
                                 .FirstOrDefaultAsync(r => r.RecordId == id && !r.IsDeleted);
        }

        public async Task<Record> AddRecordAsync(Record record)
        {
            record.CreatedAt = DateTime.UtcNow;
            record.UpdatedAt = DateTime.UtcNow;
            _context.Records.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<List<Record>> AddRecordsBulkAsync(List<Record> records)
        {
            await _context.Records.AddRangeAsync(records); // Efficient bulk addition
            await _context.SaveChangesAsync();

            return records;
        }

        public async Task<Record> UpdateRecordAsync(Record record)
        {
            record.UpdatedAt = DateTime.UtcNow;
            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteRecordAsync(int id)
        {
            var record = await _context.Records.FindAsync(id);
            if (record != null)
            {
                record.IsDeleted = true;
                record.UpdatedAt = DateTime.UtcNow;
                _context.Entry(record).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        private IQueryable<Record> ApplySorting(IQueryable<Record> query, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField)) sortField = "UpdatedAt"; // Default sort field
            if (string.IsNullOrEmpty(sortOrder)) sortOrder = "desc"; // Default sort order

            switch (sortField.ToLower())
            {
                case "createdat":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.CreatedAt) : query.OrderByDescending(w => w.CreatedAt);
                    break;
                case "updatedat":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(w => w.UpdatedAt) : query.OrderByDescending(w => w.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(w => w.UpdatedAt); // Default sorting by UpdatedAt in descending order
                    break;
            }

            return query;
        }
    }
}
