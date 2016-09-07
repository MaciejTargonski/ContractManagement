using AutoMapper;
using ContractManagement.Domain.Models;

namespace ContractManagement.WebApi.App_Start
{
    public static class AutoMapperManager
    {
        public static void Configure()
        {

            Mapper.Initialize(
                cnf =>
                {


                    cnf.CreateMap<Contract, WebApi.Types.Contract>()
                    .ForMember(c => c.ContractType, opt => opt.MapFrom(x => (WebApi.Types.SoftwareEngineerType)(int)x.ContractType))
                    .ForMember(c => c.Salary, opt => opt.MapFrom(x => x.RecommendedSalary.NetSalary));

                    cnf.CreateMap<WebApi.Types.Contract, Contract>()
                    .ForMember(c => c.ContractType, opt => opt.MapFrom(x => (SoftwareEngineerType)(int)x.ContractType))
                    .ForMember(c => c.RecommendedSalary, opt => opt.UseValue(new Salary()))
                    .AfterMap((src, dest) =>
                    {
                        dest.RecommendedSalary.NetSalary = src.Salary;
                    });

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