using System.ComponentModel.DataAnnotations;

namespace SalarySlipManagementApi.DTOs
{
    public class DepartmentUpdateDto
    {
        [Required(ErrorMessage = "Department Name is required.")]
        [MaxLength(100, ErrorMessage = "Department Name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; } = string.Empty;
    }
}
