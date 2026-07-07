using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.Data;
using SalarySlipManagementApi.DTOs.RoleDTOs;
using SalarySlipManagementApi.Entities;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetAllRoles()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            var response = roles.Select(r => new RoleResponseDto
            {
                GlobalId = r.GlobalId,
                RoleName = r.Name,
            });

            return Ok(response);
        }

        [HttpGet("GetRoleById/{globalId}")]
        public async Task<ActionResult<RoleResponseDto>> GetRoleById(Guid globalId)
        {
            var roles = await _unitOfWork.Roles.FindAsync(r => r.GlobalId == globalId);
            var role = roles.FirstOrDefault();

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var response = new RoleResponseDto { GlobalId = role.GlobalId, RoleName = role.Name };
            return Ok(response);
        }

        [HttpPost("CreateNewRole")]
        public async Task<ActionResult> CreateNewRole([FromBody] RoleCreateDto dto)
        {
            var newRole = new Role { Name = dto.RoleName };

            await _unitOfWork.Roles.AddAsync(newRole);
            await _unitOfWork.CompleteAsync();

            return Ok(new { Message = "Role created successfully.", GlobalId = newRole.GlobalId });
        }

        [HttpPut("UpdateRole/{globalId}")]
        public async Task<ActionResult> UpdateRole(Guid globalId, [FromBody] RoleUpdateDto dto)
        {
            var roles = await _unitOfWork.Roles.FindAsync(r => r.GlobalId == globalId);
            var role = roles.FirstOrDefault();

            if (role == null)
            {
                return BadRequest("Role not found.");
            }

            role.Name = dto.RoleName;

            _unitOfWork.Roles.Update(role);
            await _unitOfWork.CompleteAsync();

            return Ok("Role updated successfully.");
        }

        [HttpDelete("DeleteRole/{globalId}")]
        public async Task<ActionResult> DeleteRole(Guid globalId)
        {
            var roles = await _unitOfWork.Roles.FindAsync(r => r.GlobalId == globalId);
            var role = roles.FirstOrDefault();
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            _unitOfWork.Roles.Delete(role);
            await _unitOfWork.CompleteAsync();

            return Ok("Role deleted successfully.");
        }
    }
}
