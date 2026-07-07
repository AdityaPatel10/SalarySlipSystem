namespace SalarySlipManagementApi.DTOs.AuthDTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public Guid GlobalId { get; set; }
    }
}
