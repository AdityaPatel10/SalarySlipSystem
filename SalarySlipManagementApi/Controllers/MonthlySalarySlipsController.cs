using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs.SalarySlipDTOs;
using SalarySlipManagementApi.Repositories;
using SalarySlipManagementApi.Services;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlySalarySlipsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalaryCalculationService _salaryService;

        public MonthlySalarySlipsController(
            IUnitOfWork unitOfWork,
            ISalaryCalculationService salaryService
        )
        {
            _unitOfWork = unitOfWork;
            _salaryService = salaryService;
        }

        [HttpPost("GenerateSalarySlip")]
        public async Task<ActionResult<SalarySlipResponseDto>> GenerateSalarySlip(
            [FromBody] GenerateSalarySlipDto request
        )
        {
            try
            {
                var response = await _salaryService.CalculateSalaryAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeHistory/{employeeGlobalId}")]
        public async Task<ActionResult> GetEmployeeHistory(Guid employeeGlobalId)
        {
            var employees = await _unitOfWork.Employees.FindAsync(e =>
                e.GlobalId == employeeGlobalId
            );
            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var slips = await _unitOfWork.MonthlySalarySlips.FindAsync(s =>
                s.EmployeeId == employee.Id
            );

            return Ok(slips);
        }
    }
}
