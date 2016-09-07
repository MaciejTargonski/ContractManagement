namespace ContractManagement.Domain.Models
{
    public interface ISalaryPolicyFactory
    {
        ISalaryPolicy GetSalaryPolicy(SoftwareEngineerType softwareEngineerType);
    }
}