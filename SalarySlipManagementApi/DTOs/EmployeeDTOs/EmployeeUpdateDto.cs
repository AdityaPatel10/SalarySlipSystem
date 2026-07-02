using System.ComponentModel.DataAnnotations;

namespace SalarySlipManagementApi.DTOs.EmployeeDTOs
{
    public class EmployeeUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }
}
