using VirocGanpati.DTOs;
using VirocGanpati.Models;

namespace VirocGanpati.Services
{
    public interface IRecordService
    {
        Task<(int totalElements, int filteredTotalCount, int projectCount, IEnumerable<RecordDto> data)> GetRecordsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, int? projectId, List<int>? users, string status, bool locationMarkedOnly, bool enablePagination, DateTime? startDate, DateTime? endDate, bool? resurveyOnly, bool? resurveyDoneOnly);
        Task<(int totalElements, IEnumerable<RecordExportDto> data)> GetExportRecordsAsync(string sortField, string sortOrder, int? projectId, List<int>? users, string status, DateTime? startDate, DateTime? endDate);
        Task<RecordDto> GetRecordByIdAsync(int id);
        Task<RecordDto> AddRecordAsync(AddRecordDto recordDto);
        Task<IEnumerable<RecordDto>> AddRecordsBulkAsync(IEnumerable<AddRecordDto> recordDtos);
        Task<RecordDto> UpdateRecordAsync(int id, AddRecordDto recordDto);
        Task DeleteRecordAsync(int id);
    }
}
