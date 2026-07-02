namespace SalarySlipManagementApi.Entities
{
    public class SalaryStructure
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public decimal BasicSalary { get; set; }
        public decimal HRAPercentage { get; set; }
        public decimal OtherAllowancesPercentage { get; set; }
        public decimal PFDeductionPercentage { get; set; }
        public decimal TaxDeductionPercentage { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
