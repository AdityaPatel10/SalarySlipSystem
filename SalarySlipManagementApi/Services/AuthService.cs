using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalarySlipManagementApi.DTOs.AuthDTOs;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var employees = await _unitOfWork.Employees.FindAsync(
                e => e.Email == request.Email,
                includeProperties: "Role"
            );
            var employee = employees.FirstOrDefault();

            if (
                employee == null
                || !BCrypt.Net.BCrypt.Verify(request.Password, employee.PasswordHash)
            )
            {
                throw new Exception("Invalid email or password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(
                _config["JwtSettings:Secret"] ?? throw new Exception("JWT Secret not found")
            );

            var roleName = employee.Role?.Name ?? "Employee";

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, employee.GlobalId.ToString()),
                        new Claim(ClaimTypes.Email, employee.Email),
                        new Claim(ClaimTypes.Role, roleName),
                    }
                ),

                Expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["JwtSettings:ExpirationInMinutes"] ?? "60")
                ),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                Message = "Login successful",
                UserRole = roleName,
            };
        }
    }
}
