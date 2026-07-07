namespace SalarySlipManagementApi.DTOs.EmployeeDTOs
{
    public class EmployeeResponseDto
    {
        // Safe external ID
        public Guid GlobalId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }

        // Instead of sending the integer IDs back to Angular, 
        // in Phase 2 we will populate these string names using EF Core!
        public string DepartmentName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public Guid DepartmentGlobalId { get; set; }
        public Guid RoleGlobalId { get; set; }

    }
}
