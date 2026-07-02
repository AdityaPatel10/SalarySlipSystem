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
