using Microsoft.IdentityModel.Tokens.Experimental;
using SalarySlipManagementApi.DTOs.SalarySlipDTOs;
using SalarySlipManagementApi.Entities;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Services
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaryCalculationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SalarySlipResponseDto> CalculateSalaryAsync(GenerateSalarySlipDto request)
        {
            var employees = await _unitOfWork.Employees.FindAsync(
                e => e.GlobalId == request.EmployeeGlobalId,
                includeProperties: "Department,SalaryStructure"
            );

            var employee = employees.FirstOrDefault();
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            if (employee.SalaryStructure == null)
            {
                throw new Exception("No salary structure found for the employee.");
            }

            var requestDate = new DateTime(request.Year, request.Month, 1);
            var currentDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            if (requestDate > currentDate)
            {
                throw new Exception(
                    "Access Denied: You cannot generate a Salary slip for future month."
                );
            }

            var existingSlips = await _unitOfWork.MonthlySalarySlips.FindAsync(s =>
                s.EmployeeId == employee.Id && s.Month == request.Month && s.Year == request.Year
            );

            if (existingSlips.Any())
            {
                throw new Exception(
                    $"Duplicate Error: A salary slip already exists for {employee.Name} for {request.Month}/{request.Year}."
                );
            }

            decimal basic = employee.SalaryStructure.BasicSalary;

            decimal hra = basic * (employee.SalaryStructure.HRAPercentage / 100m);

            decimal otherAllowances =
                basic * (employee.SalaryStructure.OtherAllowancesPercentage / 100m);

            decimal grossSalary = basic + hra + otherAllowances;

            decimal pf = basic * (employee.SalaryStructure.PFDeductionPercentage / 100m);

            decimal tax = grossSalary * (employee.SalaryStructure.TaxDeductionPercentage / 100m);

            decimal totalDeduction = pf + tax;

            decimal netSalary = grossSalary - totalDeduction;

            var newSlip = new MonthlySalarySlip
            {
                Month = request.Month,
                Year = request.Year,
                BasicSalary = basic,
                HRA = hra,
                OtherAllowances = otherAllowances,
                PFDeduction = pf,
                TaxDeduction = tax,
                GrossSalary = grossSalary,
                TotalDeduction = totalDeduction,
                NetSalary = netSalary,
                EmployeeId = employee.Id,
            };

            await _unitOfWork.MonthlySalarySlips.AddAsync(newSlip);
            await _unitOfWork.CompleteAsync();

            return new SalarySlipResponseDto
            {
                EmployeeName = employee.Name,
                DepartmentName = employee.Department?.Name ?? "N/A",
                MonthYear = $"{request.Month}/{request.Year}",
                BasicSalary = basic,
                HraAmount = hra,
                OtherAllowancesAmount = otherAllowances,
                GrossSalary = grossSalary,
                PfDeductionAmount = pf,
                TaxDeductionAmount = tax,
                NetSalary = netSalary,
            };
        }
    }
}
