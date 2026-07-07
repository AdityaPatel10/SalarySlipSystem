using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs.AuthDTOs;
using SalarySlipManagementApi.Services;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto request)
        {
            var otp = await _authService.RequestOtpAsync(request);
            if (otp == "User not found")
                return NotFound(new { message = otp });

            return Ok(new { message = "OTP generated successfully", otpCode = otp });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto request)
        {
            var isValid = await _authService.VerifyOtpAsync(request);

            if (!isValid)
                return BadRequest(new { message = "Invalid or Expired OTP" });

            return Ok(new { message = "OTP verified successfully" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var success = await _authService.ResetPasswordAsnyc(request);

            if (!success)
                return BadRequest(new { message = "Failed to reset password. Invalid user" });

            return Ok(new { message = "Password reset successfully!" });
        }
    }
}
