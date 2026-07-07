using SalarySlipManagementApi.DTOs.AuthDTOs;

namespace SalarySlipManagementApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto request);
    }
}
