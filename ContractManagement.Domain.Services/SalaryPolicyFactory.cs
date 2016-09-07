using System;
using ContractManagement.Domain.Models;

namespace ContractManagement.DomainServices
{
    public class SalaryPolicyFactory : ISalaryPolicyFactory
    {
        private IProgrammerSalaryPolicy programmerSalaryPolicy;
        private ITesterSalaryPolicy testerSalaryPolicy;

        public SalaryPolicyFactory(IProgrammerSalaryPolicy programmerSalaryPolicy, ITesterSalaryPolicy testerSalaryPolicy)
        {
            this.programmerSalaryPolicy = programmerSalaryPolicy;
            this.testerSalaryPolicy = testerSalaryPolicy;
        }
        public ISalaryPolicy GetSalaryPolicy(SoftwareEngineerType softwareEngineerType)
        {
            switch (softwareEngineerType)
            {
                case SoftwareEngineerType.Programmer:
                    return programmerSalaryPolicy;

                case SoftwareEngineerType.Tester:
                    return testerSalaryPolicy;

                default: throw new NotSupportedException($"Cannot find SalaryPolicy for software developer - {softwareEngineerType}");
            }
        }
    }
}
