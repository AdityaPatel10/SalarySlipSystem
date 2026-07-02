namespace SalarySlipManagementApi.Entities
{
    public class MonthlySalarySlip
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal OtherAllowances { get; set; }
        public decimal PFDeduction { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedOn { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
