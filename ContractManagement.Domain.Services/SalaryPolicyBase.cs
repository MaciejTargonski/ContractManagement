using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Services
{
    public abstract class SalaryPolicyBase : ISalaryPolicy
    {
        public abstract Salary AdjustSalaryBasedOnWorkExperiance(byte proffessionalExperianceInYears, int minimalNetSalary);
        public abstract int GetMinimalSalary(byte proffessionalExperianceInYears);
        public abstract Task<Salary> RecommendSalaryAsync(byte experienceInYears);
        public virtual bool ContractSatisfiesPolicy(Contract contract)
        {
            var recommendedSalary = RecommendSalaryAsync(contract.ExperienceInYears).Result;

            return contract.RecommendedSalary.NetSalary >= recommendedSalary.NetSalary;
        }
    }
}
