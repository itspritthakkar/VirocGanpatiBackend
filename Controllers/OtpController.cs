using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirocGanpati.DTOs;
using VirocGanpati.DTOs.Otp;
using VirocGanpati.Exceptions;
using VirocGanpati.Helpers;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IUserService _userService;

        public OtpController(IOtpService otpService, IUserService userService)
        {
            _otpService = otpService;
            _userService = userService;
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto request)
        {
            try
            {
                int userId = HttpContext.GetLoggedInUserId() ?? throw new UnauthorizedAccessException("Tampered JWT token. UserId not found");

                UserDto userdto = await _userService.GetUserByIdAsync(userId);

                var otp = await _otpService.SendOtpAsync(userdto.Mobile, request.Purpose, 4);
                return Ok(new SendOtpResponseDto
                {
                    Message = "OTP sent successfully",
                    OtpId = otp.Id
                });
            }
            catch (UnauthorizedAccessException e)
            {
                return ResponseHelper.UnauthorizedMessage(e.Message);
            }
            catch(OtpAlreadyExistsException e)
            {
                return ResponseHelper.ConflictMessage(e.Message);
            }
            catch (Exception e) {
                return ResponseHelper.InternalServerErrorMessage(e.Message);
            }
        }

        [HttpPost("resend")]
        [Authorize]
        public async Task<IActionResult> ResendOtp([FromBody] SendOtpRequestDto request)
        {
            try
            {
                int userId = HttpContext.GetLoggedInUserId() ?? throw new UnauthorizedAccessException("Tampered JWT token. UserId not found");

                UserDto userdto = await _userService.GetUserByIdAsync(userId);

                var otp = await _otpService.SendOtpAsync(userdto.Mobile, request.Purpose, 4);
                return Ok(new SendOtpResponseDto
                {
                    Message = "OTP sent successfully",
                    OtpId = otp.Id
                });
            }
            catch (UnauthorizedAccessException e)
            {
                return ResponseHelper.UnauthorizedMessage(e.Message);
            }
            catch (Exception e)
            {
                return ResponseHelper.InternalServerErrorMessage(e.Message);
            }
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
        {
            try
            {
                int userId = HttpContext.GetLoggedInUserId() ?? throw new UnauthorizedAccessException("Tampered JWT token. UserId not found");

                UserDto userdto = await _userService.GetUserByIdAsync(userId);

                var success = await _otpService.VerifyOtpAsync(userdto.Mobile, request.OtpCode, request.Purpose);
                if (!success)
                    return BadRequest(new VerifyOtpResponseDto
                    {
                        Message = "Invalid or expired OTP",
                        IsVerified = false
                    });

                return Ok(new VerifyOtpResponseDto
                {
                    Message = "OTP verified successfully",
                    IsVerified = true
                });
            }
            catch (UnauthorizedAccessException e)
            {
                return ResponseHelper.UnauthorizedMessage(e.Message);
            }
        }
    }
}
