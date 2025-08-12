using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;
using VirocGanpati.Models;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IConfiguration config, IMapper mapper)
        {
            _userService = userService;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.ValidateUserAsync(loginDto.Mobile, loginDto.Password);

                var token = JwtTokenHelper.GenerateJwtToken(user, _config);
                UserDto userDto = _mapper.Map<UserDto>(user);
                return Ok(new { user = userDto, token });
            }
            catch (UnauthorizedAccessException ex) 
            {
                return ResponseHelper.UnauthorizedMessage(ex.Message);
            }
        }
    }
}
