using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public interface ISalaryPolicy
    {
        Task<Salary> RecommendSalaryAsync(byte experienceInYears);

        Salary AdjustSalaryBasedOnWorkExperiance(byte proffessionalExperianceInYears, int minimalNetSalary);

        int GetMinimalSalary(byte proffessionalExperianceInYears);

        bool ContractSatisfiesPolicy(Contract contract);
    }
}