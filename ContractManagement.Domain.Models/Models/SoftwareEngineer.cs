namespace ContractManagement.Domain.Models
{
    public abstract class SoftwareEngineer
    {
        private ISalaryPolicy salaryPolicy;
        protected SoftwareEngineerType softwareEngineerTypeEnum;

        public SoftwareEngineer(ISalaryPolicy salaryPolicy)
        {
            this.salaryPolicy = salaryPolicy;
        }
        public byte ExperianceInYears { get; set; }

        public Salary RecommendedSalary
        {
            get
            {
                return salaryPolicy.RecommendSalaryAsync(ExperianceInYears).Result;
            }

        }
    }
}
