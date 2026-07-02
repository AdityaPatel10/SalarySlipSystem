using System.ComponentModel.DataAnnotations;

namespace SalarySlipManagementApi.DTOs.SalaryStructureDTOs
{
    public class SalaryStructureUpdateDto
    {
        [Required]
        public decimal BasicSalary { get; set; }

        [Required]
        public decimal HRAPercentage { get; set; }

        [Required]
        public decimal OtherAllowancesPercentage { get; set; }

        [Required]
        public decimal PFDeductionPercentage { get; set; }

        [Required]
        public decimal TaxDeductionPercentage { get; set; }
    }
}
