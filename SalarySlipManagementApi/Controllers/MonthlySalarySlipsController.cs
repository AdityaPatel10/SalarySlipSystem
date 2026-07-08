using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs.SalarySlipDTOs;
using SalarySlipManagementApi.Repositories;
using SalarySlipManagementApi.Services;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        public async Task<ActionResult> GetEmployeeHistory(Guid employeeGlobalId)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (loggedInUserRole != "Admin" && loggedInUserId != employeeGlobalId.ToString())
            {
                return Forbid();
            }

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

            var response = slips.Select(s => new
            {
                month = s.Month,
                year = s.Year,

                basicSalary = s.BasicSalary,
                hra = s.HRA,
                otherAllowances = s.OtherAllowances,

                pfDeduction = s.PFDeduction,
                taxDeduction = s.TaxDeduction,

                grossSalary = s.GrossSalary,
                totalDeduction = s.TotalDeduction,
                netSalary = s.NetSalary,
            });

            return Ok(response);
        }

        [HttpGet("GetTotalPayroll")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetTotalPayroll()
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var slips = await _unitOfWork.MonthlySalarySlips.FindAsync(s =>
                s.Month == currentMonth && s.Year == currentYear
            );

            var totalPayroll = slips.Sum(s => s.NetSalary);

            return Ok(new { total = totalPayroll });
        }
    }
}
