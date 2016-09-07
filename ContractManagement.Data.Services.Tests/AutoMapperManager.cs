using AutoMapper;
using ContractManagement.Domain.Models;

namespace ContractManagement.Data.Services.Tests
{
    public static class AutoMapperManager
    {
        public static void Configure()
        {

            Mapper.Initialize(
                cnf =>
                {

                    cnf.CreateMap<Data.Types.Contract, Contract>()
                        .ConstructUsingServiceLocator()
                        .ForMember(c => c.ContractType, opt => opt.MapFrom(x => (SoftwareEngineerType)(int)x.ContractType))
                        .ForMember(c => c.RecommendedSalary, opt => opt.MapFrom(x => new Salary() { NetSalary = x.Salary }));

                    cnf.CreateMap<Contract, Data.Types.Contract>()
                        .ForMember(c => c.ContractType, opt => opt.MapFrom(x => (Data.Types.SoftwareEngineerType)(int)x.ContractType))
                        .ForMember(c => c.Salary, opt => opt.MapFrom(x => x.RecommendedSalary.NetSalary));
                }

                );

        }
    }
}