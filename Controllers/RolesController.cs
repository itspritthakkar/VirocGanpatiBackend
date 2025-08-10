using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return ResponseHelper.NotFoundMessage("Role not found");
            }
            return Ok(role);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto roleDto)
        {
            var role = await _roleService.AddRoleAsync(roleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.RoleId }, role);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] AddRoleDto roleDto)
        {
            var role = await _roleService.UpdateRoleAsync(id, roleDto);
            if (role == null)
            {
                return ResponseHelper.NotFoundMessage("Role not found");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}
