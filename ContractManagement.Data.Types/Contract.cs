using System.ComponentModel.DataAnnotations;

namespace ContractManagement.Data.Types
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public SoftwareEngineerType ContractType { get; set; }

        public byte ExperienceInYears { get; set; }

        public int Salary { get; set; }
    }
}
