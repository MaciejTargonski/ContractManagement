using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractManagement.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Moq;
using System.Data.Entity;
using ContractManagement.Domain.Models;

namespace ContractManagement.Data.Services.Tests
{
    [TestClass()]
    public class EntityFrameworkContractsRepositoryTests
    {
        Fixture fixture;
        Mock<IContractManagementDbContext> dbContextMock;
        Mock<DbSet<Types.Contract>> contractDbSetMock;

        EntityFrameworkContractsRepository entityFrameworkContractsRepository;

        [ClassInitialize]
        public static void TestsInit(TestContext context)
        {
            AutoMapperManager.Configure();
        }

        [TestInitialize]
        public void Init()
        {
            fixture = new Fixture();

            dbContextMock = new Mock<IContractManagementDbContext>();

            contractDbSetMock = new Mock<DbSet<Types.Contract>>();
            dbContextMock.Setup(db => db.Contracts).Returns(contractDbSetMock.Object);


            entityFrameworkContractsRepository = new EntityFrameworkContractsRepository(dbContextMock.Object);
        }

        private IQueryable<Types.Contract> InitContractDbSet(int count = 5)
        {

            var data = new List<Types.Contract>();

            for (int i = 0; i < count; i++)
            {
                data.Add(fixture.Create<Types.Contract>());
            }

            var qdata = data.AsQueryable();

            contractDbSetMock.As<IQueryable<Types.Contract>>().Setup(m => m.Provider).Returns(qdata.Provider);
            contractDbSetMock.As<IQueryable<Types.Contract>>().Setup(m => m.Expression).Returns(qdata.Expression);
            contractDbSetMock.As<IQueryable<Types.Contract>>().Setup(m => m.ElementType).Returns(qdata.ElementType);
            contractDbSetMock.As<IQueryable<Types.Contract>>().Setup(m => m.GetEnumerator()).Returns(qdata.GetEnumerator());

            return qdata;
        }

        [TestMethod()]
        public void GetAllContractsFromEfRepositorySuccess()
        {
            // assign
            var seedData = InitContractDbSet();

            var expectedType = typeof(EnumerableQuery<Domain.Models.Contract>);

            // act
            var results = entityFrameworkContractsRepository.GetAll();

            // assert
            Assert.IsInstanceOfType(results, expectedType);
            Assert.AreEqual(seedData.Count(), results.Count());
            foreach (var expected in seedData)
            {
                var found = results.FirstOrDefault(r => r.Id == expected.Id);
                Assert.IsNotNull(found, "Cannot find seedData in results");

                Assert.AreEqual(expected.Name, found.Name);
                Assert.AreEqual((int)expected.ContractType, (int)found.ContractType);
                Assert.AreEqual(expected.ExperienceInYears, found.ExperienceInYears);
                Assert.AreEqual(expected.Salary, found.RecommendedSalary.NetSalary);
            }
        }

        [TestMethod()]
        public void AddSingleContractToEfRepositorySuccess()
        {
            // assign

            var contractToAdd = fixture.Create<Contract>();

            contractDbSetMock.Setup(m => m.Add(It.IsAny<Data.Types.Contract>()))
                .Callback((Data.Types.Contract contract) =>
                {
                    Assert.AreEqual(contractToAdd.Id, contract.Id);
                    Assert.AreEqual(contractToAdd.Name, contract.Name);
                    Assert.AreEqual((int)contractToAdd.ContractType, (int)contract.ContractType);
                    Assert.AreEqual(contractToAdd.ExperienceInYears, contract.ExperienceInYears);
                    Assert.AreEqual(contractToAdd.RecommendedSalary.NetSalary, contract.Salary);
                }
                );

            // act
            entityFrameworkContractsRepository.AddSingle(contractToAdd);

            // assert
            contractDbSetMock.Verify(m => m.Add(It.IsAny<Data.Types.Contract>()), Times.Once);
        }

        [TestMethod()]
        public async Task SaveChangesInEfRepository()
        {
            // assign

            dbContextMock.Setup(m => m.SaveChangesAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // act
            await entityFrameworkContractsRepository.SaveChangesAsync();

            // assert
            dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod()]
        public void DisposeEfRepository()
        {
            // assign

            dbContextMock.Setup(m => m.Dispose())
                .Verifiable();

            // act
            entityFrameworkContractsRepository.Dispose();

            // assert
            dbContextMock.Verify(m => m.Dispose(), Times.Once);
        }
    }
}