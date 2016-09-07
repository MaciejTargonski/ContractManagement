using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractManagement.WebApi.Controllers.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ContractManagement.Domain.Models;
using DataTypes = ContractManagement.Data.Types;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Ploeh.AutoFixture;
using System.Reflection;
using System.Linq.Expressions;
using System.Web.Http.OData;
using ContractManagement.WebApiTests;
using System.Web.Http.OData.Results;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Results;

namespace ContractManagement.WebApi.Controllers.OData.Tests
{
    [TestClass()]
    [ExcludeFromCodeCoverage]
    public class ContractsControllerTests
    {
        Mock<IContractsRepository> contractsRepoMock;
        Mock<ISalaryPolicyFactory> salaryPolicyFactory;
        Mock<ISalaryPolicy> salaryPolicy;
        ContractsController controller;
        Fixture fixture;

        [TestInitialize]
        public void Init()
        {
            AutoMapperManager.Configure();
            fixture = new Fixture();
            contractsRepoMock = new Mock<IContractsRepository>();
            salaryPolicyFactory = new Mock<ISalaryPolicyFactory>();
            salaryPolicy = new Mock<ISalaryPolicy>();
            controller = new ContractsController(contractsRepoMock.Object, salaryPolicyFactory.Object);

            var config = new HttpConfiguration { /* set DI cont here for a mock if needed */ };
            InitHttpRequests(config);
        }

        private void InitHttpRequests(HttpConfiguration config)
        {
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            Mock<HttpRequestMessage> MockHttpRequestMessage;
            MockHttpRequestMessage = new Mock<HttpRequestMessage>();
            var request = MockHttpRequestMessage.Object;

            request.SetConfiguration(config);
            request.RequestUri = new Uri("http://localhost");

            controller.Request = MockHttpRequestMessage.Object;
            controller.Url = new UrlHelper(MockHttpRequestMessage.Object);
        }

        private IQueryable<Contract> InitDbCollection(int count = 5)
        {

            var data = new List<Contract>();

            for (int i = 0; i < count; i++)
            {
                data.Add(fixture.Create<Contract>());
            }

            return data.AsQueryable();

        }

        private bool MethodHasAnAttribute(Expression<Action> expression, Type attribute)
        {
            var methodBody = (MethodCallExpression)expression.Body;
            return methodBody.Method.GetCustomAttributes(attribute, inherit: false).Any();
        }
        [TestMethod()]
        public void GetAllContractsFromDb()
        {

            // assign
            var dbData = InitDbCollection();
            contractsRepoMock.Setup(cr => cr.GetAll()).Returns(dbData);

            // act
            var result = controller.GetContracts();

            // assert
            Assert.IsTrue(
                MethodHasAnAttribute(() => controller.GetContracts()
                , typeof(EnableQueryAttribute))
                , "Method doesn't have an EnableQuery OData attribute. Querying will not be possible on the server side");

            Assert.AreEqual(dbData.Count(), result.Count());


        }



        [TestMethod()]
        public void GetSingleContractTest()
        {
            // assign
            var dbData = InitDbCollection();
            contractsRepoMock.Setup(cr => cr.GetAll()).Returns(dbData);
            var firstEntry = dbData.First();

            // act
            var result = controller.GetContract(firstEntry.Id);

            // assert
            Assert.IsTrue(
                MethodHasAnAttribute(() => controller.GetContracts()
                , typeof(EnableQueryAttribute))
                , "Method doesn't have an EnableQuery OData attribute. Querying will not be possible on the server side");

            Assert.AreEqual(firstEntry.Id, result.Queryable.First().Id);
            Assert.AreEqual(firstEntry.Name, result.Queryable.First().Name);
            Assert.AreEqual((int)firstEntry.ContractType, (int)result.Queryable.First().ContractType);
            Assert.AreEqual(firstEntry.ExperienceInYears, result.Queryable.First().ExperienceInYears);
            Assert.AreEqual(firstEntry.RecommendedSalary.NetSalary, result.Queryable.First().Salary);

        }

        [TestMethod()]
        public async Task PostNewContractTest()
        {
            // assign
            var dataToPost = fixture.Create<WebApi.Types.Contract>();

            var dataSavedToDb = fixture.Create<Contract>();

            contractsRepoMock.Setup(cr => cr.AddSingle(It.IsAny<Contract>()))
                .Callback((Contract contract) => {

                    Assert.AreEqual(dataToPost.ExperienceInYears, contract.ExperienceInYears);
                    Assert.AreEqual((int)dataToPost.ContractType, (int)contract.ContractType);
                    Assert.AreEqual(dataToPost.Name, contract.Name);
                    Assert.AreEqual(dataToPost.Salary, contract.RecommendedSalary.NetSalary);
                    Assert.AreEqual(dataToPost.Id, contract.Id);

                }).Returns(dataSavedToDb);

            contractsRepoMock.Setup(cr => cr.SaveChangesAsync()).Returns(Task.CompletedTask);

            salaryPolicy.Setup(sp => sp.ContractSatisfiesPolicy(It.IsAny<Contract>()))
                .Returns(true);

            salaryPolicyFactory.Setup(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()))
                .Returns(salaryPolicy.Object);

            // act
            var result = await controller.Post(dataToPost);
            var castedResult = result as CreatedODataResult<Types.Contract>;

            // assert
            Assert.IsInstanceOfType(result, typeof(CreatedODataResult<WebApi.Types.Contract>));
            Assert.AreEqual(dataSavedToDb.Id, castedResult.Entity.Id);

            salaryPolicy.Verify(sp => sp.ContractSatisfiesPolicy(It.IsAny<Contract>()), Times.Once);

            salaryPolicyFactory.Verify(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()), Times.Once);
        }

        [TestMethod()]
        public async Task PostNewContractSalaryDoesMeetPolicy()
        {
            // assign
            var dataToPost = fixture.Create<WebApi.Types.Contract>();

            var dataSavedToDb = fixture.Create<Contract>();

            contractsRepoMock.Setup(cr => cr.AddSingle(It.IsAny<Contract>()))
                .Callback((Contract contract) => {

                    Assert.AreEqual(dataToPost.ExperienceInYears, contract.ExperienceInYears);
                    Assert.AreEqual((int)dataToPost.ContractType, (int)contract.ContractType);
                    Assert.AreEqual(dataToPost.Name, contract.Name);
                    Assert.AreEqual(dataToPost.Salary, contract.RecommendedSalary.NetSalary);
                    Assert.AreEqual(dataToPost.Id, contract.Id);

                }).Returns(dataSavedToDb);

            contractsRepoMock.Setup(cr => cr.SaveChangesAsync()).Returns(Task.CompletedTask);

            salaryPolicy.Setup(sp => sp.ContractSatisfiesPolicy(It.IsAny<Contract>()))
                .Returns(false);

            salaryPolicyFactory.Setup(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()))
                .Returns(salaryPolicy.Object);

            // act
            var result = await controller.Post(dataToPost);

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var castedResult = result as BadRequestErrorMessageResult;

            salaryPolicy.Verify(sp => sp.ContractSatisfiesPolicy(It.IsAny<Contract>()), Times.Once);

            salaryPolicyFactory.Verify(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()), Times.Once);
        }

    }
}