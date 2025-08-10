using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MandalsController : ControllerBase
    {
        private readonly IMandalService _mandalService;
        private readonly IUserService _userService;

        public MandalsController(IMandalService mandalService, IUserService userService)
        {
            _mandalService = mandalService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<ActionResult<object>> GetMandals([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchValue = "", [FromQuery] string sortField = "UpdatedAt", [FromQuery] string sortOrder = "desc", [FromQuery] string status = "")
        {
            var (totalElements, data) = await _mandalService.GetMandalsAsync(page, pageSize, searchValue, sortField, sortOrder, status);

            return Ok(new
            {
                totalElements,
                data
            });
        }

        [HttpGet("All")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<ActionResult<object>> GetAllMandalss()
        {
            var (totalElements, data) = await _mandalService.GetAllMandalsAsync();

            return Ok(new
            {
                totalElements,
                data
            });
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<ActionResult<MandalDto>> GetMandalById(int id)
        {
            var mandal = await _mandalService.GetMandalByIdAsync(id);
            if (mandal == null) return ResponseHelper.NotFoundMessage<MandalDto>("Mandal not found");
            return Ok(mandal);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> AddMandal(AddMandalDto mandalDto)
        {
            try
            {
                var userId = HttpContext.Items["UserId"] as int?;
                if (userId == null)
                {
                    return ResponseHelper.NotFoundMessage("User token invalid");
                }

                UserDto user = await _userService.GetUserByIdAsync(userId.Value);

                mandalDto.CreatedBy = user.FirstName + ' ' + user.LastName;

                var createdProject = await _mandalService.AddMandalAsync(mandalDto);
                return CreatedAtAction(nameof(GetMandalById), new { id = createdProject.MandalId }, createdProject);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.BadRequestMessage(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> UpdateMandal(int id, UpdateMandalDto mandalDto)
        {
            var updatedMandal = await _mandalService.UpdateMandalAsync(id, mandalDto);
            if (updatedMandal == null) return ResponseHelper.NotFoundMessage("Mandal not found");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> DeleteMandal(int id)
        {
            try
            {
                await _mandalService.DeleteMandalAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)  // Handles the case when a mandal is assigned to a user
            {
                return ResponseHelper.BadRequestMessage(ex.Message);
            }
        }

        //[HttpPost("import")]
        //[Authorize(Policy = "ManagerPolicy")]
        //public async Task<IActionResult> ImportProjects(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return ResponseHelper.BadRequestMessage("No file uploaded.");

        //    var projectDtos = new List<AddMandalDto>();

        //    using (var stream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(stream);
        //        stream.Position = 0;  // Reset stream position

        //        IWorkbook workbook;
        //        if (file.FileName.EndsWith(".xlsx"))
        //        {
        //            workbook = new XSSFWorkbook(stream);  // For .xlsx files
        //        }
        //        else if (file.FileName.EndsWith(".xls"))
        //        {
        //            workbook = new HSSFWorkbook(stream);  // For .xls files
        //        }
        //        else
        //        {
        //            return ResponseHelper.BadRequestMessage("Invalid file format. Please upload a .xls or .xlsx file.");
        //        }

        //        ISheet sheet = workbook.GetSheetAt(0);  // Get the first sheet

        //        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)  // Start from row 1 (skip header)
        //        {
        //            IRow row = sheet.GetRow(rowIndex);
        //            if (row == null) continue;  // Skip empty rows

        //            var projectDto = new AddMandalDto
        //            {
        //                MandalName = row.GetCell(0).ToString(),
        //                Status = row.GetCell(1).ToString(),
        //                MandalDescription = row.GetCell(2)?.ToString()  // Optional, may be null
        //            };

        //            projectDtos.Add(projectDto);
        //        }
        //    }

        //    foreach (var projectDto in projectDtos)
        //    {
        //        await _mandalService.AddMandalAsync(projectDto);
        //    }

        //    return Ok("Mandals imported successfully.");
        //}


    }
}
