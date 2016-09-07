using System.ComponentModel.DataAnnotations;

namespace ContractManagement.WebApi.Types
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public SoftwareEngineerType ContractType { get; set; }

        [Required]
        public byte ExperienceInYears { get; set; }

        [Required]
        public int Salary { get; set; }
    }
}
