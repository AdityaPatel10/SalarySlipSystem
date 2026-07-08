using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs;
using SalarySlipManagementApi.DTOs.DepartmentDTOs;
using SalarySlipManagementApi.Entities;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDepartments")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAllDepartments()
        {
            var departsments = await _unitOfWork.Departments.GetAllAsync();
            var response = departsments.Select(d => new DepartmentResponseDto
            {
                GlobalId = d.GlobalId,
                DepartmentName = d.Name,
            });

            return Ok(response);
        }

        [HttpGet("GetDepartmentById/{globalId}")]
        [Authorize]
        public async Task<ActionResult<DepartmentResponseDto>> GetDepartmentById(Guid globalId)
        {
            var departments = await _unitOfWork.Departments.FindAsync(d => d.GlobalId == globalId);
            var department = departments.FirstOrDefault();
            if (department == null)
            {
                return NotFound("Department not found.");
            }

            var response = new DepartmentResponseDto
            {
                GlobalId = department.GlobalId,
                DepartmentName = department.Name,
            };

            return Ok(response);
        }

        [HttpPost("CreateNewDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateNewDepartment([FromBody] DepartmentCreateDto dto)
        {
            var newDepartment = new Department { Name = dto.DepartmentName };

            await _unitOfWork.Departments.AddAsync(newDepartment);
            await _unitOfWork.CompleteAsync();

            return Ok(
                new
                {
                    Message = "Department created successfully!",
                    GlobalId = newDepartment.GlobalId,
                }
            );
        }

        [HttpPut("UpdateDepartment/{globalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDepartment(
            Guid globalId,
            [FromBody] DepartmentUpdateDto dto
        )
        {
            var departments = await _unitOfWork.Departments.FindAsync(d => d.GlobalId == globalId);
            var department = departments.FirstOrDefault();
            if (department == null)
            {
                return BadRequest("Department not found.");
            }

            department.Name = dto.DepartmentName;
            department.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Departments.Update(department);
            await _unitOfWork.CompleteAsync();

            return Ok("Department updated successfully.");
        }

        [HttpDelete("DeleteDepartment/{globalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDepartment(Guid globalId)
        {
            var departments = await _unitOfWork.Departments.FindAsync(d => d.GlobalId == globalId);
            var department = departments.FirstOrDefault();

            if (department == null)
            {
                return NotFound("Department not found.");
            }

            _unitOfWork.Departments.Delete(department);
            await _unitOfWork.CompleteAsync();
            return Ok("Department deleted successfully.");
        }
    }
}
