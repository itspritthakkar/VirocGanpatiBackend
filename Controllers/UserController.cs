using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerViewPolicy")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchValue = "", [FromQuery] string sortField = "UpdatedAt", [FromQuery] string sortOrder = "desc", [FromQuery] string role = "", [FromQuery] bool enablePagination = true, [FromQuery] int? projectId = 0)
        {
            var (totalCount, users) = await _userService.GetAllUsersAsync(page, pageSize, searchValue, sortField, sortOrder, role, enablePagination, projectId);
            return Ok(new { totalCount, users });
        }

        [HttpGet("Mandal")]
        [Authorize]
        public async Task<IActionResult> GetUserProject()
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (userId == null)
            {
                return ResponseHelper.NotFoundMessage("User not found");
            }
            var project = await _userService.GetUserProjectAsync(userId.Value);
            return Ok(project);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userId = HttpContext.Items["UserId"] as int?;
            if (userId == null)
            {
                return ResponseHelper.NotFoundMessage("User token invalid");
            }
            UserDto user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
            {
                return ResponseHelper.NotFoundMessage("User not found");
            }
            return Ok(user);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerViewPolicy")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return ResponseHelper.NotFoundMessage("User not found");
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
        {
            try
            {
                var user = await _userService.AddUserAsync(userDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return ResponseHelper.BadRequestMessage(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, userDto);
                if (user == null)
                {
                    return ResponseHelper.NotFoundMessage("User not found");
                }
                return NoContent();
            }
            catch (DbUpdateException e)
            {
                return ResponseHelper.BadRequestMessage("User Email already exists");
            }
        }

        [HttpPut("Manager")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> UpdateManager([FromBody] UpdateManagerDto userDto)
        {
            try
            {
                var id = HttpContext.Items["UserId"] as int?;
                if (id == null)
                {
                    return ResponseHelper.NotFoundMessage("User token invalid");
                }

                var user = await _userService.UpdateManagerAsync(id.Value, userDto);
                if (user == null)
                {
                    return ResponseHelper.NotFoundMessage("User not found");
                }
                return NoContent();
            }
            catch (DbUpdateException e)
            {
                return ResponseHelper.BadRequestMessage("User Email already exists");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
