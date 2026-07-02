using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs.EmployeeDTOs;
using SalarySlipManagementApi.Entities;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllEmployees")]
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
            });
            return Ok(response);
        }

        [HttpGet("GetEmployeeById/{globalId}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployeeById(Guid globalId)
        {
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
            };

            return Ok(response);
        }

        [HttpPost("CreateNewEmployee")]
        public async Task<ActionResult> CreateNewEmployee(
            [FromBody] EmployeeCreateDto dto
        )
        {
            var newEmployee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfJoining = dto.DateOfJoining,
                BankAccountNumber = dto.BankAccountNumber,
                RoleId = dto.RoleId,
                DepartmentId = dto.DepartmentId,
                PasswordHash = dto.Password, // We will hash this later!
            };

            await _unitOfWork.Employees.AddAsync(newEmployee);
            await _unitOfWork.CompleteAsync();

            return Ok(
                new
                {
                    Message = "Employee created successfully!",
                    GlobalId = newEmployee.GlobalId,
                }
            );
        }

        [HttpPut("UpdateEmployee/{globalId}")]
        public async Task<IActionResult> UpdateEmployee(
            Guid globalId,
            [FromBody] EmployeeUpdateDto dto
        )
        {
            var employees = await _unitOfWork.Employees.FindAsync(e => e.GlobalId == globalId);
            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                return BadRequest("Employee Not Found");
            }

            employee.Name = dto.Name;
            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.BankAccountNumber = dto.BankAccountNumber;
            employee.RoleId = dto.RoleId;
            employee.DepartmentId = dto.DepartmentId;

            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync();

            return Ok("Employee Updated Successfully!");
        }

        [HttpDelete("DeleteEmployee/{globalId}")]
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
