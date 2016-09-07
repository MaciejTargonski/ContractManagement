using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using ContractManagement.Data.Services;
using ContractManagement.Domain.Models;
using ContractManagement.DomainServices;

namespace ContractManagement.WebApi
{
    public static class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ContractManagementDbContext>()
                .As<IContractManagementDbContext>()
                .InstancePerRequest();

            builder.RegisterType<EntityFrameworkContractsRepository>()
                .As<IContractsRepository>()
                .InstancePerRequest();

            builder.RegisterType<ProgrammerSalaryPolicy>()
                .As<IProgrammerSalaryPolicy>()
                .InstancePerRequest();

            builder.RegisterType<TesterSalaryPolicy>()
                .As<ITesterSalaryPolicy>()
                .InstancePerRequest();

            builder.RegisterType<SalaryPolicyFactory>()
                .As<ISalaryPolicyFactory>()
                .InstancePerRequest();


            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
