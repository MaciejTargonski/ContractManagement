namespace ContractManagement.Domain.Models
{
    public class Developer : SoftwareEngineer
    {

        public Developer(ISalaryPolicy salaryPolicy)
            : base(salaryPolicy)
        {
            softwareEngineerTypeEnum = SoftwareEngineerType.Programmer;
        }

    }
}
