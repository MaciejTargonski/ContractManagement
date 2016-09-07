namespace ContractManagement.WebApi.Migrations
{
    using ContractManagement;
    using ContractManagement.Data.Types;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ContractManagement.ContractManagementDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ContractManagement.ContractManagementDbContext context)
        {
            SeedContracts(context);
        }

        private void SeedContracts(ContractManagementDbContext context)
        {
            context.Contracts.AddOrUpdate(
                            c => c.Name,
                            new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 3,
                                Name = "3yo programmer name",
                                Salary = 5375
                            },
                            new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 3,
                                Name = "3yo programmer name 2",
                                Salary = 5375
                            },
                            new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 3,
                                Name = "3yo programmer name 3",
                                Salary = 5375
                            },
                            new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 3,
                                Name = "3yo programmer name 4",
                                Salary = 5375
                            },
                            new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 3,
                                Name = "3yo programmer name 5",
                                Salary = 5375
                            },
                            new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 6,
                                Name = "6yo programmer name",
                                Salary = 6250
                            }, new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 6,
                                Name = "6yo programmer name 2",
                                Salary = 6250
                            }, new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 6,
                                Name = "6yo programmer name 3",
                                Salary = 6250
                            }, new Contract
                            {
                                ContractType = SoftwareEngineerType.Programmer,
                                ExperienceInYears = 6,
                                Name = "6yo programmer name 4",
                                Salary = 6250
                            }, new Contract
                            {
                                ContractType = SoftwareEngineerType.Tester,
                                ExperienceInYears = 1,
                                Name = "1yo tester name",
                                Salary = 2600
                            }
                            );
        }
    }
}
