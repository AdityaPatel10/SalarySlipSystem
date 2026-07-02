using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalarySlipManagementApi.DTOs.SalaryStructureDTOs;
using SalarySlipManagementApi.Entities;
using SalarySlipManagementApi.Repositories;

namespace SalarySlipManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryStructuresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaryStructuresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllSalaryStructures")]
        public async Task<
            ActionResult<IEnumerable<SalaryStructureResponseDto>>
        > GetAllSalaryStructures()
        {
            var structures = await _unitOfWork.SalaryStructures.GetAllAsync(
                includeProperties: "Employee"
            );

            var response = structures.Select(s => new SalaryStructureResponseDto
            {
                GlobalId = s.GlobalId,
                EmployeeGlobalId = s.Employee?.GlobalId ?? Guid.Empty,
                BasicSalary = s.BasicSalary,
                HRAPercentage = s.HRAPercentage,
                OtherAllowancesPercentage = s.OtherAllowancesPercentage,
                PFDeductionPercentage = s.PFDeductionPercentage,
                TaxDeductionPercentage = s.TaxDeductionPercentage,
            });

            return Ok(response);
        }

        [HttpGet("GetSalaryStructureById/{globalId}")]
        public async Task<ActionResult<SalaryStructureResponseDto>> GetSalaryStructureById(
            Guid globalId
        )
        {
            var structures = await _unitOfWork.SalaryStructures.FindAsync(
                s => s.GlobalId == globalId,
                includeProperties: "Employee"
            );
            var structure = structures.FirstOrDefault();

            if (structure == null)
            {
                return NotFound("Salary structure not found.");
            }

            var response = new SalaryStructureResponseDto
            {
                GlobalId = structure.GlobalId,
                EmployeeGlobalId = structure.Employee?.GlobalId ?? Guid.Empty,
                BasicSalary = structure.BasicSalary,
                HRAPercentage = structure.HRAPercentage,
                OtherAllowancesPercentage = structure.OtherAllowancesPercentage,
                PFDeductionPercentage = structure.PFDeductionPercentage,
                TaxDeductionPercentage = structure.TaxDeductionPercentage,
            };

            return Ok(response);
        }

        [HttpPost("CreateNewSalaryStructure")]
        public async Task<ActionResult> CreateNewSalaryStructure(
            [FromBody] SalaryStructureCreateDto dto
        )
        {
            var employees = await _unitOfWork.Employees.FindAsync(e =>
                e.GlobalId == dto.EmployeeGlobalId
            );
            var employee = employees.FirstOrDefault();

            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var newStructure = new SalaryStructure
            {
                EmployeeId = employee.Id,
                BasicSalary = dto.BasicSalary,
                HRAPercentage = dto.HRAPercentage,
                OtherAllowancesPercentage = dto.OtherAllowancesPercentage,
                PFDeductionPercentage = dto.PFDeductionPercentage,
                TaxDeductionPercentage = dto.TaxDeductionPercentage,
            };

            await _unitOfWork.SalaryStructures.AddAsync(newStructure);
            await _unitOfWork.CompleteAsync();

            return Ok(
                new
                {
                    Message = "Salary structure created successfully.",
                    SalaryStructureGlobalId = newStructure.GlobalId,
                }
            );
        }

        [HttpPut("UpdateSalaryStructure/{globalId}")]
        public async Task<ActionResult> UpdateSalaryStructure(
            Guid globalId,
            [FromBody] SalaryStructureUpdateDto dto
        )
        {
            var structures = await _unitOfWork.SalaryStructures.FindAsync(s =>
                s.GlobalId == globalId
            );
            var structure = structures.FirstOrDefault();

            if (structure == null)
            {
                return NotFound("Salary structure not found.");
            }

            structure.BasicSalary = dto.BasicSalary;
            structure.HRAPercentage = dto.HRAPercentage;
            structure.OtherAllowancesPercentage = dto.OtherAllowancesPercentage;
            structure.PFDeductionPercentage = dto.PFDeductionPercentage;
            structure.TaxDeductionPercentage = dto.TaxDeductionPercentage;

            _unitOfWork.SalaryStructures.Update(structure);
            await _unitOfWork.CompleteAsync();

            return Ok("Salary structure updated successfully.");
        }

        [HttpDelete("DeleteSalaryStructure/{globalId}")]
        public async Task<ActionResult> DeleteSalaryStructure(Guid globalId)
        {
            var sturctures = await _unitOfWork.SalaryStructures.FindAsync(s =>
                s.GlobalId == globalId
            );
            var structure = sturctures.FirstOrDefault();

            if (structure == null)
            {
                return NotFound("Salary structure not found.");
            }

            _unitOfWork.SalaryStructures.Delete(structure);
            await _unitOfWork.CompleteAsync();

            return Ok("Salary structure deleted successfully.");
        }
    }
}
