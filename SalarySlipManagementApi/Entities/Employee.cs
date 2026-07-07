namespace SalarySlipManagementApi.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? RestOtp { get; set; }
        public DateTime? OtpEpiry { get; set; }
        public Role? Role { get; set; }
        public Department? Department { get; set; }
        public SalaryStructure? SalaryStructure { get; set; }
        public ICollection<MonthlySalarySlip> MonthlySalarySlips { get; set; } =
            new List<MonthlySalarySlip>();
    }
}
