using System;

namespace ContractManagement.Domain.Models
{
    public class Contract
    {
        private ISalaryPolicyFactory salaryPolicyFactory;
        private ISalaryPolicy salaryPolicy;
        private Salary recommendedSalary;

        public int Id { get; set; }

        public string Name { get; set; }

        public SoftwareEngineerType ContractType { get; set; }

        public byte ExperienceInYears { get; set; }

        public Salary RecommendedSalary
        {
            get
            {
                if (recommendedSalary == null)
                {
                    salaryPolicy = salaryPolicyFactory.GetSalaryPolicy(ContractType);
                    recommendedSalary = salaryPolicy.RecommendSalaryAsync(ExperienceInYears).Result;
                }
                return recommendedSalary;
            }
            set
            {
                if (salaryPolicyFactory != null && salaryPolicy == null)
                {
                    salaryPolicy = salaryPolicyFactory.GetSalaryPolicy(ContractType);
                    var recommended = salaryPolicy.RecommendSalaryAsync(this.ExperienceInYears).Result;
                    if (value.NetSalary < recommended.NetSalary)
                    {
                        throw new ArgumentException($"New salary {value.NetSalary} is lower then the recommended by policy: {recommended.NetSalary}");
                    }
                }
                recommendedSalary = value;
            }
        }
        public Contract()
        {

        }

        public Contract(ISalaryPolicyFactory salaryPolicyFactory)
        {
            this.salaryPolicyFactory = salaryPolicyFactory;

        }


    }
}
