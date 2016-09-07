using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractManagement.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ContractManagement.Domain.Models;
using Ploeh.AutoFixture;
using System.Web.Http.Results;
using System.Diagnostics.CodeAnalysis;

namespace ContractManagement.WebApi.Controllers.Tests
{
    [TestClass()]
    [ExcludeFromCodeCoverage]
    public class RecommendedSalaryControllerTests
    {
        Mock<ISalaryPolicyFactory> salaryPolicyFactoryMock;
        Mock<ISalaryPolicy> salaryPolicyMock;
        RecommendedSalaryController controller;
        Fixture fixture;

        [TestInitialize]
        public void Init()
        {
            fixture = new Fixture();
            salaryPolicyFactoryMock = new Mock<ISalaryPolicyFactory>();
            salaryPolicyMock = new Mock<ISalaryPolicy>();
            controller = new RecommendedSalaryController(salaryPolicyFactoryMock.Object);
        }


        [TestMethod()]
        public async Task GetRecommendedSalarySuccess()
        {
            //assign
            var softwareDeveloper = fixture.Create<SoftwareEngineerType>();
            var experiance = fixture.Create<byte>();
            var expectedSalary = new Salary() { NetSalary = fixture.Create<int>() };

            salaryPolicyFactoryMock
                .Setup(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()))
                .Returns(salaryPolicyMock.Object);

            salaryPolicyMock
                .Setup(sp => sp.RecommendSalaryAsync(It.IsAny<byte>()))
                .ReturnsAsync(expectedSalary);

            //act
            var result = await controller.GetRecommendedSalary(softwareDeveloper, experiance);

            //assert
            salaryPolicyFactoryMock
                .Verify(spf => spf.GetSalaryPolicy(softwareDeveloper), Times.Once);

            salaryPolicyMock
                .Verify(sp => sp.RecommendSalaryAsync(experiance), Times.Once);


            var castedResult = result as OkNegotiatedContentResult<Salary>;

            Assert.AreEqual(expectedSalary.NetSalary, castedResult.Content.NetSalary);
        }

        [TestMethod()]
        public async Task GetRecommendedSalaryFailExperianceTooLow()
        {
            //assign
            var softwareDeveloper = fixture.Create<SoftwareEngineerType>();
            var experiance = fixture.Create<byte>();
            var expectedErrorMessage = "UnitTestExceptionMessage";

            salaryPolicyFactoryMock
                .Setup(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()))
                .Returns(salaryPolicyMock.Object);

            salaryPolicyMock
                .Setup(sp => sp.RecommendSalaryAsync(It.IsAny<byte>()))
                .Throws(new ProffessionalExperianceTooLowException(expectedErrorMessage));

            //act
            var result = await controller.GetRecommendedSalary(softwareDeveloper, experiance);

            //assert
            salaryPolicyFactoryMock
                .Verify(spf => spf.GetSalaryPolicy(softwareDeveloper), Times.Once);

            salaryPolicyMock
                .Verify(sp => sp.RecommendSalaryAsync(experiance), Times.Once);


            var castedResult = result as BadRequestErrorMessageResult;

            Assert.AreEqual(expectedErrorMessage, castedResult.Message);
        }

        [TestMethod()]
        public async Task GetRecommendedSalaryFailSoftwareDeveloperTypeNotSupported()
        {
            //assign
            var softwareDeveloper = fixture.Create<SoftwareEngineerType>();
            var experiance = fixture.Create<byte>();
            var expectedErrorMessage = "UnitTestExceptionMessage";

            salaryPolicyFactoryMock
                .Setup(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()))
                .Throws(new NotSupportedException(expectedErrorMessage));

            salaryPolicyMock
                .Setup(sp => sp.RecommendSalaryAsync(It.IsAny<byte>()))
                .Throws(new ProffessionalExperianceTooLowException(expectedErrorMessage));

            //act
            var result = await controller.GetRecommendedSalary(softwareDeveloper, experiance);

            //assert
            salaryPolicyFactoryMock
                .Verify(spf => spf.GetSalaryPolicy(softwareDeveloper), Times.Once);

            salaryPolicyMock
                .Verify(sp => sp.RecommendSalaryAsync(It.IsAny<byte>()), Times.Never);


            var castedResult = result as BadRequestErrorMessageResult;

            Assert.AreEqual(expectedErrorMessage, castedResult.Message);
        }

        [TestMethod()]
        public async Task GetRecommendedSalaryFailSalaryInUndefined()
        {
            //assign
            var softwareDeveloper = fixture.Create<SoftwareEngineerType>();
            var experiance = fixture.Create<byte>();
            Salary expectedSalary = null;


            salaryPolicyFactoryMock
                .Setup(spf => spf.GetSalaryPolicy(It.IsAny<SoftwareEngineerType>()))
                .Returns(salaryPolicyMock.Object);

            salaryPolicyMock
                .Setup(sp => sp.RecommendSalaryAsync(It.IsAny<byte>()))
                .ReturnsAsync(expectedSalary);

            //act
            var result = await controller.GetRecommendedSalary(softwareDeveloper, experiance);

            //assert
            salaryPolicyFactoryMock
                .Verify(spf => spf.GetSalaryPolicy(softwareDeveloper), Times.Once);

            salaryPolicyMock
                .Verify(sp => sp.RecommendSalaryAsync(experiance), Times.Once);


            var castedResult = result as BadRequestErrorMessageResult;

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

    }
}