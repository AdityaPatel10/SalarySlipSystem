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
    }
}
