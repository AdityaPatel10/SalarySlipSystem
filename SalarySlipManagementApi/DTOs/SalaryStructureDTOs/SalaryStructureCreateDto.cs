using System.ComponentModel.DataAnnotations;

namespace SalarySlipManagementApi.DTOs.SalaryStructureDTOs
{
    public class SalaryStructureCreateDto
    {
        [Required]
        public Guid EmployeeGlobalId { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        public decimal HRAPercentage { get; set; } = 20;
        public decimal OtherAllowancesPercentage { get; set; } = 10;
        public decimal PFDeductionPercentage { get; set; } = 12;
        public decimal TaxDeductionPercentage { get; set; } = 10;
    }
}
