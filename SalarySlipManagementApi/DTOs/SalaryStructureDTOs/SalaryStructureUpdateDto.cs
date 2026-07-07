using System.ComponentModel.DataAnnotations;

namespace SalarySlipManagementApi.DTOs.SalaryStructureDTOs
{
    public class SalaryStructureUpdateDto
    {
        [Required]
        public decimal BasicSalary { get; set; }

        [Required]
        public decimal HRAPercentage { get; set; } = 20;

        [Required]
        public decimal OtherAllowancesPercentage { get; set; } = 10;

        [Required]
        public decimal PFDeductionPercentage { get; set; } = 12;

        [Required]
        public decimal TaxDeductionPercentage { get; set; } = 10;
    }
}
