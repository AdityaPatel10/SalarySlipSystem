using SalarySlipManagementApi.DTOs.SalarySlipDTOs;

namespace SalarySlipManagementApi.Services
{
    public interface ISalaryCalculationService
    {
        Task<SalarySlipResponseDto> CalculateSalaryAsync(GenerateSalarySlipDto request);
    }
}
