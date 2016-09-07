using System;
using System.Threading.Tasks;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Services;

namespace ContractManagement.DomainServices
{
    public class TesterSalaryPolicy : SalaryPolicyBase, ITesterSalaryPolicy
    {

        public override async Task<Salary> RecommendSalaryAsync(byte proffessionalExperianceInYears)
        {
            var minimalNetSalary = GetMinimalSalary(proffessionalExperianceInYears);
            var recommendedSalary = AdjustSalaryBasedOnWorkExperiance(proffessionalExperianceInYears, minimalNetSalary);
            return await Task.FromResult(recommendedSalary);
        }

        public override Salary AdjustSalaryBasedOnWorkExperiance(byte proffessionalExperianceInYears, int minimalNetSalary)
        {
            return new Salary() { NetSalary = minimalNetSalary + (proffessionalExperianceInYears * 100 + (minimalNetSalary / 4)) };
        }

        public override int GetMinimalSalary(byte proffessionalExperianceInYears)
        {
            if (proffessionalExperianceInYears < 1)
            {
                throw new ProffessionalExperianceTooLowException($"{SoftwareEngineerType.Tester} needs at least 1 year of experiance");
            }

            if (proffessionalExperianceInYears >= 1 && proffessionalExperianceInYears < 2)
            {
                return 2000;
            }

            if (proffessionalExperianceInYears >= 2 && proffessionalExperianceInYears <= 4)
            {
                return 2700;
            }

            if (proffessionalExperianceInYears > 4)
            {
                return 3200;
            }

            throw new NotSupportedException($"Cannot recommend the salary for {SoftwareEngineerType.Tester} with {proffessionalExperianceInYears} years of experiance");
        }

    }
}
