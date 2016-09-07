using System;
using System.Threading.Tasks;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Services;

namespace ContractManagement.DomainServices
{
    public class ProgrammerSalaryPolicy : SalaryPolicyBase, IProgrammerSalaryPolicy
    {

        public override async Task<Salary> RecommendSalaryAsync(byte proffessionalExperianceInYears)
        {
            var minimalNetSalary = GetMinimalSalary(proffessionalExperianceInYears);
            var recommendedSalary = AdjustSalaryBasedOnWorkExperiance(proffessionalExperianceInYears, minimalNetSalary);

            return await Task.FromResult(recommendedSalary);
        }

        public override Salary AdjustSalaryBasedOnWorkExperiance(byte proffessionalExperianceInYears, int minimalNetSalary)
        {
            return new Salary() { NetSalary = minimalNetSalary + (proffessionalExperianceInYears * 125) };
        }

        public override int GetMinimalSalary(byte proffessionalExperianceInYears)
        {
            if (proffessionalExperianceInYears < 1)
            {
                throw new ProffessionalExperianceTooLowException($"{SoftwareEngineerType.Programmer} needs at least 1 year of experiance");
            }

            if (proffessionalExperianceInYears >= 1 && proffessionalExperianceInYears < 3)
            {
                return 2500;
            }

            if (proffessionalExperianceInYears >= 3 && proffessionalExperianceInYears <= 5)
            {
                return 5000;
            }

            if (proffessionalExperianceInYears > 5)
            {
                return 5500;
            }

            throw new NotSupportedException($"Cannot recommend the salary for {SoftwareEngineerType.Programmer} with {proffessionalExperianceInYears} years of experiance");
        }
        

    }
}
