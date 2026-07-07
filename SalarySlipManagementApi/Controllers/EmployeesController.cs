using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs.EmployeeDTOs;
using SalarySlipManagementApi.Entities;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetAllEmployees()
        {
            var employees = await _unitOfWork.Employees.GetAllAsync(
                includeProperties: "Role,Department"
            );

            var response = employees.Select(e => new EmployeeResponseDto
            {
                GlobalId = e.GlobalId,
                Name = e.Name,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                BankAccountNumber = e.BankAccountNumber,
                DateOfJoining = e.DateOfJoining,
                DepartmentName = e.Department?.Name ?? "",
                RoleName = e.Role?.Name ?? "",
                DepartmentGlobalId = e.Department?.GlobalId ?? Guid.Empty,
                RoleGlobalId = e.Role?.GlobalId ?? Guid.Empty,
            });
            return Ok(response);
        }

        [HttpGet("GetEmployeeById/{globalId}")]
        [Authorize]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployeeById(Guid globalId)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (loggedInUserId != globalId.ToString() && loggedInUserRole != "Admin")
            {
                return Forbid();
            }

            var employees = await _unitOfWork.Employees.FindAsync(
                e => e.GlobalId == globalId,
                includeProperties: "Role,Department"
            );
            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var response = new EmployeeResponseDto
            {
                GlobalId = employee.GlobalId,
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                BankAccountNumber = employee.BankAccountNumber,
                DateOfJoining = employee.DateOfJoining,
                DepartmentName = employee.Department?.Name ?? "",
                RoleName = employee.Role?.Name ?? "",
                DepartmentGlobalId = employee.Department?.GlobalId ?? Guid.Empty,
                RoleGlobalId = employee.Role?.GlobalId ?? Guid.Empty,
            };

            return Ok(response);
        }

        [HttpPost("CreateNewEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateNewEmployee([FromBody] EmployeeCreateDto dto)
        {
            var departments = await _unitOfWork.Departments.FindAsync(d =>
                d.GlobalId == dto.DepartmentGlobalId
            );
            var department = departments.FirstOrDefault();
            if (department == null)
                return BadRequest("Invalid Department: Could not find it in the DB.");

            var roles = await _unitOfWork.Roles.FindAsync(r => r.GlobalId == dto.RoleGlobalId);
            var role = roles.FirstOrDefault();
            if (role == null)
                return BadRequest("Invalid Role: Could not find it in the DB.");

            var newEmployee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfJoining = dto.DateOfJoining,
                BankAccountNumber = dto.BankAccountNumber,
                RoleId = role.Id,
                DepartmentId = department.Id,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // We will hash this later!
            };

            await _unitOfWork.Employees.AddAsync(newEmployee);
            await _unitOfWork.CompleteAsync();

            return Ok(
                new { Message = "Employee created successfully!", GlobalId = newEmployee.GlobalId }
            );
        }

        [HttpPut("UpdateEmployee/{globalId}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(
            Guid globalId,
            [FromBody] EmployeeUpdateDto dto
        )
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (loggedInUserId != globalId.ToString() && loggedInUserRole != "Admin")
            {
                return Forbid();
            }

            var employees = await _unitOfWork.Employees.FindAsync(e => e.GlobalId == globalId);
            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                return BadRequest("Employee Not Found");
            }

            var departments = await _unitOfWork.Departments.FindAsync(d =>
                d.GlobalId == dto.DepartmentGlobalId
            );
            var department = departments.FirstOrDefault();
            if (department == null)
                return BadRequest("Invalid Department");

            var roles = await _unitOfWork.Roles.FindAsync(r => r.GlobalId == dto.RoleGlobalId);
            var role = roles.FirstOrDefault();
            if (role == null)
                return BadRequest("Invalid Role");

            employee.Name = dto.Name;
            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.BankAccountNumber = dto.BankAccountNumber;
            employee.DepartmentId = department.Id;
            employee.RoleId = role.Id;

            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync();

            return Ok("Employee Updated Successfully!");
        }

        [HttpDelete("DeleteEmployee/{globalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(Guid globalId)
        {
            var employees = await _unitOfWork.Employees.FindAsync(e => e.GlobalId == globalId);
            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            _unitOfWork.Employees.Delete(employee);
            await _unitOfWork.CompleteAsync();

            return Ok("Employee deleted successfully!");
        }
    }
}
