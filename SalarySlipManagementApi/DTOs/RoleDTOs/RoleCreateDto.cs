using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SalarySlipManagementApi.DTOs.RoleDTOs
{
    public class RoleCreateDto
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength (100, ErrorMessage = "Role name cannot exceed 100 characters.")]
        public string RoleName { get; set; } = string.Empty;
    }
}
