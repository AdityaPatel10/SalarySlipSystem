namespace SalarySlipManagementApi.DTOs.EmployeeDTOs
{
    public class EmployeeSelfUpdateDto
    {
        public string? Name { get; set; } 
        public string? Email { get; set; }
        public string? BankAccountNumber { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? NewPassword { get; set; }
    }
}
