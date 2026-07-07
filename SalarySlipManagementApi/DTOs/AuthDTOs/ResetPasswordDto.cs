using System.Globalization;

namespace SalarySlipManagementApi.DTOs.AuthDTOs
{
    public class ResetPasswordDto
    {
        public string EmailorPhone { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
