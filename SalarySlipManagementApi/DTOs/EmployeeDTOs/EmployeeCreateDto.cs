using System.ComponentModel.DataAnnotations;

namespace SalarySlipManagementApi.DTOs.EmployeeDTOs
{
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(30)]
        public string BankAccountNumber { get; set; } = string.Empty;

        public DateTime DateOfJoining { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}