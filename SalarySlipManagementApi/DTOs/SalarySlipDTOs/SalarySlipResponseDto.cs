namespace SalarySlipManagementApi.DTOs.SalarySlipDTOs
{
    public class SalarySlipResponseDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string MonthYear { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public decimal HraAmount { get; set; }
        public decimal OtherAllowancesAmount { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal PfDeductionAmount { get; set; }
        public decimal TaxDeductionAmount { get; set; }
        public decimal NetSalary { get; set; }
    }
}
