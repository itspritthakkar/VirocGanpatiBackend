using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IRecordRepository
    {
        Task<(int totalCount, int filteredTotalCount, int projectCount, IEnumerable<Record>)> GetRecordsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, int? projectId, List<int>? users, string status, bool locationMarkedOnly, bool enablePagination, DateTime? startDate, DateTime? endDate, bool? resurveyOnly, bool? resurveyDoneOnly);
        Task<(int totalCount, IEnumerable<Record>)> GetExportRecordsAsync(string sortField, string sortOrder, int? projectId, List<int>? users, string status, DateTime? startDate, DateTime? endDate);
        Task<Record> GetRecordByIdAsync(int id);
        Task<Record> AddRecordAsync(Record record);
        Task<List<Record>> AddRecordsBulkAsync(List<Record> records);
        Task<Record> UpdateRecordAsync(Record record);
        Task DeleteRecordAsync(int id);
    }
}
