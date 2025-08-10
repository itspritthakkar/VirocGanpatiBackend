using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;
        private readonly IUserService _userService;

        public RecordsController(IRecordService recordService, IUserService userService)
        {
            _recordService = recordService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerViewPolicy")]
        public async Task<ActionResult<object>> GetRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchValue = "", [FromQuery] string sortField = "UpdatedAt", [FromQuery] string sortOrder = "desc", [FromQuery] int? projectId = 0, [FromQuery] string users = "", [FromQuery] string status = "", [FromQuery] bool locationMarkedOnly = false, [FromQuery] bool enablePagination = true, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, [FromQuery] bool resurveyOnly = false, [FromQuery] bool resurveyDoneOnly = false)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                throw new ArgumentException($"'{nameof(sortField)}' cannot be null or empty.", nameof(sortField));
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                throw new ArgumentException($"'{nameof(sortOrder)}' cannot be null or empty.", nameof(sortOrder));
            }

            List<int> userIdList = users
                                .Split(',')
                                .Where(id => !string.IsNullOrWhiteSpace(id)) // Filters out empty parts
                                .Select(int.Parse)
                                .ToList();

            var (totalElements, filteredTotalCount, projectCount, data) = await _recordService.GetRecordsAsync(page, pageSize, searchValue, sortField, sortOrder, projectId, userIdList, status, locationMarkedOnly, enablePagination, startDate, endDate, resurveyOnly, resurveyDoneOnly);
            return Ok(new
            {
                totalElements,
                filteredTotalCount,
                projectCount,
                data
            });
        }

        [HttpGet("Export")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<ActionResult<object>> GetExportRecords([FromQuery] string sortField = "UpdatedAt", [FromQuery] string sortOrder = "desc", [FromQuery] int? projectId = 0, [FromQuery] string users = "", [FromQuery] string status = "", [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                throw new ArgumentException($"'{nameof(sortField)}' cannot be null or empty.", nameof(sortField));
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                throw new ArgumentException($"'{nameof(sortOrder)}' cannot be null or empty.", nameof(sortOrder));
            }

            List<int> userIdList = users
                                .Split(',')
                                .Where(id => !string.IsNullOrWhiteSpace(id)) // Filters out empty parts
                                .Select(int.Parse)
                                .ToList();

            var (totalElements, data) = await _recordService.GetExportRecordsAsync(sortField, sortOrder, projectId, userIdList, status, startDate, endDate);
            return Ok(new
            {
                totalElements,
                data
            });
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<RecordDto>> GetRecordById(int id)
        {
            var record = await _recordService.GetRecordByIdAsync(id);
            if (record == null) return ResponseHelper.NotFoundMessage<RecordDto>("Survey Record not found");
            return Ok(record);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<ActionResult<RecordDto>> AddRecord(AddRecordDto recordDto)
        {
            var createdRecord = await _recordService.AddRecordAsync(recordDto);
            return CreatedAtAction(nameof(GetRecordById), new { id = createdRecord.RecordId }, createdRecord);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> UpdateRecord(int id, AddRecordDto recordDto)
        {
            var updatedRecord = await _recordService.UpdateRecordAsync(id, recordDto);
            if (updatedRecord == null) return ResponseHelper.NotFoundMessage("Survey Record not found");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            await _recordService.DeleteRecordAsync(id);
            return NoContent();
        }

        [HttpGet("User")]
        [Authorize]
        public async Task<IActionResult> GetUserRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchValue = "", [FromQuery] string sortField = "UpdatedAt", [FromQuery] string sortOrder = "desc", [FromQuery] string users = "", [FromQuery] string status = "", [FromQuery] bool locationMarkedOnly = false, [FromQuery] bool enablePagination = true, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, [FromQuery] bool? resurveyOnly = false, [FromQuery] bool? resurveyDoneOnly = false)
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (userId == null)
            {
                return ResponseHelper.NotFoundMessage("User not found");
            }
            var project = await _userService.GetUserProjectAsync(userId.Value);

            if (project == null)
            {
                return ResponseHelper.NotFoundMessage("No Mandal Assigned");
            }

            List<int> userIdList = users
                                .Split(',')
                                .Where(id => !string.IsNullOrWhiteSpace(id)) // Filters out empty parts
                                .Select(int.Parse)
                                .ToList();

            var (totalElements, filteredTotalCount, projectCount, records) = await _recordService.GetRecordsAsync(page, pageSize, searchValue, sortField, sortOrder, project.MandalId, userIdList, status, locationMarkedOnly, enablePagination, startDate, endDate, resurveyOnly, resurveyDoneOnly);
            return Ok(new
            {
                totalElements,
                filteredTotalCount,
                projectCount,
                records
            });
        }
    }
}
