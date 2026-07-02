namespace SalarySlipManagementApi.DTOs.SalaryStructureDTOs
{
    public class SalaryStructureResponseDto
    {
        public Guid GlobalId { get; set; }
        public Guid EmployeeGlobalId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRAPercentage { get; set; }
        public decimal OtherAllowancesPercentage { get; set; }
        public decimal PFDeductionPercentage { get; set; }
        public decimal TaxDeductionPercentage { get; set; }
    }
}
