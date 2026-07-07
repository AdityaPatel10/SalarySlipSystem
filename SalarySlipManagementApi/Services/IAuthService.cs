using SalarySlipManagementApi.DTOs.AuthDTOs;

namespace SalarySlipManagementApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto request);
        Task<string> RequestOtpAsync(RequestOtpDto request);
        Task<bool> VerifyOtpAsync(VerifyOtpDto request);
        Task<bool> ResetPasswordAsnyc(ResetPasswordDto request);
    }
}
