namespace ContractManagement.Domain.Models
{
    public class Tester : SoftwareEngineer
    {

        public Tester(ISalaryPolicy salaryPolicy)
            : base(salaryPolicy)
        {
            softwareEngineerTypeEnum = SoftwareEngineerType.Tester;
        }

    }
}
