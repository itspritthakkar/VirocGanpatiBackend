using AutoMapper;
using VirocGanpati.DTOs;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public RecordService(IRecordRepository recordRepository, IMapper mapper)
        {
            _recordRepository = recordRepository;
            _mapper = mapper;
        }

        public async Task<(int totalElements, int filteredTotalCount, int projectCount, IEnumerable<RecordDto> data)> GetRecordsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, int? projectId, List<int>? users, string status, bool locationMarkedOnly, bool enablePagination, DateTime? startDate, DateTime? endDate, bool? resurveyOnly, bool? resurveyDoneOnly)
        {
            var (totalCount, filteredTotalCount, projectCount, records) = await _recordRepository.GetRecordsAsync(page, pageSize, searchValue, sortField, sortOrder, projectId, users, status, locationMarkedOnly, enablePagination, startDate, endDate, resurveyOnly, resurveyDoneOnly);
            var recordDtos = records.Select(record =>
            {
                var recordDto = _mapper.Map<RecordDto>(record);
                return recordDto;
            }).ToList();
            return (totalCount, filteredTotalCount, projectCount, recordDtos);
        }

        public async Task<(int totalElements, IEnumerable<RecordExportDto> data)> GetExportRecordsAsync(string sortField, string sortOrder, int? projectId, List<int>? users, string status, DateTime? startDate, DateTime? endDate)
        {
            var (totalCount, records) = await _recordRepository.GetExportRecordsAsync(sortField, sortOrder, projectId, users, status, startDate, endDate);
            var recordExportDtos = records.Select(record =>
            {
                var recordExportDtos = _mapper.Map<RecordExportDto>(record);
                return recordExportDtos;
            }).ToList();
            return (totalCount, recordExportDtos);
        }

        public async Task<RecordDto> GetRecordByIdAsync(int id)
        {
            var record = await _recordRepository.GetRecordByIdAsync(id);
            if (record == null) return null;
            var recordDto = _mapper.Map<RecordDto>(record);
            return recordDto;
        }

        public async Task<RecordDto> AddRecordAsync(AddRecordDto recordDto)
        {
            var record = _mapper.Map<Record>(recordDto);
            var createdRecord = await _recordRepository.AddRecordAsync(record);
            return _mapper.Map<RecordDto>(createdRecord);
        }

        public async Task<IEnumerable<RecordDto>> AddRecordsBulkAsync(IEnumerable<AddRecordDto> recordDtos)
        {
            List<Record> bulkRecords = new List<Record>();
            foreach (var recordDto in recordDtos)
            {
                var record = _mapper.Map<Record>(recordDto);
                record.CreatedAt = DateTime.UtcNow;
                record.UpdatedAt = DateTime.UtcNow;
                bulkRecords.Add(record);
            }

            await _recordRepository.AddRecordsBulkAsync(bulkRecords);

            return _mapper.Map<List<RecordDto>>(bulkRecords);
        }

        public async Task<RecordDto> UpdateRecordAsync(int id, AddRecordDto recordDto)
        {
            var record = await _recordRepository.GetRecordByIdAsync(id);
            if (record == null) return null;
            _mapper.Map(recordDto, record);
            await _recordRepository.UpdateRecordAsync(record);
            return _mapper.Map<RecordDto>(record);
        }

        public async Task DeleteRecordAsync(int id)
        {
            await _recordRepository.DeleteRecordAsync(id);
        }
    }
}
